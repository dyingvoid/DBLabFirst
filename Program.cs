namespace DBFirstLab
{
    class Program
    {
        static void Main()
        {
            string? fileName = @"C:\Users\Administrator\Downloads\tableConvert.com_blj4hh.csv";
            var csvFile = new CsvTable(fileName);

            var list = new List<string?>() { "" };
            var book = new Book(list);
        }
    }
}