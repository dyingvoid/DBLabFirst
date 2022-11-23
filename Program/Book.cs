namespace DBFirstLab;

public class Book
{
    private string? _name;
    private string? _author;

    public Book(List<string?> properties)
    {
        Controller.PrepareListOfProperties<Book>(ref properties);

        Name = properties[0];
        Author = properties[1];
        YearPublished = SetIntegerValue<int>(properties[2]);
        BookCase = SetIntegerValue<uint>(properties[3]);
        BookShelf = SetIntegerValue<uint>(properties[4]);
    }

    public string? Name
    {
        get => _name;
        set => _name = Controller.MakeNullIfEmptyOrSpace(value); 
    }
    
    public string? Author
    {
        get => _author;
        set => _author = Controller.MakeNullIfEmptyOrSpace(value);
    }

    public int? YearPublished { get; set; }
    public uint? BookCase { get; set; }
    public uint? BookShelf { get; set; }
    

    public T? SetIntegerValue<T>(string? value)
    where T : struct, IParsable<T>
    {
        T? property = Controller.SetNullIfValueEmptyOrWhiteSpace<T>(value);
        return property;
    }
}