using System.Globalization;

namespace DBFirstLab
{
    class Program
    {
        static void Main()
        {
            // Start();
            FileInfo file = new FileInfo(@"C:\Users\Wlad\RiderProjects\DBLabFirst\structure.json");

            var dict = Controller.ParseJson(file);

            CsvTable.SetColumnTypes(dict["ConsumerTable"]);
            Console.WriteLine(typeof(long));
        }

        public static void Start()
        {
            string directoryPath = @"C:\Users\Administrator\Downloads\csvs";
            var directoryInfo = Directory.CreateDirectory(directoryPath);
            var fileEnumerator = directoryInfo.EnumerateFiles("*.csv");

            var dbTables = new List<CsvTable>();
            foreach (var csvFile in fileEnumerator)
            {
                CreateCsvTableAndAddToCollection(csvFile, dbTables); 
            }

            var bookList = InitializeObjects<Book>(dbTables[0], list => new Book(list));
            var history = new History(dbTables[1]);
            var customerList = InitializeObjects<Customer>(dbTables[2], list => new Customer(list));
            
            Console.WriteLine("Success");
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

        private static void CreateCsvTableAndAddToCollection(FileInfo csvFile, List<CsvTable> dbTables)
        {
            try
            {
                var table = new CsvTable(csvFile);
                dbTables.Add(table);
            }
            catch (NullReferenceException exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
    }
}