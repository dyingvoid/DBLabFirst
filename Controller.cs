using System.Globalization;
using System.Text.Json;

namespace DBFirstLab;

public class Controller
{
    public static int NumberOfFieldsAndProps<TObj>()
    {
        var objType = typeof(TObj);
        int numberFields = objType.GetFields().Length;
        int numberProperties = objType.GetProperties().Length;
        return numberFields + numberProperties;
    }

    public static Dictionary<string, Dictionary<string, string>>? ParseJson(FileInfo file)
    {
        using StreamReader stream = new StreamReader(file.FullName);
        string json = stream.ReadToEnd();
        var jsonDict = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(json);

        return jsonDict;
    }
}