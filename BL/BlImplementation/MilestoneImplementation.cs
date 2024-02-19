using BlApi;
using BO;

namespace BlImplementation;

internal class MilestoneImplementation : IMilestone
{
    private static readonly DalApi.IDal _dal = DalApi.Factory.Get;

    private static bool _alreadyCreated = false;

    private class PlanMileston
    {
        public int MilestoneTaskId { get; set; }
        public IEnumerable<int> PreviousTasks { get; set; }
    }
    private static IEnumerable<PlanMileston> CreateMilestones()
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

        return milestones;

    }

    public void CreateSchedule(DateTime start, DateTime end)
    {
        if (_alreadyCreated) return;
        if (_dal.StartDate == null || _dal.EndDate == null || _dal.GetProjectStatus() != ProjectStatus.Planning) throw new BlIllegalException("Schedule", "creation");
        _alreadyCreated = true;
        //TODO handle second try
        var milestones = CreateMilestones();
        //TODO clear all schedule
        var lastMileston = _dal.Task.Read(milestones.Last().MilestoneTaskId)!;
        var scheduledStartDate = ScheduleEnd(lastMileston, end);
        if (scheduledStartDate < start) throw new BlIllegalException("Schedule", "creation");
        //var scheduledEndDate = ScheduleStart(lastMileston, end);


    }
    private DateTime ScheduleEnd(DO.Task lastMileston, DateTime end)
    {
        var prevMilestons = SetEndDate(lastMileston, end);
        foreach (var m in prevMilestons)
        {
            if (m.prevMileston != null)
                ScheduleEnd(m.prevMileston, m.nextEndDate);
        }
        return prevMilestons.Min(m => m.nextEndDate);
    }
    private IEnumerable<(DO.Task? prevMileston, DateTime nextEndDate)> SetEndDate(DO.Task milestone, DateTime end)
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

    //private DateTime ScheduleStart(DO.Task firstMileston, DateTime start)
    //{
    //    var nextMilestons = SetStartDate(firstMileston, start);
    //    foreach (var m in nextMilestons)
    //    {
    //        if (m.prevMileston != null)
    //            ScheduleEnd(m.prevMileston, m.nextEndDate);
    //    }
    //    return nextMilestons.Min(m => m.nextEndDate);
    //}
    //private IEnumerable<(DO.Task? prevMileston, DateTime nextEndDate)> SetStartDate(DO.Task milestone, DateTime end)
    //{
    //    if (milestone.DeadlineDate != null && milestone.DeadlineDate > end)
    //        end = milestone.DeadlineDate.Value;
    //    else
    //        _dal.Task.Update(milestone with { DeadlineDate = end });

    //    var prevMilestons = _dal.Dependency
    //        .ReadAll(d => d.DependentTask == milestone.Id)
    //        .GroupBy(d => d.DependsOnTask)
    //        .Select(g =>
    //        {
    //            var prevMileston = _dal.Task.Read(g.Key);
    //            var prevTasks = g.Select(d => _dal.Task.Read(d.DependsOnTask));
    //            var endDates = prevTasks
    //            .Select(task =>
    //                    {
    //                        var endDate = task!.DeadlineDate == null || task.DeadlineDate > end ? end : task!.DeadlineDate;
    //                        var nextEndDate = endDate?.Subtract(task.Duration!.Value);
    //                        _dal.Task.Update(task! with { DeadlineDate = endDate });
    //                        return nextEndDate;
    //                    });
    //            return (
    //                prevMileston,
    //                nextEndDate: endDates.Min()!.Value
    //            ); ;
    //        });
    //    return prevMilestons;
    //}





    public Milestone GetMilestone(int id)
    {
        throw new NotImplementedException();
    }

    public Milestone UpdateMilestone(int id)
    {
        throw new NotImplementedException();
    }
}
