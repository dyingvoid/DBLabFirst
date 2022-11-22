namespace DBFirstLab;

public class Customer
{
    private string? _name;
    private long? _id;
    public Customer(List<string?> properties)
    {
        Extensions.PrepareListOfProperties<Customer>(ref properties);

        Name = properties[0];
        Id = SetIntegerValue<long>(properties[1]);
    }

    public string? Name
    {
        get => _name;
        set => _name = Extensions.SetStringNullIfValueEmptyOrWhiteSpace(value); 
    }

    public long? Id { get; private set; }

    public T? SetIntegerValue<T>(string? value)
    where T : struct, IParsable<T>
    {
        T? property = Extensions.SetNullIfValueEmptyOrWhiteSpace<T>(value);
        return property;
    }
}