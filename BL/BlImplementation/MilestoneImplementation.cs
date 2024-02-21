using BlApi;
using BO;

namespace BlImplementation;

internal class MilestoneImplementation : IMilestone
{
    private static readonly DalApi.IDal _dal = DalApi.Factory.Get;

    #region private help functions
    private static IEnumerable<PlanMileston>? milestonesList = null;

    private class PlanMileston
    {
        public int MilestoneTaskId { get; set; }
        public IEnumerable<int> PreviousTasks { get; set; }
    }
    private static void CreateMilestones()
    {
        static int getNextId() { int id = 1; return id++; }

        var dependenciesList = _dal.Dependency.ReadAll();
        _dal.Dependency.Reset();

        var tasks = from t in _dal.Task.ReadAll() select t.Id;

        var dependentGroups = from DO.Dependency d in dependenciesList
                              group d by d.DependentTask into gs
                              select new
                              {
                                  key = gs.Key,
                                  tasks = (from g in gs
                                           orderby g.DependsOnTask
                                           select g.DependsOnTask)
                              };

        var distinctGroups = dependentGroups.Select(d => d.tasks).Distinct();

        int firstMilestonId = _dal.Task.Create(new(0, "", "Start", true, null, null, null, null, null, null, "", "", 0, 0));

        tasks
            .Where(t => !dependenciesList.Any(d => d.DependentTask == t))
            .ToList()
            .ForEach(t => _dal.Dependency.Create(new(0, t, firstMilestonId)));

        var milestones = distinctGroups.Select(previousTasks => new PlanMileston
        {
            MilestoneTaskId = _dal.Task.Create(new(0, "", $"M{getNextId()}", true, null, null, null, null, null, null, "", "", 0, 0)),
            PreviousTasks = previousTasks
        }).ToList();

        var lastMileston = new PlanMileston
        {
            MilestoneTaskId = _dal.Task.Create(new(0, "", "End", true, null, null, null, null, null, null, "", "", 0, 0)),
            PreviousTasks = tasks.Where(t => !dependenciesList.Any(d => d.DependsOnTask == t))
        };

        milestones.Add(lastMileston);

        var milestonesDependencies = from m in milestones
                                     from t in m.PreviousTasks
                                     select (_dal.Dependency.Create(new(0, m.MilestoneTaskId, t)));

        var tasksDependencies = from milestone in milestones
                                from dependency in dependentGroups
                                where milestone.PreviousTasks.SequenceEqual(dependency.tasks)
                                select (_dal.Dependency.Create(new(0, dependency.key, milestone.MilestoneTaskId)));

        milestonesList = milestones;
    }
 private static void CleaerIllegalSchedual()
    {
        _dal.Task.ReadAll()
            .ToList()
            .ForEach(t => _dal.Task.Update(t with { DeadlineDate = null, SchedualDate = null }));
    }

    private DateTime ScheduleEnd(DO.Task lastMileston, DateTime end)
    {
        var prevMilestons = SetEndDate(lastMileston, end);
        foreach (var (prevMileston, nextEndDate) in prevMilestons)
        {
            if (prevMileston != null)
                ScheduleEnd(prevMileston, nextEndDate);
        }
        return prevMilestons.Min(m => m.nextEndDate);
    }
    private static IEnumerable<(DO.Task? prevMileston, DateTime nextEndDate)> SetEndDate(DO.Task milestone, DateTime end)
    {
        if (milestone.DeadlineDate != null && milestone.DeadlineDate > end)
            end = milestone.DeadlineDate.Value;
        else
            _dal.Task.Update(milestone with { DeadlineDate = end });

        var prevMilestons = _dal.Dependency
            .ReadAll(d => d.DependentTask == milestone.Id)
            .GroupBy(d => d.DependsOnTask)
            .Select(g =>
            {
                var prevMileston = _dal.Task.Read(g.Key);
                var prevTasks = g.Select(d => _dal.Task.Read(d.DependsOnTask));
                var endDates = prevTasks
                .Select(task =>
                {
                    var endDate = task!.DeadlineDate == null || task.DeadlineDate > end ? end : task!.DeadlineDate;
                    var nextEndDate = endDate?.Subtract(task.Duration!.Value);
                    _dal.Task.Update(task! with { DeadlineDate = endDate });
                    return nextEndDate;
                });
                return (
                    prevMileston,
                    nextEndDate: endDates.Min()!.Value
                ); ;
            });
        return prevMilestons;
    }

    private DateTime ScheduleStart(DO.Task firstMileston, DateTime start)
    {
        var nextMilestons = SetStartDate(firstMileston, start);
        foreach (var (nextMileston, nextStartDate) in nextMilestons)
        {
            if (nextMileston != null)
                ScheduleStart(nextMileston, nextStartDate);
        }
        return nextMilestons.Max(m => m.nextStartDate);
    }
    private static IEnumerable<(DO.Task? nextMileston, DateTime nextStartDate)> SetStartDate(DO.Task milestone, DateTime start)
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
        return nextMilestons.First();
    }

    #endregion
    public void CreateSchedule(DateTime start, DateTime end)
    {
        _dal.SetProjectStatus(ProjectStatus.Scheduling);
        if (_dal.StartDate == null || _dal.EndDate == null || _dal.GetProjectStatus() != ProjectStatus.Planning) throw new BlIllegalException("Schedule", "creation");
        if (milestonesList == null) CreateMilestones();
        else CleaerIllegalSchedual();
        var lastMileston = _dal.Task.Read(milestonesList!.Last().MilestoneTaskId)!;
        var scheduledStartDate = ScheduleEnd(lastMileston, end);
        if (scheduledStartDate < start) throw new BlIllegalException("Schedule", "creation");
        var scheduledEndDate = ScheduleStart(_dal.Task.Read(t => t.Alias == "Start")!, start);
        if (scheduledEndDate > end) throw new BlIllegalException("Schedule", "creation");
        _dal.SetProjectStatus(ProjectStatus.Execution);
    }
    private Milestone GetMilestone(DO.Task task)
    {
        var milestone = task.Convert<DO.Task, Milestone>();
        return setValues(milestone);
    }

    private Milestone setValues(Milestone milestone)
    {
        
    }

    public Milestone GetMilestone(int id)
    {
        DO.Task? task = _dal.Task.Read(id);
        if (task == null || !task.IsMilestone) throw new BlDoesNotExistException("Milestone");
        return GetMilestone(task);
    }
    public Milestone UpdateMilestone(int id, string alias, string description, string? comments)
    {
        DO.Task? toUpdate = _dal.Task.Read(id);
        if (toUpdate == null || !toUpdate.IsMilestone) throw new BlDoesNotExistException("Milestone");
        description = string.IsNullOrEmpty(description)? toUpdate.Description : description;
        alias = string.IsNullOrEmpty(alias) ? toUpdate.Alias : alias;
        comments = string.IsNullOrEmpty(comments) ? toUpdate.Remarks : comments;
        toUpdate = toUpdate with { Alias = alias, Description = description, Remarks = comments };
        _dal.Task.Update(toUpdate);
        return GetMilestone(toUpdate);  
    }
}
