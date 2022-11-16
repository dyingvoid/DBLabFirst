namespace DBFirstLab;

public class Customer
{
    public Customer(List<string?> properties)
    {
        Name.SetStringNullIfValueEmptyOrWhiteSpace(properties[0]);
        Id = SetIntegerValue<long>(properties[1]);
    }
    
    public string? Name { get; set; }
    
    public long? Id { get; set; }

    public T? SetIntegerValue<T>(string? value)
    where T : struct, IParsable<T>
    {
        T? property = Extensions.SetNullIfValueEmptyOrWhiteSpace<T>(value);
        return property;
    }
}