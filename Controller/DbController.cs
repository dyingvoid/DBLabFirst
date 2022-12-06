using DBFirstLab.Core;
using DBFirstLab.View.DbConsole;

namespace DBFirstLab.Controller;

public static class DbController
{
    public static void ViewTables(string tablesDirectoryPath, string jsonPath)
    {
        var listOfTables = CsvJsonFilesInteraction.LoadTables(tablesDirectoryPath, jsonPath);

        foreach (var table in listOfTables)
        {
            Printer.Print(table);
        }
    }
}