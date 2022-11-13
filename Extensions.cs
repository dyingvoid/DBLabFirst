using System.Data.SqlTypes;
using System.Globalization;

namespace DBFirstLab;

public static class Extensions
{
    public static TNewCollection PureForEach<TCollection, TValue, TNewCollection, TNewValue>
        (this TCollection collection, Func<TValue, TNewValue> func)
    where TCollection : ICollection<TValue>, new()
    where TNewCollection : ICollection<TNewValue>, new()
    {
        if (collection == null)
            throw new NullReferenceException();
        if (func == null) 
            throw new NullReferenceException();

        var newCollection = new TNewCollection();
        foreach (var value in collection)
        {
            var newValue = func(value);
            newCollection.Add(newValue);
        }

        return newCollection;
    }
    
    public static bool IsEmptyOrWhiteSpace(this string? value)
    {
        if (value == "" || value == " ")
            return true;
        
        return false;
    }

    public static T? SetNullIfValueEmptyOrWhiteSpace<T>(string? value)
    where T : struct, IParsable<T>
    {
        if (value.IsEmptyOrWhiteSpace() || value == null)
        {
            return null;
        }

        return T.Parse(value, CultureInfo.InvariantCulture);
    }

    public static string? SetStringNullIfValueEmptyOrWhiteSpace(this string? variable, string? value)
    {
        if (value.IsEmptyOrWhiteSpace() || value == null)
            return null;

        return value;
    }
}