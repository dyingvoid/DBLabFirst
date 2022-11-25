using System.Text.Json.Serialization;

namespace DBFirstLab;

public class DbStructure
{
    [JsonPropertyName("BookTable")]
    public Book Book { get; set; }
    [JsonPropertyName("ConsumerTable")]
    public Customer Customer { get; set; }
    [JsonPropertyName("HistoryTable")]
    public History History { get; set; }
}