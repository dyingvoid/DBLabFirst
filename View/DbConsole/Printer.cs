using DBFirstLab.Core;

namespace DBFirstLab.View.DbConsole;

public static class Printer
{
    public static void Print(CsvTable csv)
    {
        if (csv.Table.Count == 0 && csv.Columns.Count == 0)
        {
            Console.WriteLine("Empty");
            return;
        }

        var columnWidths = new List<int>();
        foreach (var column in csv.Columns)
        {
            columnWidths.Add(csv.GetColumnWidth(column));
        }

        PrintColumns(csv, columnWidths);
        PrintContent(csv, columnWidths);
        PrintBorder(columnWidths);
    }
    
    private static void PrintContent(CsvTable csv, List<int> columnWidths)
    {
        foreach (var stroke in csv)
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
    
    private static void PrintBorder(List<int> widths)
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

    private static void PrintColumns(CsvTable csv, List<int> columnWidths)
    {
        int length = 0;
        List<int> lens = new List<int>();
        
        foreach (var (width, name) in columnWidths.Zip(csv.Columns))
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
}