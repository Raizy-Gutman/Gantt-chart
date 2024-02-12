using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi;

public interface ITask
{
    public IEnumerable<BO.Task> ReadAllTasks(Func<BO.Task, bool>? filter);
    public BO.Task GetTask(int id);
    public void CreateTask(BO.Task engineer);
    public void DeleteTask(int id);
    public void UpdateTask(BO.Task engineer);
}
