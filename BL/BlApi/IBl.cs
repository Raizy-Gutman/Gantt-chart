namespace BlApi;

public interface IBl
{

    public void ResetDB();
    
    public void InitializeDB();
    
    public IEngineer Engineer { get; }
    
    public ITask Task { get; }
    
    public IMilestone Milestone { get; }
    
    #region Project Date
    public DateOnly CurrentDate { get; }

    public void AddDay();

    public DateOnly ResetDate();
    #endregion
}
