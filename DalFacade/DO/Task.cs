namespace DO;

/// <summary>
/// The task entity contains all the information needed to perform the task.
/// </summary>
/// <param name="id">A unique ID number for the task</param>
/// <param name="description">Description of the task</param>
/// <param name="alias">Another alias for the task</param>
/// <param name="mileStone">Milestones in the mission</param>
/// <param name="CreatedAtDate">Task creation date</param>
/// <param name="start">Date of starting work on the task</param>
/// <param name="schedual">Planned date for the start of work</param>
/// <param name="forecast">The amount of time required to perform the task</param>
/// <param name="deadline">Last possible end date</param>
/// <param name="complete">Actual end date</param>
/// <param name="deliverables">product</param>
/// <param name="remarks">Notes</param>
/// <param name="engineerid">The ID of the engineer assigned to the task</param>
/// <param name="ComplexityLevel">The difficulty level of the task</param>

public record Task
{
    public int Id { get; set; }
    public string? Description { get; init; } = null;
    public string? Alias { get; init; } = null;
    public bool? IsMilestone { get; init; } = null;
    public DateTime? CreatedAtDate => DateTime.Now;
    public DateTime? StartDate { get; init; } = null;
    public DateTime? SchedualDate { get; init; } = null;
    public DateTime? ForecastDate { get; init; } = null;
    public DateTime? DeadlineDate { get; init; } = null;
    public DateTime? CompleteDate { get; init; } = null;
    public string? Deliverables { get; init; }
    public string? Remarks { get; init; }
    public int EngineerId { get; init; }
    //public EngineerExperienc ComplexityLevel

    public Task() { this.Id = 0;
        this.Deliverables = ""; 
        this.Remarks = ""; 
        this.EngineerId = 0; 
    }
    public Task(int id, string description, string alias, bool mileStone, DateTime start, DateTime schedual,
        DateTime forecast, DateTime deadline, DateTime complete, string deliverables, string remarks, int engineerid)
    {
        Id = id;
        Description = description;
        Alias = alias;
        IsMilestone = mileStone;
        StartDate = start;
        SchedualDate = schedual;
        DeadlineDate = deadline;
        CompleteDate = complete;
        Deliverables = deliverables;
        Remarks = remarks;
        EngineerId = engineerid;
    }
}
