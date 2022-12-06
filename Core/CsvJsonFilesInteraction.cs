namespace DBFirstLab.Core;

public static class CsvJsonFilesInteraction
{
    public static List<CsvTable> LoadTables(string tablesDirectoryPath, string jsonPath)
    {
        FileInfo myFile = new FileInfo(@"C:\Users\Administrator\RiderProjects\DBFirstLab\structure.json");
            
        var directoryInfo = Directory.CreateDirectory(tablesDirectoryPath);
        var fileEnumerator = directoryInfo.EnumerateFiles("*.csv");
            
        var configDict = JsonParser.Parse(myFile);
        var dbTables = new List<CsvTable>();

        foreach (var csvFile in fileEnumerator)
        {
            var config = FindConfigForFile(csvFile, configDict);

            if (config != null)
                CreateCsvTableAndAddToCollection(csvFile, dbTables, config);
        }

        return dbTables;
    }
    
    private static void CreateCsvTableAndAddToCollection(FileInfo csvFile, 
        List<CsvTable> dbTables, 
        Dictionary<string, string> configuration)
    {
        try
        {
            var table = new CsvTable(csvFile, configuration);
            dbTables.Add(table);
        }
        catch (NullReferenceException exception)
        {
            Console.WriteLine(exception.Message);
        }
    }
    public static Dictionary<string, string> FindConfigForFile(FileInfo csvFile, 
        Dictionary<string, Dictionary<string, string>> configuration)
    {
        var dbStructureNames = configuration.Keys.ToList();
            
        try
        {
            var dbKey = dbStructureNames.Find(key => key == csvFile.Name);
            return configuration[dbKey];
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Could not find name {csvFile.Name} in structure.");
            throw;
        }
    }
}