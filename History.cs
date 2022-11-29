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

    // List = {Persons.Name, Persons.Id, Book.Name, DateTime}
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

    public string FindBookInformation(Book book)
    {
        var bookIndexes = new List<int>();

        for (int i = 0; i < Books.Count; ++i)
        {
            if (Books[i].Name == book.Name)
                bookIndexes.Add(i);
        }

        foreach (var index in bookIndexes)
        {
            if (Operations[index] == EnumOperation.Borrow)
                return $"Borrowed by {Persons[index].Name} at {Times[index]}.";
        }
        
        return "";
    }

    public static bool CheckCustomerExistence(string customerName, int customerId, List<Customer> customers)
    {
        if (customers == null)
            throw new NullReferenceException();
        
        var customerSpecifiedId = FindCustomerUsingId(customers, customerId);
        var customersSpecifiedName = FindCustomersUsingName(customers, customerName);

        if (customerSpecifiedId == null || customersSpecifiedName == null)
        {
            return false;
        }
        
        return customersSpecifiedName.Exists(customer => customer.Id == customerSpecifiedId.Id);
    }

    public static List<Customer>? FindCustomersUsingName(List<Customer> customers, string name)
    {
        if (customers == null)
            throw new ArgumentNullException();

        var foundCustomersList = customers.FindAll(customer => customer.Name == name);

        return foundCustomersList;
    }

    public static Customer? FindCustomerUsingId(List<Customer> customers, int id)
    {
        if (customers == null) 
            throw new ArgumentNullException();

        var foundCustomer = customers.Find(customer => customer.Id == id);

        return foundCustomer;
    }
}