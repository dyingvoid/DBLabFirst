namespace DBFirstLab;

public class Operation
{
    public Operation(List<string?> properties, List<Book?> books, List<Customer?> customers)
    {
        
    }
    
    public Customer? Person { get; set; }
    
    public Book? OperationBook { get; set; }
    
    public DateTime? OperationTime { get; set; }

    public Customer? FindCustomer(string? id, List<Customer?> customers)
    {
        if (id == null || id.IsEmptyOrWhiteSpace())
            throw new NullReferenceException("id is ambiguous. Must be long integer value.");

        var customer = customers.Find(person => person.Id == long.Parse(id));

        return customer;
    }
}