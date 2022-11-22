namespace DBFirstLab
{
    class Program
    {
        static void Main()
        {
            // Start();
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
            TestCsvTable(dbTables);
        }

        static void TestBook(List<CsvTable> dbTables)
        {
            var bookTable = dbTables[0];
            var lBook = new List<Book>();

            foreach (var dbBook in bookTable)
            {
                lBook.Add(new Book(dbBook));
            }
        }

        static void TestCustomer(List<CsvTable> dbTables)
        {
            var customerTable = dbTables[1];
            var lCustomer = new List<Customer>();

            foreach (var dbCustomer in customerTable)
            {
                lCustomer.Add(new Customer(dbCustomer));
            }
        }

        static void TestCsvTable(List<CsvTable> dbTables)
        {
            //test for null, empty file, empty table, wrong-sized table,
            //large table, table with first stroke as column names and without it
            
            var csvs = new List<CsvTable>();
            
            csvs.Add(dbTables[1]);
            csvs.Add(new CsvTable(new FileInfo(@"C:\Users\Administrator\Downloads\csvs\test.txt")));
        }

        static void TestHistory()
        {
            // Test History construction and comparison History with existence of customer and book
            // in customer and book tables respectively
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