namespace Dal;
internal static class Config
{
    static readonly string s_data_config_xml = "data-config";
    internal static int NextTaskId { get => XMLTools.GetAndIncreaseNextId(s_data_config_xml, "NextTaskId"); }
    internal static int NextDependencyId { get => XMLTools.GetAndIncreaseNextId(s_data_config_xml, "NextDependencyId"); }
    internal static DateTime? StartDate { get => XMLTools.Get(s_data_config_xml, "StartDate"); }
    internal static DateTime? EndDate { get => XMLTools.Get(s_data_config_xml, "EndDate"); }
}
