﻿using BlApi;
using BO;

namespace BlImplementation;

internal class TaskImplementation : ITask
{
    private readonly DalApi.IDal _dal = DalApi.Factory.Get;
    private static void TestTask(BO.Task task)
    {
        if (task == null)
            throw new BlNullException("Task");
        if (task.Id < 1)
            throw new BlInvalidException("Id");
        if (string.IsNullOrEmpty(task.Alias))
            throw new BlInvalidException("Alias");
    }

    private static void Calculations(ref BO.Task task)
    {



    }

    public void CreateTask(BO.Task task)
    {
        TestTask(task);
        if (_dal.GetProjectStatus() == ProjectStatus.Execution) throw new BlIllegalException("task", "creation");
        try
        {
            _dal.Task.Create(task.Convert<BO.Task, DO.Task>());
        }
        catch (DO.DalAlreadyExistsException e)
        {
            throw new BlAlreadyExistsException(e);
        }
    }

    public void DeleteTask(int id)
    {
        if (_dal.GetProjectStatus() == ProjectStatus.Execution) throw new BlIllegalException("task", "deletion");
        if (_dal.Dependency.Read(d => d.DependsOnTask == id) != null) throw new BlIllegalException("task", "deletion");
        try { _dal.Task.Delete(id); }
        catch (DO.DalDoesNotExistException e) { throw new BlDoesNotExistException(e); }
    }

    public BO.Task GetTask(int id)
    {
        BO.Task task = _dal.Task.Read(id)?.Convert<DO.Task, BO.Task>() ?? throw new BlDoesNotExistException($"Task {id}");
        Calculations(ref task);
        return task;
    }

    public IEnumerable<BO.TaskInList> ReadAllTasks(/*Func<BO.Task, bool>? filter*/)
    {
        return from DO.Task t in _dal.Task.ReadAll()
               select new BO.TaskInList { Id = t.Id, Description = t.Description, Alias = t.Alias, Status = 0 /*איך לחשב סטטוס*/};
    }
    public void UpdateTask(BO.Task task)
    {
        TestTask(task);
        DO.Task beforeUpdates = _dal.Task.Read(task.Id) ?? throw new BlDoesNotExistException("Task");
        if (_dal.GetProjectStatus() == ProjectStatus.Execution)
            if (beforeUpdates.SchedualDate != task.SchedualDate ||
                beforeUpdates.DeadlineDate != task.DeadlineDate ||
                beforeUpdates.Duration != task.Duration)
                throw new BlIllegalException("task", "update");
        _dal.Task.Update(task.Convert<BO.Task, DO.Task>());
    }
}
