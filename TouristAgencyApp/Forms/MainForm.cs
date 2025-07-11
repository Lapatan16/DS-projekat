using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouristAgencyApp.Services;

namespace TouristAgencyApp.Forms
{
    public partial class MainForm : Form
    {
        private readonly IDatabaseService _dbService;
        private System.Windows.Forms.Timer backupTimer;

        public MainForm(IDatabaseService dbService)
        {
            _dbService = dbService;
            this.Text = ConfigManager.Instance.AgencyName;
            this.Width = 420;
            this.Height = 380;

            var lblAgency = new Label
            {
                Text = ConfigManager.Instance.AgencyName,
                Font = new Font("Segoe UI", 17, FontStyle.Bold),
                AutoSize = false,
                Width = 420,
                Height = 38,
                Top = 10,
                Left = 0,
                TextAlign = ContentAlignment.MiddleCenter
            };
            this.Controls.Add(lblAgency);

            Button btnClients = new Button { Text = "Klijenti", Width = 220, Height = 48, Top = 60, Left = 90, TextAlign = ContentAlignment.MiddleCenter };
            btnClients.Click += (s, e) => new ClientsForm(_dbService).ShowDialog();

            Button btnPackages = new Button { Text = "Paketi", Width = 220, Height = 48, Top = 120, Left = 90, TextAlign = ContentAlignment.MiddleCenter };
            btnPackages.Click += (s, e) => new PackagesForm(_dbService).ShowDialog();

            Button btnReservations = new Button { Text = "Rezervacije", Width = 220, Height = 48, Top = 180, Left = 90, TextAlign = ContentAlignment.MiddleCenter };
            btnReservations.Click += (s, e) => new ReservationsForm(_dbService).ShowDialog();

            Button btnBackup = new Button { Text = "Backup (ručno)", Width = 220, Height = 48, Top = 240, Left = 90, TextAlign = ContentAlignment.MiddleCenter };
            btnBackup.Click += (s, e) => NapraviBackup(true);

            this.Controls.Add(btnClients);
            this.Controls.Add(btnPackages);
            this.Controls.Add(btnReservations);
            this.Controls.Add(btnBackup);
            this.StartPosition = FormStartPosition.CenterScreen;
            // Automatski backup na 24h
            backupTimer = new System.Windows.Forms.Timer();
            backupTimer.Interval = 24 * 60 * 60 * 1000; // 24h u ms
            backupTimer.Tick += (s, e) => NapraviBackup();
            backupTimer.Start();
        }

        private void NapraviBackup(bool showMsg = false)
        {
            string dbFile = "agencija.db";
            var backup = new TouristAgencyApp.Patterns.LoggingBackupService(new TouristAgencyApp.Patterns.BackupService(dbFile));
            backup.CreateBackup();
            if (showMsg)
                MessageBox.Show("Backup kreiran!");
        }
    }
}
