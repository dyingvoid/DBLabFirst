using System.Globalization;
using System.Net;
using System.Runtime.InteropServices;

namespace DBFirstLab;

public class History
{
    public List<Customer?> Persons { get; set; }
    public List<Book?> Books { get; set; }
    public List<DateTime?> Times { get; set; }

    // List = {Persons.Name, Persons.Id, Book.Name, DateTime}
    public History(CsvTable table)
    {
        Persons = new List<Customer?>();
        Books = new List<Book?>();
        Times = new List<DateTime?>();

        foreach (var stroke in table)
        {
            AddObjects(stroke);
        }
    }

    public void AddObjects(List<string?> properties)
    {
        Persons.Add(new Customer(new List<string?>() { properties[0], properties[1] }));
        Books.Add(new Book(new List<string?>() { properties[2] }));
        AddTime(properties[3]);
    }

    private void AddTime(string? time)
    {
        time = Extensions.SetStringNullIfValueEmptyOrWhiteSpace(time);
        if (time == null)
        {
            Times.Add(null);
        }
        else
        {
            Times.Add(DateTime.ParseExact(time, "yyyy:MM:dd", CultureInfo.InvariantCulture));
        }
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