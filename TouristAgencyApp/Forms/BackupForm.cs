using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouristAgencyApp.Patterns;
using TouristAgencyApp.Patterns.Facade;

namespace TouristAgencyApp.Forms
{
    public partial class BackupForm : Form
    {
        public BackupForm()
        {
            this.Text = "Backup";
            Button btnBackup = new Button { Text = "Backup baze", Dock = DockStyle.Fill };
            btnBackup.Click += (s, e) =>
            {
                var backupFacade = new BackupFacade("agencija.db");
                backupFacade.CreateBackup();
            };
            this.Controls.Add(btnBackup);
        }
    }
}
