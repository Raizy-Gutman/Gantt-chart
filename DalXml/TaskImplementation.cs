namespace Dal;
using DalApi;
using DO;

internal class TaskImplementation : ITask
{
    const string taskRoot = "tasks"; //XML Serializer

    public int Create(DO.Task task)
    {
        var tasksList = XMLTools.LoadListFromXMLSerializer<DO.Task>(taskRoot);
        int id = Config.NextTaskId;
        tasksList.Add(task with { Id = id });
        XMLTools.SaveListToXMLSerializer(tasksList, taskRoot);
        return id;
    }

    public void Reset()
    {
        List<Task> taskList = new();
        XMLTools.SaveListToXMLSerializer(taskList!, taskRoot);
    }

    public void Delete(int id)
    {
        var tasksList = XMLTools.LoadListFromXMLSerializer<DO.Task>(taskRoot);
        if (tasksList.RemoveAll(t => t?.Id == id) == 0)
            throw new DalDoesNotExistException($"Can't delete, task with ID: {id} does not exist!!");
        XMLTools.SaveListToXMLSerializer(tasksList, taskRoot);
    }

    public DO.Task? Read(int id) =>
    
        XMLTools.LoadListFromXMLSerializer<DO.Task>(taskRoot).FirstOrDefault(t => t?.Id == id) ?? null;       

    public DO.Task? Read(Func<DO.Task, bool> filter) =>

        XMLTools.LoadListFromXMLSerializer<DO.Task>(taskRoot).FirstOrDefault(filter) ?? null;
        
    public IEnumerable<DO.Task?> ReadAll(Func<DO.Task?, bool>? filter = null)
    {
        var tasksList = XMLTools.LoadListFromXMLSerializer<DO.Task>(taskRoot);
        return filter == null? tasksList.Select(t=>t):tasksList.Where(filter);
    }

    public void Update(DO.Task task)
    {
        Delete(task.Id);
        Create(task);
    }
}
