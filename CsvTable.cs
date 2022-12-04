using System.Collections;
using System.Reflection;

namespace DBFirstLab;

public class CsvTable : IEnumerable<List<string?>>
{
    private List<List<string?>> _table;

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
            Columns = new List<string>();
        }
        
        SetShape();
    }

    public CsvTable(List<List<string?>> table, CsvTable csv)
    {
        Types = csv.Types;
        Columns = csv.Columns;
        Shape = new Tuple<long?, long?>(null, null);

        try
        {
            _table = table;
            GoThroughTests();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message + $" Could not create CsvTable with table.");
            _table = new List<List<string?>>();
        }
        
        SetShape();
    }

    public List<List<string?>> Table
    {
        get => _table;
        set => _table = value;
    }
    
    /// <summary>
    /// Number of columns, number of strokes
    /// </summary>
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
            var column = GetColumnWithName(columnName);
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

    public List<string?> GetColumnWithName(string columnName)
    {
        return GetColumnWithName(Table, Columns, columnName);
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

    public List<string?> At(int index)
    {
        return Table[index];
    }

    public void MergeByColumn(CsvTable csv, string columnName1, string columnName2)
    {
        var thisColumnIndex = Columns.FindIndex(name => name == columnName1);
        var csvColumnIndex = csv.Columns.FindIndex(name => name == columnName2);

        var elementsIntersection = GetColumnWithIndex(Table, thisColumnIndex)
            .Intersect(GetColumnWithIndex(csv.Table, csvColumnIndex));

        Table = CreateMergedTable(csv, elementsIntersection, csvColumnIndex, thisColumnIndex);

        Columns.AddRange(csv.Columns);
        Columns.Remove(columnName2);
        
        foreach (var (name, type) in csv.Types)
            Types.Add(name, type);
        
        Types.Remove(columnName2);
        
        SetShape();
        GoThroughTests();
    }

    private List<List<string?>> CreateMergedTable(CsvTable csv, 
        IEnumerable<string?> elementsIntersection, 
        int csvColumnIndex, 
        int thisColumnIndex)
    {
        var mergedTables = new List<List<string?>>();

        foreach (var element in elementsIntersection)
        {
            if (element != null)
            {
                var csvStrokes = csv.Table
                    .FindAll(stroke => stroke[csvColumnIndex] == element);

                var thisStroke = Table.Find(stroke => stroke[thisColumnIndex] == element);

                foreach (var stroke in csvStrokes)
                {
                    var csvStrokeCopy = new List<string?>(stroke);
                    csvStrokeCopy.RemoveAt(csvColumnIndex);

                    var thisStrokeCopy = new List<string?>(thisStroke);

                    thisStrokeCopy.AddRange(csvStrokeCopy);
                    mergedTables.Add(thisStrokeCopy);
                }
            }
        }

        return mergedTables;
    }

    public void Print()
    {
        if (Table.Count == 0 && Columns.Count == 0)
        {
            Console.WriteLine("Empty");
            return;
        }

        var columnWidths = new List<int>();
        foreach (var column in Columns)
        {
            columnWidths.Add(GetColumnWidth(column));
        }

        PrintColumns(columnWidths);
        PrintContent(columnWidths);
        PrintBorder(columnWidths);
    }

    private void PrintContent(List<int> columnWidths)
    {
        foreach (var stroke in this)
        {
            var enumerator = columnWidths.GetEnumerator();
            enumerator.MoveNext();

            for (var i = 0; i < stroke.Count; ++i)
            {
                if(stroke[i] == null)
                    Console.Write($"|n/a" + new string(' ', enumerator.Current - 3));
                else
                    Console.Write($"|{stroke[i]}" + new string(' ', enumerator.Current - stroke[i].Length));
                
                enumerator.MoveNext();
            }

            Console.WriteLine('|');
        }
    }

    private void PrintBorder(List<int> widths)
    {
        var lens = new List<int>() {0};
        int len = 0;
        
        foreach (var width in widths)
        {
            len += width + 1;
            lens.Add(len);
        }
        
        for (var i = 0; i < widths.Sum() + widths.Count + 1; ++i)
        {
            if(lens.Contains(i))
                Console.Write('|');
            else
                Console.Write('-');
        }
        
        Console.WriteLine();
    }

    private void PrintColumns(List<int> columnWidths)
    {
        int length = 0;
        List<int> lens = new List<int>();
        
        foreach (var (width, name) in columnWidths.Zip(Columns))
        {
            string output = $"|{name}" + new string(' ', width - name.Length);
            length += output.Length;
            lens.Add(length);
            Console.Write(output);
        }
        Console.WriteLine("|");
        Console.Write("|");

        for (var i = 0; i < length; ++i)
        {
            if(lens.Contains(i+1))
                Console.Write('|');
            else
                Console.Write('-');
        }
        Console.WriteLine();
    }
    
    private int GetColumnWidth(string columnName)
    {
        var column = GetColumnWithName(columnName);

        int length = 0;
        foreach (var element in column)
        {
            if (element!= null && element.Length > length)
                length = element.Length;
        }

        try
        {
            var smt = Columns.Find(name => name == columnName).Length;
        }
        catch (Exception)
        {
            Console.WriteLine($"There is no column with name {columnName}");
            throw;
        }

        return Math.Max(length, Columns.Find(name => name == columnName).Length);
    }
}