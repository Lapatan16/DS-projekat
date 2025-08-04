using TouristAgencyApp.Forms;
using TouristAgencyApp.Services;

namespace TouristAgencyApp
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            var config = ConfigManager.Instance;
            IDatabaseService dbService;

            // daj mi interfejs
            if (config.ConnectionString.ToLower().Contains("data source"))
                dbService = new SQLiteDatabaseService(config.ConnectionString);
            else
                dbService = new MySQLDatabaseService(config.ConnectionString);

            Application.Run(new MainForm(dbService));
        }
    }
}