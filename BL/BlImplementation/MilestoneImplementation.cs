using BlApi;
using BO;

namespace BlImplementation;
internal class MilestoneImplementation : IMilestone
{
    private static readonly DalApi.IDal _dal = DalApi.Factory.Get;

    /// <summary>
    /// Private inner class that implements IEqualityComparer to enable the distinct operation on <IEnumerable<T>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    private class CollectionComparer<T> : IEqualityComparer<IEnumerable<T>>
    {
        public bool Equals(IEnumerable<T>? x, IEnumerable<T>? y)
        {
            return x!.SequenceEqual(y!);
        }

        public int GetHashCode(IEnumerable<T> obj)
        {
            return obj.Aggregate(17, (hash, item) => hash * 31 + item!.GetHashCode());
        }
    }

    #region private help functions

    /// <summary>
    /// In case an attempt is made to create a timetable that fails, 
    /// the function will delete the incorrect data saved in the database.
    /// </summary>
    private static void CleaerIllegalSchedual()
    {
        _dal.Task.ReadAll()
            .ToList()
            .ForEach(t => _dal.Task.Update(t with { DeadlineDate = null, SchedualDate = null }));
    }

    /// <summary>
    /// The function takes the list of dependencies and creates in the database a new list of dependencies between tasks and milestones and vice versa.
    /// </summary>
    private static void CreateMilestones()
    { 
        //Milestone runner ID number
        int nextId = 1;
        int getNextId() => nextId++;

        //List of all existing dependencies
        var dependenciesList = _dal.Dependency.ReadAll().ToList();

        //Deleting the dependency pool
        _dal.Dependency.Reset();

        //ID numbers of all tasks
        var tasks = (from t in _dal.Task.ReadAll()
                     select t.Id).ToList();


        //A collection that contains for each dependent task:
        //Key: ID number of pending task
        //Tasks: collection of ID numbers of the previous tasks.

        var dependentGroups = from DO.Dependency d in dependenciesList
                              group d by d.DependentTask into gs
                              select new
                              {
                                  key = gs.Key,
                                  tasks = gs.Select(g => g.DependsOnTask).OrderBy(task => task)
                              };



        //A collection of lists of previous tasks without duplicates
        var distinctGroups = dependentGroups.Select(d => d.tasks).Distinct(new CollectionComparer<int>());

        //Creating a first milestone
        int firstMilestonId = _dal.Task.Create(new(0, "", "Start", true, null, null, null, null, null, null, "", "", 0, 0));

        //Create a dependency between each task that does not depend on any task, to the first milestone.
        tasks
            .Where(t => !dependenciesList.Any(d => d.DependentTask == t))
            .ToList()
            .ForEach(t => _dal.Dependency.Create(new(0, t, firstMilestonId)));

        //Create a new milestone for each list of previous tasks
        var milestones = distinctGroups.Select(previousTasks => new
        {
            MilestoneTaskId = _dal.Task.Create(new(0, "", $"M{getNextId()}", true, null, null, null, null, null, null, "", "", 0, 0)),
            PreviousTasks = previousTasks
        }).ToList();

        //Create a final milestone and add it to the list of milestones
        //Its previous task list is all tasks that no task is dependent on
        var lastMileston = new
        {
            MilestoneTaskId = _dal.Task.Create(new(0, "", "End", true, null, null, null, null, null, null, "", "", 0, 0)),
            PreviousTasks = tasks.Where(t => !dependenciesList.Any(d => d.DependsOnTask == t))
        };
        milestones.Add(lastMileston);

        //Create a new dependency between a milestone and each of its previous tasks
        var milestonesDependencies = (from m in milestones
                                      from t in m.PreviousTasks
                                      select (_dal.Dependency.Create(new(0, m.MilestoneTaskId, t)))).ToList();

        //Create a dependency between each task and a milestone corresponding to its list of previous tasks.
        var tasksDependencies = (from milestone in milestones
                                 from dependency in dependentGroups
                                 where milestone.PreviousTasks.SequenceEqual(dependency.tasks)
                                 select (_dal.Dependency.Create(new(0, dependency.key, milestone.MilestoneTaskId)))).ToList();
        
    }

