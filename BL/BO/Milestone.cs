using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;

public class Milestone
{
    public int Id { get; init; }
    public required string Description { get; set; }
    public required string Alias { get; set; }
    public Status Status { get; set; }
    public DateTime CreatedAtDate { get; set; }
    public DateTime? SchedualDate { get; set; }
    public DateTime? DeadlineDate { get; set; }
    public DateTime? CompleteDate { get; set; }
    public Double? CompletionPercentage { get; set; }
    public string? Remarks { get; set; }
    public required List<TaskInList> Dependencies {  get; set; }

}

