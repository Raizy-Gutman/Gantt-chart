
namespace BO;
public static class Tools
{
    public static ProjectStatus GetProjectStatus(this DalApi.IDal d) => d.StartDate == null ? ProjectStatus.Planning : ProjectStatus.Execution;
}
