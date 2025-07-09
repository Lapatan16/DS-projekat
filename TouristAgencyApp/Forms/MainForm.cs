using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouristAgencyApp.Services;

namespace TouristAgencyApp.Forms
{
    //public partial class MainForm : Form
    //{
    //    private readonly IDatabaseService _dbService;

    //    public MainForm(IDatabaseService dbService)
    //    {
    //        //InitializeComponent();
    //        _dbService = dbService;
    //        this.Text = ConfigManager.Instance.AgencyName;
    //        this.Width = 350; this.Height = 320;

    //        Button btnClients = new Button { Text = "Klijenti", Width = 200, Top = 30, Left = 60 };
    //        btnClients.Click += (s, e) => new ClientsForm(_dbService).ShowDialog();

    //        Button btnPackages = new Button { Text = "Paketi", Width = 200, Top = 80, Left = 60 };
    //        btnPackages.Click += (s, e) => new PackagesForm(_dbService).ShowDialog();

    //        Button btnReservations = new Button { Text = "Rezervacije", Width = 200, Top = 130, Left = 60 };
    //        btnReservations.Click += (s, e) => new ReservationsForm(_dbService).ShowDialog();

    //        Button btnBackup = new Button { Text = "Backup", Width = 200, Top = 180, Left = 60 };
    //        btnBackup.Click += (s, e) => new BackupForm(_dbService).ShowDialog();

    //        this.Controls.Add(btnClients);
    //        this.Controls.Add(btnPackages);
    //        this.Controls.Add(btnReservations);
    //        this.Controls.Add(btnBackup);
    //    }
    //}
    public partial class MainForm : Form
    {
        private readonly IDatabaseService _dbService;
        private System.Windows.Forms.Timer backupTimer;

        public MainForm(IDatabaseService dbService)
        {
            _dbService = dbService;
            this.Text = ConfigManager.Instance.AgencyName;
            this.Width = 350; this.Height = 320;

            Button btnClients = new Button { Text = "Klijenti", Width = 200, Top = 30, Left = 60 };
            btnClients.Click += (s, e) => new ClientsForm(_dbService).ShowDialog();

            Button btnPackages = new Button { Text = "Paketi", Width = 200, Top = 80, Left = 60 };
            btnPackages.Click += (s, e) => new PackagesForm(_dbService).ShowDialog();

            Button btnReservations = new Button { Text = "Rezervacije", Width = 200, Top = 130, Left = 60 };
            btnReservations.Click += (s, e) => new ReservationsForm(_dbService).ShowDialog();

            Button btnBackup = new Button { Text = "Backup (ručno)", Width = 200, Top = 180, Left = 60 };
            btnBackup.Click += (s, e) => NapraviBackup(true);

            this.Controls.Add(btnClients);
            this.Controls.Add(btnPackages);
            this.Controls.Add(btnReservations);
            this.Controls.Add(btnBackup);

            // AUTOMATSKI BACKUP NA 24h
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
