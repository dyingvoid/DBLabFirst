using System.Text.Json.Serialization;

namespace DBFirstLab;

public class DbStructure
{
    public DbStructure(string jsonString)
    {
        JsonString = jsonString;
    }
    public String JsonString { get; set; }

    public List<string> ToFormat()
    {
        return JsonString.Split(',').ToList();
    }
}