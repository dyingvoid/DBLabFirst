using System.Collections;
using System.Globalization;

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

    public void MakeEmptyAndSpaceElementsNull()
    {
        for (var i = 0; i < _table.Count; ++i)
        {
            for (var j = 0; j < _table[i].Count; ++j)
            {
                if(_table[i][j].IsEmptyOrWhiteSpace())
                    _table[i][j] = null;
            }
        }
    }

    public void FillNullsWithValue(string value)
    {
        for (var i = 0; i < _table.Count; ++i)
        {
            for (var j = 0; j < _table[i].Count; ++j)
            {
                if(_table[i][j] == null)
                    _table[i][j] = value;
            }
        }
    }

    public List<TValue> GetColumnGenericType<TValue>(string? columnName)
    where TValue : IParsable<TValue>
    {
        int columnNameInt = _table[0].FindIndex(str => str == columnName);
        
        if (columnNameInt == -1)
            throw new ArgumentException("Column was not found.");
        
        List<TValue> outList = _table[columnNameInt]
            .Select(str => TValue.Parse(str, CultureInfo.InvariantCulture)).ToList();

        return outList;
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