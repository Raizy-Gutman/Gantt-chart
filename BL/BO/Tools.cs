
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

    public static List<TD> ConvertList<TS,TE,TD>(this IEnumerable<TS> list) where TE: new() where TD : new()
    {
        return list.Select(d => d.Convert<TS, TE>()).Select(e => e.Convert<TE, TD>()).ToList();
    }

    public static TD? Convert<TS, TE, TD>(this TS ts) where TE : new() where TD : new()
    {
        return ts.Convert<TS, TE>().Convert<TE, TD>();
    }
}