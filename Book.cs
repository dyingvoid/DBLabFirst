namespace DBFirstLab;

public class Book
{
    private string? _name;
    private string? _author;
    
    public string? Name
    {
        get
        {
            return _name;
        }
        set
        {
            _name.SetStringNullIfValueEmptyOrWhiteSpace(value);
        }
    }
    
    public string? Author
    {
        get
        {
            return _author;
        }
        set
        {
            _author.SetStringNullIfValueEmptyOrWhiteSpace(value);
        }
    }

    public int? YearPublished { get; set; }
    public uint? BookCase { get; set; }
    public uint? BookShelf { get; set; }

    public Book(List<string?> properties)
    {
        _name = properties[0];
        _author = properties[1];
        YearPublished = SetIntegerValue<int>(properties[2]);
        BookCase = SetIntegerValue<uint>(properties[3]);
        BookShelf = SetIntegerValue<uint>(properties[2]);
    }

    public T? SetIntegerValue<T>(string? value)
    where T : struct, IParsable<T>
    {
        T? property = Extensions.SetNullIfValueEmptyOrWhiteSpace<T>(value);
        return property;
    }
}