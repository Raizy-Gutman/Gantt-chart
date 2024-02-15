
using System.ComponentModel;
using System.Reflection;

namespace BO;
public static class Tools
{



    public static ProjectStatus? GetProjectStatus(this DalApi.IDal d) => (ProjectStatus?)d.ProjectStatus;
    public static void SetProjectStatus(this DalApi.IDal d, ProjectStatus s) => d.ProjectStatus = (DO.ProjectStatus)s;
    public static TD Convert<TS, TD>(this TS source) where TD : new()
    {
        TD destination = new();
        var srcPropsWithValues = typeof(TS)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .ToDictionary(x => x.Name, y => y.GetValue(source));

        var dstProps = typeof(TD)
       .GetProperties(BindingFlags.Public | BindingFlags.Instance)
       .ToDictionary(key => key, value => value.GetCustomAttribute<DefaultValueAttribute>()?.Value
                                       ?? (value.PropertyType.IsValueType
                                       ? Activator.CreateInstance(value.PropertyType, null)
                                       : null));
        foreach (var prop in dstProps)
        {
            var destProperty = prop.Key;

            if (srcPropsWithValues.ContainsKey(destProperty.Name))
            {
                var defaultValue = prop.Value;
                var sourceValue = srcPropsWithValues[destProperty.Name];

                destProperty.SetValue(destination, sourceValue ?? defaultValue);
            }
        }
        return destination;
    }

    public static BO.Engineer ConvertToBEngineer(this DalApi.IDal d, DO.Engineer source) 
    {
        Engineer dest = source.Convert<DO.Engineer, BO.Engineer>();
        DO.Task? t = d.Task.Read(t => t?.EngineerId == dest.Id);
        dest.Task = t == null ? null : t.Convert<DO.Task, BO.TaskInEngineer>();
        return dest;
    }
}