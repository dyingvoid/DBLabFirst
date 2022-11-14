namespace DBFirstLab
{
    class Program
    {
        static void Main()
        {
            string directoryPath = @"C:\Users\Administrator\Downloads\csvs";
            var directoryInfo = Directory.CreateDirectory(directoryPath);
            var fileEnumerator = directoryInfo.EnumerateFiles("*.csv");

            var dbTables = new List<CsvTable>();
            foreach (var csvFile in fileEnumerator)
            {
                CreateCsvTableAndAddToCollection(csvFile, dbTables);
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