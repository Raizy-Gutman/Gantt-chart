using BlApi;
using BO;
using System.Net.Mail;

namespace BlImplementation;
internal class EngineerImplementation : IEngineer
{
    private readonly DalApi.IDal _dal = DalApi.Factory.Get;
    private void testEngineer(Engineer e)
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
    public void CreateEngineer(Engineer engineer)
    {
        testEngineer(engineer);
        try
        {
            _dal.Engineer.Create(engineer.Convert<BO.Engineer, DO.Engineer>());
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
                throw new BlIllegalDeletionException("Engineer");
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
        return _dal.ConvertToBEngineer(de);
    }

    //Func<Engineer, bool>? filter
    public IEnumerable<EngineerInList> ReadAllEngineers()
    {
        return from DO.Engineer e in _dal.Engineer.ReadAll()
               select e.Convert<DO.Engineer, BO.EngineerInList>();
    }

    public void UpdateEngineer(Engineer engineer)
    {
        testEngineer(engineer);
        Engineer beforeUpdates = GetEngineer(engineer.Id);
        if (beforeUpdates.Level > engineer.Level) throw new BlIllegalUpdateException($"level");
        //אהמממ מה
        if (beforeUpdates.Task != engineer.Task && engineer.Task != null)
        {
            DO.Task task = _dal.Task.Read(engineer.Task.Id) ?? throw new BlIllegalUpdateException($"task");
            DO.Task updateTask = task with { EngineerId = engineer.Id };
            _dal.Task.Update(updateTask);
        }
        _dal.Engineer.Update(engineer.Convert<BO.Engineer, DO.Engineer>());

    }
}
