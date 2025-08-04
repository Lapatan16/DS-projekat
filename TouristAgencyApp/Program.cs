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

            if (config.ConnectionString.ToLower().Contains("data source"))
                dbService = new SQLiteDatabaseService(config.ConnectionString);
            else
                dbService = new MySQLDatabaseService(config.ConnectionString);

            //var builder = new Microsoft.Data.Sqlite.SqliteConnectionStringBuilder(config.ConnectionString);
            //string dbPath = Path.GetFullPath(builder.DataSource);
            //MessageBox.Show(dbPath, "Full DB Path");

            Application.Run(new MainForm(dbService));
        }
    }
}