using System;
using System.Drawing;
using System.Windows.Forms;
using TouristAgencyApp.Models;
using System.Linq;

namespace TouristAgencyApp.Forms
{
    partial class ClientsForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            
            // Form properties
            this.Text = "👥 Upravljanje klijentima";
            this.Width = 1400;
            this.Height = 700;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(248, 249, 250);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Initialize controls
            InitializeForm();
            CreateModernUI();
        }

        private void InitializeForm()
        {
            // Form setup
        }

        private void CreateModernUI()
        {
            var headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = Color.FromArgb(42, 204, 113)
            };

            var lblTitle = new Label
            {
                Text = "Upravljanje klijentima",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = false,
                Width = 1400,
                Height = 50,
                Top = 15,
                Left = 0,
                TextAlign = ContentAlignment.MiddleCenter
            };
            headerPanel.Controls.Add(lblTitle);

            var toolbarPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = Color.White,
                Padding = new Padding(20, 10, 20, 10)
            };

            btnUndo = CreateModernButton("↩️ Undo", Color.FromArgb(255, 255, 165, 0));
            btnUndo.Location = new Point(880, 15);
            btnUndo.TextAlign = ContentAlignment.MiddleCenter;
            btnUndo.Click += (s, e) => OpozoviAkciju();
            btnUndo.Width = 100;

            btnRedo = CreateModernButton("Redo ↪️", Color.FromArgb(255, 255, 165, 0));
            btnRedo.Location = new Point(1000, 15);
            btnRedo.TextAlign = ContentAlignment.MiddleCenter;
            btnRedo.Click += (s, e) => NapredAkcija();
            btnRedo.Width = 100;

            var btnAdd = CreateModernButton("➕ Dodaj klijenta", Color.FromArgb(46, 204, 113));
            btnAdd.Click += (s, e) => DodajKlijenta();
            btnAdd.Location = new Point(20, 15);

            var btnEdit = CreateModernButton("✏️ Izmeni klijenta", Color.FromArgb(52, 152, 219));
            btnEdit.Click += (s, e) => IzmeniKlijenta();
            btnEdit.Location = new Point(200, 15);

            var txtPretraga = new TextBox
            {
                PlaceholderText = "🔍 Pretraga po imenu, prezimenu ili broju pasoša...",
                Width = 400,
                Height = 40,
                Font = new Font("Segoe UI", 12, FontStyle.Regular),
                Location = new Point(400, 20),
                BorderStyle = BorderStyle.FixedSingle
            };
            txtPretraga.TextChanged += (s, e) =>
            {
                var searchTerm = txtPretraga.Text.ToLower();
                var filteredClients = _clientFacade.GetAllClients()
                    .Where(c => c.FirstName.ToLower().Contains(searchTerm) ||
                                c.LastName.ToLower().Contains(searchTerm) ||
                                c.PassportNumber.ToLower().Contains(searchTerm))
                    .ToList();
                grid.DataSource = filteredClients;
                AutoSizeGrid();
            };

            toolbarPanel.Controls.AddRange(new Control[] { btnAdd, btnEdit, txtPretraga, btnUndo, btnRedo });

            // Grid setup
            grid.Dock = DockStyle.Fill;
            grid.ReadOnly = true;
            grid.AutoGenerateColumns = true;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.Font = new Font("Segoe UI", 11, FontStyle.Regular);
            grid.RowTemplate.Height = 35;
            grid.AllowUserToAddRows = false;
            grid.BackgroundColor = Color.White;
            grid.BorderStyle = BorderStyle.None;
            grid.GridColor = Color.LightGray;
            grid.EnableHeadersVisualStyles = false;
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(52, 73, 94);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            grid.ColumnHeadersHeight = 40;

            // Add controls to form
            this.Controls.Add(grid);
            this.Controls.Add(toolbarPanel);
            this.Controls.Add(headerPanel);
        }

        private Button CreateModernButton(string text, Color baseColor)
        {
            var button = new Button
            {
                Text = text,
                Size = new Size(150, 40),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = baseColor,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                TextAlign = ContentAlignment.MiddleCenter
            };

            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = Color.FromArgb(
                Math.Max(0, baseColor.R - 20),
                Math.Max(0, baseColor.G - 20),
                Math.Max(0, baseColor.B - 20)
            );

            return button;
        }
    }
} 