    /// <summary>
    /// The function receives a milestone, and updates the deadline date for the milestone and for all tasks preceding it.
    /// </summary>
    /// <param name="milestone">Milestone DAL object</param>
    /// <param name="end">The deadline date for updating</param>
    /// <returns>A list of the next milestones in line, and for each a calculated deadline.</returns>
    private static IEnumerable<(DO.Task? prevMileston, DateTime nextEndDate)>? SetEndDate(DO.Task milestone, DateTime end)
    {
        //In case the last date has already been updated in the milestone and it is earlier than the end,
        //we will update the previous tasks with the existing date in the milestone.
        if (milestone.DeadlineDate != null && milestone.DeadlineDate < end)
            end = milestone.DeadlineDate.Value;
        else
            _dal.Task.Update(milestone with { DeadlineDate = end });

        var prevMilestons = _dal.Dependency
            //All dependencies in which the milestone is the dependent task
            .ReadAll(d => d.DependentTask == milestone.Id)
            //All the milestones that the tasks depend on
            //(the tasks that the current milestone depends on)
            .Select(task => _dal.Dependency
            .ReadAll(milestone => milestone.DependentTask == task.DependsOnTask)
            //Grouped by milestone. Each tasks group depends on the same milestone. (avoid duplication)
            .GroupBy(d => d.DependsOnTask)
            .Select(g =>
            {
                //The Milestone DAL object
                var prevMileston = _dal.Task.Read(g.Key);
                //Task objects that depend on the milestone
                var prevTasks = g.Select(d => _dal.Task.Read(d.DependentTask)).ToList();
                if(prevTasks == null)
                    return (
                   prevMileston,
                   nextEndDate: end
               );
                //All possible deadline dates for the milestone that the tasks depend on.
                var endDates = prevTasks
                .Select(task =>
                {
                    //Ensure that the deadline date to be updated for the task is no later than the existing date, if any.
                    var endDate = task!.DeadlineDate == null || task.DeadlineDate > end ? end : task!.DeadlineDate;
                    //The possible deadline date for the milestone
                    var nextEndDate = endDate?.Subtract(task.Duration!.Value);
                    //Update the deadline date for the task
                    _dal.Task.Update(task! with { DeadlineDate = endDate });
                    return nextEndDate;
                });
                //Returns an object:
                //Milestone, and the last update date - the minimum of all possible dates.
                return (
                    prevMileston,
                    nextEndDate: endDates.Min()!.Value
                );
            }));

        return prevMilestons.Any()? prevMilestons.First():null;
    }

    /// <summary>
    /// A recursive function that updates last possible finish dates for tasks and milestones, from the last milestone and back.
    /// </summary>
    /// <param name="lastMileston">The last milestone</param>
    /// <param name="end">Project completion date</param>
    /// <returns>the earliest end date  of all end dates.</returns>
    private static DateTime ScheduleEnd(DO.Task lastMileston, DateTime end)
    {
        var prevMilestons = SetEndDate(lastMileston, end);
        if (prevMilestons == null) return end;
        foreach (var (prevMileston, nextEndDate) in prevMilestons)
        {
            if (prevMileston != null)
                ScheduleEnd(prevMileston, nextEndDate);
        }
        return prevMilestons.Min(m => m.nextEndDate);
    }

    /// <summary>
    /// The function does the same as the SetEndDate functionת
    /// only that the date to be updated is a planned date for starting work,
    /// and each time we will update the milestone, and the tasks that depend on it.
    /// </summary>
    /// <param name="milestone">mileston Dal object</param>
    /// <param name="start">Project start date</param>
    /// <returns> a list of next milestones and start dates accordingly.</returns>
    private static IEnumerable<(DO.Task? nextMileston, DateTime nextStartDate)>? SetStartDate(DO.Task milestone, DateTime start)
    {
        if (milestone.SchedualDate != null && milestone.SchedualDate > start)
            start = milestone.SchedualDate.Value;
        else
            _dal.Task.Update(milestone with { SchedualDate = start });

        var nextMilestons = _dal.Dependency
            .ReadAll(d => d.DependsOnTask == milestone.Id)
            .Select(task => _dal.Dependency.ReadAll(milestone => milestone.DependsOnTask == task.DependentTask)
            .GroupBy(d => d.DependentTask)
            .Select(g =>
            {
                var nextMileston = _dal.Task.Read(g.Key);
                var prevTasks = g.Select(d => _dal.Task.Read(d.DependsOnTask));
                if(prevTasks ==  null)
                    return (
                   nextMileston,
                   nextStartDate: start
               );
                var startDates = prevTasks
                .Select(task =>
                        {
                            var startDate = task!.SchedualDate == null || task.SchedualDate < start ? start : task.SchedualDate.Value;
                            var nextStartDate = startDate.Add(task.Duration!.Value);
                            _dal.Task.Update(task! with { SchedualDate = startDate });
                            return nextStartDate;
                        });
                return (
                    nextMileston,
                    nextStartDate: startDates.Max()
                );
            }));
        return nextMilestons.Any() ? nextMilestons.First() : null;
    }
    
