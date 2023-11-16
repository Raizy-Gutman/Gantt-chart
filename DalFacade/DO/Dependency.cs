namespace DO;

/// <summary>
/// Dependency entity keeps for each task the task it relies on.
/// </summary>
/// <param name="id">Identification number for the dependency </param>
/// <param name="dependentTask"> The task number </param>
/// <param name="dependsOnTask"> The number of the task it depends on </param>
/// 

public record Dependency
{
    public int Id;
    public int DependentTask;
    public int DependsOnTask;
    Dependency() : this(0, 0, 0) { }

    Dependency(int id, int dependentTask, int dependsOnTask)
    {
        Id = id;
        DependentTask = dependentTask;
        DependsOnTask = dependsOnTask;
    }
}
