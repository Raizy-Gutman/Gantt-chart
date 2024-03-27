using BlApi;

namespace BlImplementation;
internal class Bl : IBl
{
    public void InitializeDB() => DalTest.Initialization.Do();

    public void ResetDB() => DalTest.Initialization.Reset();
     
    public IEngineer Engineer => new EngineerImplementation();
    public ITask Task => new TaskImplementation();
    public IMilestone Milestone => new MilestoneImplementation();
}


