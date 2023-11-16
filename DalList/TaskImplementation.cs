

namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

public class TaskImplementation : ITask
{
    public int Create(Task _task)
    {
        //throw new NotImplementedException();
        int newId = DataSource.Config.NextTaskId;
        Task task = _task with { Id = newId };
        DataSource.Tasks.Add(task);
        return newId;
    }

    public void Delete(int id)
    {
        //throw new NotImplementedException();
        Task? t = DataSource.Tasks.Find(t => t.Id == id);
        if (t != null)
        {
            DataSource.Tasks.Remove(t);
            //אם צריך לעדכן לא פעיל- נוסיף שדה מתאים...
        }
        else
        {
            /*throw new NotImplementedException();*/
        }
    }

    public Task? Read(int id)
    {
        return DataSource.Tasks.Find(t => t.Id == id);
    }

    public List<Task> ReadAll()
    {
        return new List<Task>(DataSource.Tasks);
    }

    public void Update(Task _task)
    {
        //throw new NotImplementedException();
        Task? t = DataSource.Tasks.Find(t => t.Id == _task.Id);
        if (t != null)
        {
            DataSource.Tasks.Remove(t);
            DataSource.Tasks.Add(_task);
        }
        else
        {
            //throw new InvalidOperationException();
        }
    }
}
