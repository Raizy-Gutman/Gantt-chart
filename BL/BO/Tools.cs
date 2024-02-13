
using System.ComponentModel;
using System.Reflection;

namespace BO;
public static class Tools
{



    public static ProjectStatus GetProjectStatus(this DalApi.IDal d) => d.StartDate == null ? ProjectStatus.Planning : ProjectStatus.Execution;

    public static TD ConvertTo<TS, TD>(this TS source) where TD : new()
    {
        TD destination = new TD();
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


}
