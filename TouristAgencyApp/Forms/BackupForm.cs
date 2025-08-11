using System;
using System.Windows.Forms;
using TouristAgencyApp.Patterns;
using TouristAgencyApp.Patterns.Facade;

namespace TouristAgencyApp.Forms
{
    public partial class BackupForm : Form
    {
        public BackupForm()
        {
            InitializeComponent();
        }

        private void btnBackup_Click(object sender, EventArgs e)
        {
            string dbFile = "agencija.db";
            IBackupService backup = new LoggingBackupService(new BackupService(dbFile));
            backup.CreateBackup();
            MessageBox.Show("Backup kreiran!");
        }
    }
}
