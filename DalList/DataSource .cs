namespace Dal;
internal static class DataSource
{
    internal static class Config
    {
        internal const int firstTaskId = 1;
        private static int nextTaskId = firstTaskId;
        internal static int NextTaskId { get => nextTaskId++; }

        internal const int firstDependencyId = 1;
        private static int nextDependencyId = firstDependencyId;
        internal static int NextDependencyId { get => nextDependencyId++; }

        internal static DateTime? StartDate { get; set; }
        internal static DateTime? EndDate { get; set; }

    }
    internal static List<DO.Task?> Tasks = new();
    internal static List<DO.Dependency?> Dependencies = new();
    internal static List<DO.Engineer?> Engineers = new();
}

