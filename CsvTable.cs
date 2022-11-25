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
            GoThroughTests();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message + " Could not create CsvTable.");
            _table = new List<List<string?>>();
        }
        
        SetShape();
    }
    
    public List<List<string?>> Table => _table;
    
    public Tuple<long, long> Shape { get; set; }
    public object this[int index] => GetColumnWithIndex(Table, index);
    public Dictionary<string, Type> Types;

    private List<List<string?>> CreateTableFromFile(string? filePath)
    {
        return ReadFromFileToList(filePath);
    }
    
    public void SetShape()
    {
        Shape = Tuple.Create<long, long>(Table.Count, Table[0].Count);
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

    // Don't Forget to change for non static private dict<string, Type>
    public static void SetColumnTypes(Dictionary<string, string> types)
    {
        foreach (var (key, value) in types)
        {
            var type = Type.GetType(value);
            if(type != null)
                Console.WriteLine(key + " " + type.ToString());
        }
    } 

    private void GoThroughTests()
    {
        CheckTableDimensionsEquality(_table);
    }

    private void CheckTableDimensionsEquality(List<List<string?>> table)
    {
        var size = table.Count;

        if (size > 0)
        {
            var sizeOfStroke = table[0].Count;
            
            for (var i = 0; i < size; ++i)
            {
                if (table[i].Count != sizeOfStroke)
                    throw new Exception($"Table dimensions are not equal." +
                                        $"{i} stroke of size {table[i].Count}, when first is {sizeOfStroke}");
            }
        }
    }

    private void CheckColumnsDataTypeEquality(List<List<string?>> table)
    {
        if (table.Count > 0)
        {
            for (var i = 0; i < table[0].Count; ++i)
            {
                var column = GetColumnWithIndex(table, i);

                // CheckColumnDataTypeEquality(column);
            }
        }
    }

    private void CheckColumnDataTypeEquality<TData>(List<string?> column)
    where TData : struct, IParsable<TData>
    {
        
    }

    private static void TryAddElement<TData>(string? element, List<TData?> checkList) 
        where TData : struct, IParsable<TData>
    {
        if (element == null)
            checkList.Add(null);
        else
            checkList.Add(TData.Parse(element, CultureInfo.InvariantCulture));
    }

    public List<string?> GetColumnWithIndex(List<List<string?>> table, int index)
    {
        var column = new List<string?>();

        foreach (var stroke in table)
        {
            column.Add(stroke[index]);
        }

        return column;
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