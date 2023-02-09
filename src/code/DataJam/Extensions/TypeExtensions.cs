using System.Collections;

namespace DataJam.Extensions;

public static class TypeExtensions
{
    public static Type ToSingleType(this Type type)
    {
        return type.IsGenericType && type.IsEnumerable()
            ? type.GetGenericArguments().Single()
            : type;
    }

    public static bool IsEnumerable(this Type type)
    {
        if (type == typeof(string))
        {
            return false;
        }
        return type == typeof(IEnumerable) || type.GetInterfaces().Contains(typeof(IEnumerable));
    }
}