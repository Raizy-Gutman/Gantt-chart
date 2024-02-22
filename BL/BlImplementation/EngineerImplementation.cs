using BlApi;
using BO;
using System.Net.Mail;

namespace BlImplementation;
internal class EngineerImplementation : IEngineer
{
    private readonly DalApi.IDal _dal = DalApi.Factory.Get;
    private static void TestEngineer(Engineer e)
    {
        if (e == null)
            throw new BlNullException("Engineer");
        if (e.Id < 0)
            throw new BlInvalidException("Id");
        if (string.IsNullOrEmpty(e.Name))
            throw new BlInvalidException("Name");
        if (e.Cost < 0)
            throw new BlInvalidException("Cost");
        if (!MailAddress.TryCreate(e.Email, out _))
            throw new BlInvalidException("Email");
    }
    public int CreateEngineer(BO.Engineer engineer)
    {
        TestEngineer(engineer);
        try
        {
           return  _dal.Engineer.Create(engineer.Convert<BO.Engineer, DO.Engineer>());
        }
        catch (DO.DalAlreadyExistsException e)
        {
            throw new BlAlreadyExistsException(e);
        }
    }
    public void DeleteEngineer(int id)
    {
        if (_dal.GetProjectStatus() == ProjectStatus.Execution)
        {
            if (_dal.Task.Read(t => t?.EngineerId == id) != null)
                throw new BlIllegalException("engineer", "deletion");
        }
        try
        {
            _dal.Engineer.Delete(id);
        }
        catch (DO.DalDoesNotExistException e)
        {
            throw new BlDoesNotExistException(e);
        }
    }

    public Engineer GetEngineer(int id)
    {
        DO.Engineer de = _dal.Engineer.Read(id) ?? throw new BlDoesNotExistException($"Engineer {id}");
        TaskInEngineer? t = _dal.Task.Read(t => t.EngineerId == de.Id)?.Convert<DO.Task, BO.Task, BO.TaskInEngineer>() ?? null;
        BO.Engineer be = de.Convert<DO.Engineer, BO.Engineer>();
        be.Task = t;
        return be;
    }

    //Func<Engineer, bool>? filter
    public IEnumerable<EngineerInList> ReadAllEngineers()
    {
        return from DO.Engineer e in _dal.Engineer.ReadAll()
               select e.Convert<DO.Engineer, BO.EngineerInList>();
    }

    public void UpdateEngineer(Engineer engineer)
    {
        TestEngineer(engineer);
        Engineer beforeUpdates = GetEngineer(engineer.Id);
        if (beforeUpdates.Level > engineer.Level) throw new BlIllegalException("level", "update");
        //אהמממ מה
        if (beforeUpdates.Task != engineer.Task && engineer.Task != null)
        {
            DO.Task task = _dal.Task.Read(engineer.Task.Id) ?? throw new BlIllegalException("level", "update");
            DO.Task updateTask = task with { EngineerId = engineer.Id };
            _dal.Task.Update(updateTask);
        }
        _dal.Engineer.Update(engineer.Convert<BO.Engineer, DO.Engineer>());

    }
}
