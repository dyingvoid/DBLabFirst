using System.Globalization;

namespace DBFirstLab;

public class History
{
    // List = {Persons.Name, Persons.Id, Book.Name, DateTime, OperationType}
    public History(CsvTable table)
    {
        Persons = new List<Customer?>();
        Books = new List<Book?>();
        Times = new List<DateTime?>();
        Types = new List<OperationType?>();

        foreach (var stroke in table)
        {
            AddObjects(stroke);
        }
    }
    
    public List<Customer?> Persons { get; set; }
    public List<Book?> Books { get; set; }
    public List<DateTime?> Times { get; set; }
    public List<OperationType?> Types;

    public void AddObjects(List<string?> properties)
    {
        Persons.Add(new Customer(new List<string?>() { properties[0], properties[1] }));
        Books.Add(new Book(new List<string?>() { properties[2] }));
        AddTime(properties[3]);
        AddOperationType(properties[4]);
    }

    private void AddTime(string? time)
    {
        time = Controller.SetStringNullIfValueEmptyOrWhiteSpace(time);
        if (time == null)
        {
            Times.Add(null);
        }
        else
        {
            Times.Add(DateTime.ParseExact(time, "yyyy:MM:dd", CultureInfo.InvariantCulture));
        }
    }

    private void AddOperationType(string? value)
    {
        if (value == null)
        {
            Types.Add(null);
        }
        else
        {
            OperationType.TryParse(value, out OperationType type);
            Types.Add(type);
        }
    }
}