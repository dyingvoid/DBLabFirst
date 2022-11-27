using System.Reflection;

namespace DBFirstLab
{
    class Program
    {
        static void Main()
        {
            Start();
            // Test();
        }

        public static void Test()
        {
            string time = "2/16/2008 12:15:12 PM";
            var newtime = time.ToTypeWithStructConstraint<DateTime>();
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
                
                if(config != null)
                    CreateCsvTableAndAddToCollection(csvFile, dbTables, config); 
            }

            //var bookList = InitializeObjects<Book>(dbTables[0], list => new Book(list));
            //var history = new History(dbTables[1]);
            //var customerList = InitializeObjects<Customer>(dbTables[2], list => new Customer(list));
            
            Console.WriteLine("Success");
        }

        public static Dictionary<string, string>? FindConfigForFile(FileInfo csvFile, 
            Dictionary<string, Dictionary<string, string>> configuration)
        {
            var properties = File.ReadAllLines(csvFile.FullName)[0].Split(',').ToHashSet();

            foreach (var (key, configDict) in configuration)
            {
                var keys = configDict.Keys.ToHashSet();
                if (properties.SetEquals(keys))
                {
                    return configDict;
                }
            }

            return null;
        }
        
        public static List<TObj> InitializeObjects<TObj>(CsvTable table, Func<List<string?>, TObj> constructorDelegate)
        {
            var objectList = new List<TObj>();
            
            foreach (var stroke in table)
            {
                objectList.Add(constructorDelegate(stroke));
            }

            return objectList;
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