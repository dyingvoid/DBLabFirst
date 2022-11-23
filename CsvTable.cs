using System.Collections;

namespace DBFirstLab;

public class CsvTable : IEnumerable<List<string?>>
{
    private readonly List<List<string?>> _table;

    public CsvTable(FileInfo? fileInfo)
    {
        try
        {
            _table = CreateTableFromFile(fileInfo.FullName);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message + " Could not create CsvTable.");
            _table = new List<List<string?>>();
        }
    }
    
    public List<List<string?>> Table => _table;

    private List<List<string?>> CreateTableFromFile(string? filePath)
    {
        var tempCsvTable = ReadFromFileToList(filePath);
        CheckCsvTableDimensionsEquality(tempCsvTable);
        
        return tempCsvTable;
    }

    private static List<List<string?>> ReadFromFileToList(string? filePath)
    {
        if (!filePath.EndsWith(".csv"))
            throw new Exception("Can't read non csv file.");
        
        var tempCsvTable = File.ReadAllLines(filePath)
                .ToList()
                .PureForEach<List<string>, string, List<List<string?>>, List<string?>>
                    (line => line.Split(',').ToList());
        
        return tempCsvTable;
    }

    private void CheckCsvTableDimensionsEquality(List<List<string?>> table)
    {
        if (table.Count <= 0 || table[0].Count <= 0)
        {
            throw new Exception("CsvTable does not have dimensions. Must be at least (1, 1)");
        }

        var size = table[0].Count;
        foreach (var list in table)
        {
            if (list.Count != size)
                throw new Exception("Difference in table's dimensions. All strokes must be the same size.");
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    public IEnumerator<List<string?>> GetEnumerator()
    {
        var enumerator = _table.GetEnumerator();
        return enumerator;
    }
}