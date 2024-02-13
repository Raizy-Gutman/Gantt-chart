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
            _dal.Engineer.Create(engineer.ConvertTo<BO.Engineer, DO.Engineer>());
        }
        catch(DO.DalAlreadyExistsException e)
        { 
            throw new BlAlreadyExistsException(e);
        }
        
    }

    public void DeleteEngineer(int id)
    {
        throw new NotImplementedException();
    }

    public Engineer GetEngineer(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Engineer> ReadAllEngineers(Func<Engineer, bool>? filter)
    {
        throw new NotImplementedException();
    }

    public void UpdateEngineer(Engineer engineer)
    {
        throw new NotImplementedException();
    }
}
