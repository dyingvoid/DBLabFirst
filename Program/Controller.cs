using System.Globalization;

namespace DBFirstLab;

public class Controller
{
    public static T? SetNullIfValueEmptyOrWhiteSpace<T>(string? value)
        where T : struct, IParsable<T>
    {
        if (T.TryParse(value, CultureInfo.InvariantCulture, out var returnVariable))
        {
            return returnVariable;
        }

        return null;
    }

    public static string? MakeNullIfEmptyOrSpace(string? value)
    {
        return MakeNullIfPredicate(value, str => str.IsEmptyWhiteSpaceNull());
    }

    public static string? MakeNullIfPredicate(string? value, Func<string?, bool> predicate)
    {
        if (predicate(value))
        {
            return null;
        }

        return value;
    }
    
    public static int NumberOfFieldsAndProps<TObj>()
    {
        var objType = typeof(TObj);
        int numberFields = objType.GetFields().Length;
        int numberProperties = objType.GetProperties().Length;
        return numberFields + numberProperties;
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