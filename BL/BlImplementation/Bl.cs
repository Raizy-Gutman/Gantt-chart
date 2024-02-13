using BlApi;

namespace BlImplementation;
internal class Bl : IBl
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public IEngineer Engineer => new EngineerImplementation();
    public ITask Task => new TaskImplementation();
    public DateTime? StartDate { get => _dal.StartDate; set => _dal.StartDate = value; }
    public DateTime? EndDate { get => _dal.EndDate; set => _dal.EndDate = value; }
}


