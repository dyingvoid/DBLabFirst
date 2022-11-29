using System.Collections;
using System.Reflection;

namespace DBFirstLab;

public class CsvTable : IEnumerable<List<string?>>
{
    private readonly List<List<string?>> _table;

    public CsvTable(FileInfo csvFile, Dictionary<string, string> configuration)
    {
        Types = new Dictionary<string, Type>();
        Columns = new List<string>();
        Shape = new Tuple<long?, long?>(null, null);
        
        try
        {
            _table = CreateTableFromFile(csvFile.FullName);
            SetColumnTypes(configuration);
            SetColumns();
            Table.RemoveAt(0);
            MakeEmptyAndSpaceElementsNull();
            
            GoThroughTests();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message + $" Could not create CsvTable with {csvFile.Name}.");
            _table = new List<List<string?>>();
        }
        
        SetShape();
    }
    
    public List<List<string?>> Table => _table;
    public Tuple<long?, long?> Shape { get; set; }
    public object this[int index] => GetColumnWithIndex(Table, index);
    public Dictionary<string, Type> Types { get; }
    public List<string> Columns { get; set; }

    private void SetColumns()
    {
        try
        {
            Columns = Table[0];
        }
        catch (Exception)
        {
            Console.WriteLine("Could not set table columns.");
            throw;
        }
    }

    private List<List<string>> CreateTableFromFile(string filePath)
    {
        return ReadFromFileToList(filePath);
    }
    
    public void SetShape()
    {
        Shape = Tuple.Create<long?, long?>(Columns.Count, Table.Count);
    }

    private static List<List<string>> ReadFromFileToList(string filePath)
    {
        if (!filePath.EndsWith(".csv"))
            throw new Exception("Can't read non csv file.");
        
        var tempCsvTable = File.ReadAllLines(filePath)
                .ToList()
                .PureForEach<List<string>, string, List<List<string>>, List<string>>
                    (line => line.Split(',').ToList());
        
        return tempCsvTable;
    }
    
    private void SetColumnTypes(Dictionary<string, string> types)
    {
        foreach (var (key, value) in types)
        {
            var type = Type.GetType(value);
            try
            {
                Types.Add(key, type);
            }
            catch (Exception)
            {
                Console.WriteLine("Could not set column types.");
                throw;
            }
        }
    }

    private void GoThroughTests()
    {
        CheckStructureEquality(Types, Columns);
        CheckTableDimensionsEquality(Table);
        CheckColumnsDataTypeEquality(Table, Types);
    }

    private void CheckTableDimensionsEquality(List<List<string?>> table)
    {
        var size = Columns.Count;
        if (size <= 0)
            throw new Exception("Could not find any column in table.");

        for (int i = 0; i < table.Count; ++i)
        {
            if (table[i].Count != size)
            {
                throw new Exception($"Stroke with index {i}(or {i + 2} in file) is size of {table[i].Count}," +
                                    $"when must be {size}");
            }
        }
    }

    private void CheckStructureEquality(Dictionary<string, Type> structure, List<string> columns)
    {
        var structureNames = structure.Keys.ToHashSet();
        var columnNames = columns.ToHashSet();

        if (!structureNames.SetEquals(columnNames))
            throw new Exception("Column names do not match names in json structure.");
    }

    private void CheckColumnsDataTypeEquality(List<List<string?>> table, Dictionary<string, Type> structure)
    {
        if (table.Count <= 0) return;
        
        // Foreach column
        foreach (var columnName in Columns)
        {
            var column = GetColumnWithName(Table, Columns, columnName);
            var columnType = FindTypeInJsonByColumnName(structure, columnName);
            
            try
            {
                CheckColumnDataTypeEquality(column, columnType);
            }
            catch (Exception)
            {
                Console.WriteLine($"Error in {columnName} column.");
                throw;
            }
        }
    }

    private static Type FindTypeInJsonByColumnName(Dictionary<string, Type> structure, string? name)
    {
        try
        {
            var type = structure[name];
            return type;
        }
        catch (Exception)
        {
            Console.WriteLine($"Could not find {name} column in json structure.");
            throw;
        }
    }

    private static void CheckColumnDataTypeEquality(List<string?> column, Type type)
    {
        foreach (var element in column)
        {
            if (type != typeof(String))
            {
                var castGenericMethod = TryMakeGenericWithType(type);
                TryCastToType(type, castGenericMethod, element);
            }
        }
    }

    private static void TryCastToType(Type type, MethodInfo castGenericMethod, string? element)
    {
        try
        {
            castGenericMethod.Invoke(null, new object[] { element });
        }
        catch (Exception)
        {
            Console.WriteLine($"Element '{element}' can't be casted to type {type}.");
            throw;
        }
    }

    private static MethodInfo TryMakeGenericWithType(Type type)
    {
        try
        {

            var methodInfo = ChooseGenericMethodByTypeConstraints(type);
            
            return methodInfo;
        }
        catch (Exception)
        {
            Console.WriteLine($"{type} type caused error.");
            throw;
        }
    }
    
    private static MethodInfo ChooseGenericMethodByTypeConstraints(Type type)
    {
        if (type.IsValueType && !type.IsEnum)
            return typeof(Extensions).GetMethod("ToTypeWithStructConstraint").MakeGenericMethod(type);
        if (type.IsEnum)
            return typeof(Extensions).GetMethod("ToTypeEnumConstraint").MakeGenericMethod(type);

        return typeof(Extensions).GetMethod("ToTypeWithClassConstraint").MakeGenericMethod(type);
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

    public List<string?> GetColumnWithName(List<List<string?>> table, List<string> columnNames, string columnName)
    {
        var indexOfColumn = columnNames.FindIndex(name=> name == columnName);
        
        if (indexOfColumn < 0)
            throw new Exception($"Could not find column with name {columnName} in Columns");

        try
        {
            return GetColumnWithIndex(table, indexOfColumn);
        }
        catch (Exception)
        {
            Console.WriteLine($"Could not find data of {columnName} column. Check format of table");
            throw;
        }
    }

    public void MakeEmptyAndSpaceElementsNull()
    {
        foreach (var stroke in _table)
        {
            for (var j = 0; j < stroke.Count; ++j)
            {
                if(stroke[j].IsEmptyOrWhiteSpace())
                    stroke[j] = null;
            }
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