namespace DBFirstLab;

public class Customer
{
    private string? _name;
    public Customer(List<string?> properties)
    {
        Controller.PrepareListOfProperties<Customer>(ref properties);

        Name = properties[0];
        Id = SetIntegerValue<long>(properties[1]);
    }

    public string? Name
    {
        get => _name;
        set => _name = Controller.SetStringNullIfValueEmptyOrWhiteSpace(value); 
    }

    public long? Id { get; set; }

    public T? SetIntegerValue<T>(string? value)
    where T : struct, IParsable<T>
    {
        T? property = Controller.SetNullIfValueEmptyOrWhiteSpace<T>(value);
        return property;
    }
}