using System.Reflection;

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

            dbTables[0].MergeByColumn(dbTables[1], "Name", "BookName");
            foreach (var table in dbTables)
            {
                table.Print();
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