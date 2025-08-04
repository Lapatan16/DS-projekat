using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TouristAgencyApp.Services;

namespace TouristAgencyApp.Forms
{
    public partial class MainForm : Form
    {
        private readonly IDatabaseService _dbService;
        private readonly string _agencyName;
        private System.Windows.Forms.Timer backupTimer;

        public MainForm(IDatabaseService dbService, string agencyName)
        {
            _dbService = dbService;
            _agencyName = agencyName;
            InitializeComponent();
            SetupBackupTimer();
        }

        private void SetupBackupTimer()
        {
            backupTimer = new System.Windows.Forms.Timer();
            backupTimer.Interval = 24 * 60 * 60 * 1000;
            backupTimer.Tick += (s, e) => NapraviBackup();
            backupTimer.Start();
        }

        private void NapraviBackup(bool showMsg = false)
        {
            try
            {
                string dbFile = "agencija.db";
                var backup = new TouristAgencyApp.Patterns.LoggingBackupService(new TouristAgencyApp.Patterns.BackupService(dbFile));
                backup.CreateBackup();
                if (showMsg)
                    MessageBox.Show("Backup uspešno kreiran!", "Backup", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška pri kreiranju backup-a: {ex.Message}", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
