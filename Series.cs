namespace DBFirstLab;

public class Series<T> where T : struct
{
    public String Name { get; set; }
    public List<T> Data { get; set; }
    public Type Type => typeof(T);
}