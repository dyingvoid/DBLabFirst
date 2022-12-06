using DBFirstLab.DbConsole;

namespace DBFirstLab
{
    class Program
    {
        static void Main()
        {
            Start();
        }

        public static void Start()
        {
            string directoryPath = @"C:\Users\Administrator\Downloads\csvs";
            FileInfo myFile = new FileInfo(@"C:\Users\Administrator\RiderProjects\DBFirstLab\structure.json");
            
            var directoryInfo = Directory.CreateDirectory(directoryPath);
            var fileEnumerator = directoryInfo.EnumerateFiles("*.csv");
            
            var configDict = Controller.ParseJson(myFile);
            var dbTables = new List<CsvTable>();

            foreach (var csvFile in fileEnumerator)
            {
                var config = FindConfigForFile(csvFile, configDict);

                if (config != null)
                    CreateCsvTableAndAddToCollection(csvFile, dbTables, config);
            }
            
            foreach (var table in dbTables)
            {
                Printer.Print(table);
                Console.WriteLine();
            }
        }

        public static void PrintBookAvailability(CsvTable bookTable)
        {
            for(var i = 0; i < bookTable.Table.Count; ++i)
            {
                var strokeCopies = bookTable.Table
                    .FindAll(strokeCopy => strokeCopy[0] == bookTable.Table[i][0]);

                for (var j = 0; j < strokeCopies.Count - 1; ++j)
                {
                    bookTable.Table.Remove(strokeCopies[j]);
                }
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
    }
}