using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouristAgencyApp.Services;

namespace TouristAgencyApp.Patterns
{
    public class DatabaseFactory
    {
        private static IDatabaseService? _instance;
        private static readonly object _lock = new();

        public static IDatabaseService GetDatabaseService(string configFile)
        {
            if (_instance != null)
                return _instance;

            lock (_lock)
            {
                if (_instance == null)
                {
                    var config = new ConfigManager(configFile);
                    string conn = config.ConnectionString;

                    if (conn.ToLower().Contains("data source"))
                        _instance = new SQLiteDatabaseService(conn);
                    else if (conn.ToLower().Contains("server="))
                        _instance = new MySQLDatabaseService(conn);
                    else
                        throw new NotSupportedException("Nepoznata baza.");
                }
            }

            return _instance;
        }

        public static void Reset()
        {
            lock (_lock)
            {
                _instance = null;
            }
        }
    }

}
