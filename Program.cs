using System.Reflection;

namespace DBFirstLab
{
    class Program
    {
        static void Main()
        {
            // Start();
            // Test();

            var type = typeof(Int32);
            
            var methodInfo = typeof(Extensions).GetMethod("ToType",
                BindingFlags.Public | BindingFlags.Static).MakeGenericMethod(type);

            var elem = methodInfo.Invoke(null, new object[] { "123" });
            Console.WriteLine(elem.GetType());
        }

        public static void Start()
        {
            string directoryPath = @"C:\Users\Administrator\Downloads\csvs";
            FileInfo myFile = new FileInfo(@"C:\Users\Administrator\RiderProjects\DBFirstLab\structure.json");
            
            var directoryInfo = Directory.CreateDirectory(directoryPath);
            var fileEnumerator = directoryInfo.EnumerateFiles("*.csv");
            var configuration = Controller.ParseJson(myFile);

            var dbTables = new List<CsvTable>();
            foreach (var (csvFile, (key, config)) in Enumerable.Zip(fileEnumerator, configuration))
            {
                CreateCsvTableAndAddToCollection(csvFile, dbTables, config); 
            }

            var bookList = InitializeObjects<Book>(dbTables[0], list => new Book(list));
            var history = new History(dbTables[1]);
            var customerList = InitializeObjects<Customer>(dbTables[2], list => new Customer(list));
            
            Console.WriteLine("Success");
        }

        public static void Test()
        {
            FileInfo wladFile = new FileInfo(@"C:\Users\Wlad\RiderProjects\DBLabFirst\structure.json");
            FileInfo myFile = new FileInfo(@"C:\Users\Administrator\RiderProjects\DBFirstLab\structure.json");
            var bookFile = new FileInfo(@"C:\Users\Administrator\Downloads\csvs\Book.csv");

            var dict = Controller.ParseJson(myFile);

            var csv = new CsvTable(bookFile, dict["BookTable"]);
        }

        public static List<TObj> InitializeObjects<TObj>(CsvTable table, Func<List<string?>, TObj> del)
        {
            var bookList = new List<TObj>();
            
            foreach (var stroke in table)
            {
                bookList.Add(del(stroke));
            }

            return bookList;
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