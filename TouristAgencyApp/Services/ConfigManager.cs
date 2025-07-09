using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TouristAgencyApp.Services
{
    public class ConfigManager
    {
        private static ConfigManager? _instance;
        public string AgencyName { get; private set; } = "";
        public string ConnectionString { get; private set; } = "";

        private ConfigManager()
        {
            var lines = File.ReadAllLines("config.txt");
            AgencyName = lines.Length > 0 ? lines[0] : "Agencija";
            ConnectionString = lines.Length > 1 ? lines[1] : "";
        }

        public static ConfigManager Instance => _instance ??= new ConfigManager();
    }
}
