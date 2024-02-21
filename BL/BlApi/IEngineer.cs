using BO;

namespace BlApi;

public interface IEngineer
{
    public IEnumerable<EngineerInList> ReadAllEngineers(/*Func<Engineer, bool>? filter*/);
    public BO.Engineer GetEngineer(int? id);
    public int CreateEngineer(BO.Engineer engineer);
    public void DeleteEngineer(int? id);
    public void UpdateEngineer(BO.Engineer engineer);
}