    /// <summary>
    /// A recursive function to calculate and update a planned start date for milestones and tasks.
    /// </summary>
    /// <param name="firstMileston"></param>
    /// <param name="start"></param>
    /// <returns></returns>
    private static DateTime ScheduleStart(DO.Task firstMileston, DateTime start)
    {
        var nextMilestons = SetStartDate(firstMileston, start);
        if (null == nextMilestons) return start;
        foreach (var (nextMileston, nextStartDate) in nextMilestons)
        {
            if (nextMileston != null)
                ScheduleStart(nextMileston, nextStartDate);
        }
        return nextMilestons.Max(m => m.nextStartDate);
    }
    
    private static Status GetStatus(double percentage) => percentage == 0.0 ? Status.Scheduled : percentage == 100.0 ? Status.Done : Status.OnTrack;
    
    private static Milestone SetValues(DO.Task task)
    {

        Milestone milestone =  task.Convert<DO.Task, Milestone>();
        milestone.Dependencies = _dal.GetListOfTasks(task);
        int doneTasks = milestone.Dependencies.Where(t => t.Status == Status.Done).Count();
        milestone.CompletionPercentage = doneTasks != 0? doneTasks / milestone.Dependencies.Count * 100.0 : 0.0;
        milestone.Status = GetStatus((double)milestone.CompletionPercentage);
        return milestone;
    }
    #endregion

    public bool ScheduleExists { get => _dal.GetProjectStatus() == ProjectStatus.Execution; }

    /// <summary>
    /// A function to create a project schedule automatically
    /// </summary>
    /// <param name="start">Requested date for the start of the project</param>
    /// <param name="end">Requested date for the end of the project</param>
    /// <exception cref="BlIllegalException">Failure to create schedule</exception>
    public void CreateSchedule(DateTime start, DateTime end)
    {
        //Checking that the project is not running
        if (ScheduleExists)
            throw new BlIllegalException("Schedule", "creation", "A project not in the planning stage.");
        //If we have already tried to create a schedule, we will clear the previous attempt.
        if (_dal.GetProjectStatus() == ProjectStatus.Scheduling)
            CleaerIllegalSchedual();
        //Otherwise we will create milestones and change project status
        else
        {
            CreateMilestones();
            _dal.SetProjectStatus(ProjectStatus.Scheduling);
        }
        DO.Task? _lastMilestone = _dal.Task.Read(t=>t.Alias== "End");
        //We will check the correctness of the dates and put values in the tasks, the milestones, the project dates, and the project status.
        if (ScheduleEnd(_lastMilestone!, end) < start)
            throw new BlIllegalException("Schedule", "creation", "End date too early.");
        if (ScheduleStart(_dal.Task.Read(t => t.Alias == "Start")!, start) > end)
            throw new BlIllegalException("Schedule", "creation", "Start date too late.");
        _dal.StartDate = start;
        _dal.EndDate = end;
        _dal.SetProjectStatus(ProjectStatus.Execution);
    }
    
    public Milestone GetMilestone(int id)
    {
        DO.Task? task = _dal.Task.Read(id);
        if (task == null || !task.IsMilestone) throw new BlDoesNotExistException("Milestone");
        return SetValues(task);
    }
    
    public Milestone UpdateMilestone(int id, string alias, string description, string? comments)
    {
        DO.Task? toUpdate = _dal.Task.Read(id);
        if (toUpdate == null || !toUpdate.IsMilestone)
            throw new BlDoesNotExistException("Milestone");
        description = string.IsNullOrEmpty(description) ? toUpdate.Description : description;
        alias = string.IsNullOrEmpty(alias) ? toUpdate.Alias : alias;
        comments = string.IsNullOrEmpty(comments) ? toUpdate.Remarks : comments;
        toUpdate = toUpdate with { Alias = alias, Description = description, Remarks = comments };
        _dal.Task.Update(toUpdate);
        return SetValues(toUpdate);
    }
}
