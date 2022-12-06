namespace DBFirstLab.Core;

public class DbException : Exception
{
    public DbException()
    {
        Console.WriteLine("something");
    }

    public DbException(string message) : base($"Could not set columns {message}")
    {
        
    }
}