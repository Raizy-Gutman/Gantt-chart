namespace BlApi;
public interface IMilestone
{
    public void CreateMilesones();
    public BO.Milestone GetMilestone(int id);
    public BO.Milestone UpdateMilestone(int id);
}
