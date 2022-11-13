namespace DBFirstLab;

public class Book
{
    private string? _name;
    private string? _author;
    private int? _yearPublished;

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

    public int? YearPublished
    {
        get => _yearPublished;
        set => _yearPublished = value;
    }
    public int? BookCase { get; set; }
    public uint? BookShelf { get; set; }

    public Book(List<string?> properties)
    {
        SetIntegerValue(ref _yearPublished, properties[0]);
    }

    public void SetIntegerValue<T>(ref T? property, string? value)
    where T : struct, IParsable<T>
    {
        property = Extensions.SetNullIfValueEmptyOrWhiteSpace<T>(value);
    }
}