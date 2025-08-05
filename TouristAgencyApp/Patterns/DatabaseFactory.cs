using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouristAgencyApp.Services;

namespace TouristAgencyApp.Patterns
{
    public static class DatabaseFactory
    {
        private static readonly ConcurrentDictionary<string, IDatabaseService> _instances = new();
        private static readonly object _lock = new();

        public static IDatabaseService GetDatabaseService(string configPath)
        {
            if (_instances.TryGetValue(configPath, out var existingInstance))
            {
                return existingInstance;
            }

            lock (_lock)
            {
                if (_instances.TryGetValue(configPath, out existingInstance))
                {
                    return existingInstance;
                }

                var config = new ConfigManager(configPath);
                IDatabaseService dbService;

                string connectionString = config.ConnectionString.ToLower();

                if (connectionString.Contains("data source"))
                {
                    dbService = new SQLiteDatabaseService(config.ConnectionString);
                }
                else if (connectionString.Contains("server") || connectionString.Contains("uid") || connectionString.Contains("mysql"))
                {
                    dbService = new MySQLDatabaseService(config.ConnectionString);
                }
                else
                {
                    throw new InvalidOperationException("Nepoznata baza podataka u config fajlu.");
                }

                _instances[configPath] = dbService;
                // MessageBox.Show($"Napravljen novi database service za: {Path.GetFileName(configPath)}");

                return dbService;
            }
        }

        public static void ResetAll()
        {
            _instances.Clear();
        }
    }

}
