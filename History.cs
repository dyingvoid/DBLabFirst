using System.Globalization;
using System.Net;
using System.Runtime.InteropServices;

namespace DBFirstLab;

public class History
{
    public List<Customer?> Persons { get; set; }
    public List<Book?> Books { get; set; }
    public List<DateTime?> Times { get; set; }
    public List<EnumOperation?> Operations { get; set; }

    // List = {Persons.Name, Persons.Id, Book.Name, DateTime, Enum.EnumOperation}
    public History(CsvTable table)
    {
        Persons = new List<Customer?>();
        Books = new List<Book?>();
        Times = new List<DateTime?>();
        Operations = new List<EnumOperation?>();

        foreach (var stroke in table)
        {
            try
            {
                AddObjects(stroke);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Source} {ex.Message}, could not create instance of History class.");
            }
        }
    }

    public void AddObjects(List<string?> properties)
    {
        Persons.Add(new Customer(new List<string?>() { properties[0], properties[1]}));
        Books.Add(new Book(new List<string?>() { properties[2] }));
        AddTime(properties[3]);
        AddOperation(properties[4]);
    }

    private void AddOperation(string? operation)
    {
        operation = Controller.SetStringNullIfValueEmptyOrWhiteSpace(operation);
        if (operation == null)
        {
            Operations.Add(null);
        }
        else
        {
            Operations.Add(Enum.Parse<EnumOperation>(operation));
        }
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
            Times.Add(DateTime.Parse(time));
        }
    }
}