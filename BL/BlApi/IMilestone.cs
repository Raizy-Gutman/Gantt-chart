﻿namespace BlApi;
public interface IMilestone
{
    public void CreateSchedule(DateTime start, DateTime end);
    public BO.Milestone GetMilestone(int id);
    public BO.Milestone UpdateMilestone(int id, string alias, string description, string comments);
}
