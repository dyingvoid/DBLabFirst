using DBFirstLab.Controller;


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
            string jsonPath = @"C:\Users\Administrator\RiderProjects\DBFirstLab\structure.json";

            DbController.ViewTables(directoryPath, jsonPath);
        }
    }
}