namespace BO;
public class TaskInList
{
    public int Id { get; init; }
    public required string Description { get; set; }
    public string Alias { get; set; }
    public Status Status { get; set; }
}

