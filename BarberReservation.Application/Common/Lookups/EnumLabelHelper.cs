using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

internal static class EnumLabelHelper
{
    public static string GetLabelCs<TEnum>(TEnum value) where TEnum : struct, Enum
    {
        var name = value.ToString();
        var member = typeof(TEnum).GetMember(name).FirstOrDefault();
        if (member is null)
            return name;

        var display = member.GetCustomAttribute<DisplayAttribute>();
        if (display?.Name is { Length: > 0 })
            return display.Name;

        var description = member.GetCustomAttribute<DescriptionAttribute>();
        if (description?.Description is { Length: > 0 }) 
            return description.Description;

        return name;
    }
}
