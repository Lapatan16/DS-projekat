using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouristAgencyApp.Services;

namespace TouristAgencyApp.Forms
{
    public partial class MainForm1 : Form
    {
        private readonly IDatabaseService _dbService;
        private System.Windows.Forms.Timer backupTimer;
        private readonly string _agencyName;

        public MainForm1(IDatabaseService dbService, string agencyName)
        {
            _dbService = dbService;
            _agencyName = agencyName;
            InitializeMainForm();
            CreateModernUI();
            SetupBackupTimer();
            
        }

        private void InitializeMainForm()
        {
            this.Text = _agencyName;
            this.Width = 800;
            this.Height = 600;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(240, 244, 248);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.DoubleBuffered = true;
        }

        private void CreateModernUI()
        {
           
            var headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 120,
                BackColor = Color.Transparent
            };
            headerPanel.Paint += (s, e) => DrawGradientHeader(e.Graphics, headerPanel.Width, headerPanel.Height);
            this.Controls.Add(headerPanel);

        
            var lblAgency = new Label
            {
                Text = _agencyName,
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = false,
                Width = 800,
                Height = 60,
                Top = 30,
                Left = 0,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };
            headerPanel.Controls.Add(lblAgency);

         
            var lblSubtitle = new Label
            {
                Text = "Sistem za upravljanje turističkim aranžmanima",
                Font = new Font("Segoe UI", 12, FontStyle.Regular),
                ForeColor = Color.FromArgb(236, 240, 241),
                AutoSize = false,
                Width = 800,
                Height = 30,
                Top = 80,
                Left = 0,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };
            headerPanel.Controls.Add(lblSubtitle);

            var menuStrip = new MenuStrip();
            var agencijaMeni = new ToolStripMenuItem("Agencija");
            var promeniAgencijuItem = new ToolStripMenuItem("Promeni agenciju");

            promeniAgencijuItem.Click += (s, e) =>
            {
                var startupForm = new StartupForm();
                this.Hide();
                startupForm.ShowDialog();
                this.Close(); // zatvara trenutni MainForm
            };

            agencijaMeni.DropDownItems.Add(promeniAgencijuItem);
            menuStrip.Items.Add(agencijaMeni);
            this.MainMenuStrip = menuStrip;
            this.Controls.Add(menuStrip);

            var mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(40, 120 + 20, 40, 40)
            };
            this.Controls.Add(mainPanel);

           
            var buttonGrid = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 2,
                ColumnStyles = { new ColumnStyle(SizeType.Percent, 50F), new ColumnStyle(SizeType.Percent, 50F) },
                RowStyles = { new RowStyle(SizeType.Percent, 50F), new RowStyle(SizeType.Percent, 50F) }
            };
            mainPanel.Controls.Add(buttonGrid);

           
            var btnClients = CreateModernButton("👥 Klijenti", "Upravljanje klijentima", Color.FromArgb(46, 204, 113));
            btnClients.Click += (s, e) => new ClientsForm(_dbService).ShowDialog();

            var btnPackages = CreateModernButton("✈️ Paketi", "Upravljanje turističkim paketima", Color.FromArgb(52, 152, 219));
            btnPackages.Click += (s, e) => new PackagesForm(_dbService).ShowDialog();

            var btnReservations = CreateModernButton("📋 Rezervacije", "Upravljanje rezervacijama", Color.FromArgb(155, 89, 182));
            btnReservations.Click += (s, e) => new ReservationsForm(_dbService).ShowDialog();

            var btnBackup = CreateModernButton("💾 Backup", "Ručno kreiranje backup-a", Color.FromArgb(231, 76, 60));
            btnBackup.Click += (s, e) => NapraviBackup(true);

           
            buttonGrid.Controls.Add(btnClients, 0, 0);
            buttonGrid.Controls.Add(btnPackages, 1, 0);
            buttonGrid.Controls.Add(btnReservations, 0, 1);
            buttonGrid.Controls.Add(btnBackup, 1, 1);

            
            var statusBar = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 30,
                BackColor = Color.FromArgb(52, 73, 94)
            };
            this.Controls.Add(statusBar);

            var lblStatus = new Label
            {
                Text = "Sistem spreman za rad",
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                ForeColor = Color.White,
                AutoSize = false,
                Width = 800,
                Height = 30,
                Top = 0,
                Left = 0,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(10, 0, 0, 0)
            };
            statusBar.Controls.Add(lblStatus);
        }

        private Button CreateModernButton(string text, string tooltip, Color baseColor)
        {
            var button = new Button
            {
                Text = text,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = baseColor,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 },
                Dock = DockStyle.Fill,
                Margin = new Padding(10),
                Cursor = Cursors.Hand
            };

            
            var toolTip = new ToolTip();
            toolTip.SetToolTip(button, tooltip);

            
            button.MouseEnter += (s, e) => {
                button.BackColor = ControlPaint.Light(baseColor);
                button.Font = new Font("Segoe UI", 15, FontStyle.Bold);
            };

            button.MouseLeave += (s, e) => {
                button.BackColor = baseColor;
                button.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            };

            return button;
        }

        private void DrawGradientHeader(Graphics g, int width, int height)
        {
            var rect = new Rectangle(0, 0, width, height);
            using var brush = new LinearGradientBrush(rect, Color.FromArgb(52, 152, 219), Color.FromArgb(41, 128, 185), LinearGradientMode.Vertical);
            g.FillRectangle(brush, rect);
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
