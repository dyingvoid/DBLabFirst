using System.Globalization;
using System.Text.Json;

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

    public static string? SetStringNullIfValueEmptyOrWhiteSpace(string? value)
    {
        if (value.IsEmptyOrWhiteSpace() || value == null)
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

    public static void ParseJsonToTypes(FileInfo file)
    {
        var outputList = new List<object>();
        using (StreamReader stream = new StreamReader(file.FullName))
        {
            string json = stream.ReadToEnd();
            var list = JsonSerializer.Deserialize<object>(json);
        }
    }
}