namespace BlApi;

public interface ITask
{
    public IEnumerable<BO.TaskInList> ReadAllTasks(/*Func<BO.Task, bool>? filter*/);
    public BO.Task GetTask(int id);
    public void CreateTask(BO.Task engineer);
    public void DeleteTask(int id);
    public void UpdateTask(BO.Task engineer);
}
