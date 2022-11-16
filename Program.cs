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
            var directoryInfo = Directory.CreateDirectory(directoryPath);
            var fileEnumerator = directoryInfo.EnumerateFiles("*.csv");

            var dbTables = new List<CsvTable>();
            foreach (var csvFile in fileEnumerator)
            {
                CreateCsvTableAndAddToCollection(csvFile, dbTables);
            }
            
            TestBook(dbTables);
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