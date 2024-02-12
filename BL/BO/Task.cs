using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Task
    {
        public int Id { get; init; }
        public string? Description { get; set; }
        public string? Alias { get; set; }
        public bool IsMilestone { get; set; }
        public DateTime? CreatedAtDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? SchedualDate { get; set; }
        public TimeSpan? Duration { get; set; }
        public DateTime? DeadlineDate { get; set; }
        public DateTime? CompleteDate { get; set; }
        public string? Deliverables { get; set; }
        public string? Remarks { get; set; }
        public int EngineerId { get; set; }
        public EngineerExperience ComplexityLevel { get; set; }
    }
}
