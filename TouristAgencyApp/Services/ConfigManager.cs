using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TouristAgencyApp.Services
{
    public class ConfigManager
    {
        public string AgencyName { get; private set; }
        public string ConnectionString { get; private set; }

        public ConfigManager(string configPath)
        {
            var lines = File.ReadAllLines(configPath);
            AgencyName = lines.Length > 0 ? lines[0] : "Agencija";
            ConnectionString = lines.Length > 1 ? lines[1] : "";
        }
    }
}
