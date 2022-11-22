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
        return value is "" or " ";
    }

    public static T? SetNullIfValueEmptyOrWhiteSpace<T>(string? value)
    where T : struct, IParsable<T>
    {
        if (T.TryParse(value, CultureInfo.InvariantCulture, out var returnVariable))
        {
            return returnVariable;
        }

        return null;
    }

    public static string? SetStringNullIfValueEmptyOrWhiteSpace( string? value)
    {
        if (value.IsEmptyOrWhiteSpace() || value == null)
        {
            return null;
        }
        
        return value;
    }

    public static void EnlargeListWithNulls(this List<string?> list, int onSize)
    {
        for (int i = 0; i < onSize; ++i)
        {
            list.Add(null);
        }
    }

    public static int NumberOfFieldsAndProps<TObj>()
    {
        var objType = typeof(TObj);
        int numberFields = objType.GetFields().Length;
        int numberProperties = objType.GetProperties().Length;
        return numberFields + numberProperties;
    }
    
    public static void CheckSizeEnlarge<TObj>(this List<string?> properties)
    {
        int numberFieldsProps = Extensions.NumberOfFieldsAndProps<TObj>();
        if (properties.Count > numberFieldsProps)
        {
            throw new Exception($"List.Count({properties.Count}) must be less or equal" +
                                $"to {typeof(TObj).FullName} number of fields and properties({numberFieldsProps})");
        }

        properties.EnlargeListWithNulls(numberFieldsProps - properties.Count);
    }

    public static void PrepareListOfProperties<TObj>(ref List<string?> properties)
    {
        try
        {
            properties.CheckSizeEnlarge<TObj>();
        }
        catch (Exception ex)
        {
            var objType = typeof(TObj);
            var requiredListSize = NumberOfFieldsAndProps<TObj>();
            
            Console.WriteLine($"{ex.Source}: {ex.Message}.\n" +
                              $"{objType.FullName} List of properties will be List[{requiredListSize}] of nulls.");
            
            properties = new List<string?>(new string?[requiredListSize]);
        }
    }
}