namespace DBFirstLab;

public class CsvTable
{
    private readonly List<List<string?>> _table;

    public List<List<string?>> Table => _table;

    public CsvTable(string? filePath)
    {
        _table = ReadFile(filePath);
    }

    private List<List<string?>> ReadFile(string? filePath)
    {
        var tempCsvTable = File.ReadAllLines(filePath)
            .ToList()
            .PureForEach<List<string>, string, List<List<string?>>, List<string?>>
                (line => line.Split(',').ToList());
        
        CheckCsvFileSize(tempCsvTable);
        
        return tempCsvTable;
    }

    private void CheckCsvFileSize(List<List<string?>> table)
    {
        var size = table[0].Count;
        foreach (var list in table)
        {
            if (list.Count != size)
                throw new Exception("Difference in table's dimensions. All strokes must be the same size.");
        }
    }

    public void MakeNa()
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

    public void FillNa(string value)
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
}