using BlApi;

namespace BlImplementation;
internal class Bl : IBl
{
    public void InitializeDB() => DalTest.Initialization.Do();

    public void ResetDB() => DalTest.Initialization.Reset();

    public IEngineer Engineer => new EngineerImplementation();

    public ITask Task => new TaskImplementation();

    public IMilestone Milestone => new MilestoneImplementation();

    #region Project Date
    private static DateOnly _currentDate = DateOnly.FromDateTime(DateTime.Now);

    public DateOnly CurrentDate { get => _currentDate; private set => _currentDate = value; }

    public void AddDay() => CurrentDate = CurrentDate.AddDays(1);

    public DateOnly ResetDate() => CurrentDate = DateOnly.FromDateTime(DateTime.Now);

    #endregion

}
