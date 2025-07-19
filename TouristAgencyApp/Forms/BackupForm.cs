using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouristAgencyApp.Patterns;

namespace TouristAgencyApp.Forms
{
    public partial class BackupForm : Form
    {
        public BackupForm(object dbService)
        {
            this.Text = "Backup";
            Button btnBackup = new Button { Text = "Backup baze", Dock = DockStyle.Fill };
            btnBackup.Click += (s, e) =>
            {
                string dbFile = "agencija.db";
                IBackupService backup = new LoggingBackupService(new BackupService(dbFile));
                backup.CreateBackup();
                MessageBox.Show("Backup kreiran!");
            };
            this.Controls.Add(btnBackup);
        }
    }
}
