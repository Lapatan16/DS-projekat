using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouristAgencyApp.Models;
using TouristAgencyApp.Services;

namespace TouristAgencyApp.Forms
{
    public partial class ClientsForm : Form
    {
        private readonly IDatabaseService _db;
        private DataGridView grid;

        public ClientsForm(IDatabaseService dbService)
        {
            _db = dbService;
            this.Text = "Klijenti";
            this.Width = 1400; // povecano
            this.Height = 600; // povecano

            // Panel za dugmad i pretragu, sada viši da grid ne bude zalepljen gore
            var panel = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 60,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(0, 10, 0, 10)
            };

            var btnAdd = new Button
            {
                Text = "Dodaj klijenta",
                Width = 160,
                Height = 40,
                Font = new Font("Segoe UI", 11, FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleCenter,
                Margin = new Padding(10, 10, 10, 10)
            };
            btnAdd.Click += (s, e) => DodajKlijenta();
            panel.Controls.Add(btnAdd);

            var btnEdit = new Button
            {
                Text = "Izmeni klijenta",
                Width = 160,
                Height = 40,
                Font = new Font("Segoe UI", 11, FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleCenter,
                Margin = new Padding(10, 10, 10, 10)
            };
            btnEdit.Click += (s, e) => IzmeniKlijenta();
            panel.Controls.Add(btnEdit);

            var txtPretraga = new TextBox
            {
                PlaceholderText = "Pretraga...",
                Width = 260,
                Height = 38,
                Font = new Font("Segoe UI", 11, FontStyle.Regular),
                Margin = new Padding(25, 12, 10, 10)
            };
            txtPretraga.TextChanged += (s, e) =>
            {
                var svi = _db.GetAllClients();
                var filter = txtPretraga.Text.ToLower();
                grid.DataSource = svi.Where(x =>
                    x.FirstName.ToLower().Contains(filter) ||
                    x.LastName.ToLower().Contains(filter) ||
                    x.PassportNumber.ToLower().Contains(filter)
                ).ToList();
                AutoSizeGrid();
            };
            panel.Controls.Add(txtPretraga);

            grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AutoGenerateColumns = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                Font = new Font("Segoe UI", 11, FontStyle.Regular),
                RowTemplate = { Height = 35 },
                AllowUserToAddRows = false
            };
            this.Controls.Add(grid);

            this.Controls.Add(panel);

            grid.CellDoubleClick += (s, e) => PrikaziRezervacijeZaKlijenta();

            LoadClients();
        }

        private void LoadClients()
        {
            grid.DataSource = null;
            grid.DataSource = _db.GetAllClients().ToList();

            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            grid.AutoResizeColumns();

            // FONT za ZAGLAVLJA kolona
            var headerStyle = new DataGridViewCellStyle
            {
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Alignment = DataGridViewContentAlignment.MiddleCenter,
                BackColor = Color.FromArgb(245, 245, 245),
                ForeColor = Color.Black
            };
            grid.ColumnHeadersDefaultCellStyle = headerStyle;

            // (opciono) veća visina zaglavlja:
            grid.ColumnHeadersHeight = 40;
        }

        private void AutoSizeGrid()
        {
            // Lepo razvuci sve kolone da se sve vidi
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            grid.AutoResizeColumns();
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12, FontStyle.Bold);
        }

        private void DodajKlijenta()
        {
            var f = new Form
            {
                Text = "Novi klijent",
                Width = 450,
                Height = 520,
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            int labelLeft = 30, txtLeft = 170;
            int labelWidth = 120, txtWidth = 220, height = 35, gap = 17;

            var lblIme = new Label { Text = "Ime:", Top = 40, Left = labelLeft, Width = labelWidth, Height = height, Font = new Font("Segoe UI", 12, FontStyle.Regular) };
            var txtIme = new TextBox { PlaceholderText = "Ime", Top = 40, Left = txtLeft, Width = txtWidth, Height = height, Font = new Font("Segoe UI", 12, FontStyle.Regular) };

            var lblPrezime = new Label { Text = "Prezime:", Top = 40 + height + gap, Left = labelLeft, Width = labelWidth, Height = height, Font = new Font("Segoe UI", 12, FontStyle.Regular) };
            var txtPrezime = new TextBox { PlaceholderText = "Prezime", Top = 40 + height + gap, Left = txtLeft, Width = txtWidth, Height = height, Font = new Font("Segoe UI", 12, FontStyle.Regular) };

            var lblPass = new Label { Text = "Broj pasoša:", Top = 40 + 2 * (height + gap), Left = labelLeft, Width = labelWidth, Height = height, Font = new Font("Segoe UI", 12, FontStyle.Regular) };
            var txtPass = new TextBox { PlaceholderText = "Broj pasoša", Top = 40 + 2 * (height + gap), Left = txtLeft, Width = txtWidth, Height = height, Font = new Font("Segoe UI", 12, FontStyle.Regular) };

            var lblEmail = new Label { Text = "Email:", Top = 40 + 3 * (height + gap), Left = labelLeft, Width = labelWidth, Height = height, Font = new Font("Segoe UI", 12, FontStyle.Regular) };
            var txtEmail = new TextBox { PlaceholderText = "Email", Top = 40 + 3 * (height + gap), Left = txtLeft, Width = txtWidth, Height = height, Font = new Font("Segoe UI", 12, FontStyle.Regular) };

            var lblTel = new Label { Text = "Telefon:", Top = 40 + 4 * (height + gap), Left = labelLeft, Width = labelWidth, Height = height, Font = new Font("Segoe UI", 12, FontStyle.Regular) };
            var txtTel = new TextBox { PlaceholderText = "Telefon", Top = 40 + 4 * (height + gap), Left = txtLeft, Width = txtWidth, Height = height, Font = new Font("Segoe UI", 12, FontStyle.Regular) };

            var lblDate = new Label { Text = "Datum rođenja:", Top = 40 + 5 * (height + gap), Left = labelLeft, Width = labelWidth, Height = height, Font = new Font("Segoe UI", 12, FontStyle.Regular) };
            var dtp = new DateTimePicker { Top = 40 + 5 * (height + gap), Left = txtLeft, Width = txtWidth, Height = height, Font = new Font("Segoe UI", 12, FontStyle.Regular) };

            var btnSave = new Button
            {
                Text = "Sačuvaj",
                Top = 40 + 6 * (height + gap) + 20,
                Left = txtLeft,
                Width = 150,
                Height = 45,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };

            btnSave.Click += (ss, ee) =>
            {
                var c = new Client
                {
                    FirstName = txtIme.Text,
                    LastName = txtPrezime.Text,
                    PassportNumber = txtPass.Text,
                    BirthDate = dtp.Value,
                    Email = txtEmail.Text,
                    Phone = txtTel.Text
                };
                _db.AddClient(c);
                f.Close();
                LoadClients();
            };

            f.Controls.AddRange(new Control[] { lblIme, txtIme, lblPrezime, txtPrezime, lblPass, txtPass, lblEmail, txtEmail, lblTel, txtTel, lblDate, dtp, btnSave });
            f.ShowDialog();
        }

        private void IzmeniKlijenta()
        {
            if (grid.SelectedRows.Count == 0) return;
            var client = grid.SelectedRows[0].DataBoundItem as Client;
            if (client == null) return;

            var f = new Form
            {
                Text = "Izmeni klijenta",
                Width = 450,
                Height = 520,
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            int labelLeft = 30, txtLeft = 170;
            int labelWidth = 120, txtWidth = 220, height = 35, gap = 17;

            var lblIme = new Label { Text = "Ime:", Top = 40, Left = labelLeft, Width = labelWidth, Height = height, Font = new Font("Segoe UI", 12, FontStyle.Regular) };
            var txtIme = new TextBox { Text = client.FirstName, Top = 40, Left = txtLeft, Width = txtWidth, Height = height, Font = new Font("Segoe UI", 12, FontStyle.Regular) };

            var lblPrezime = new Label { Text = "Prezime:", Top = 40 + height + gap, Left = labelLeft, Width = labelWidth, Height = height, Font = new Font("Segoe UI", 12, FontStyle.Regular) };
            var txtPrezime = new TextBox { Text = client.LastName, Top = 40 + height + gap, Left = txtLeft, Width = txtWidth, Height = height, Font = new Font("Segoe UI", 12, FontStyle.Regular) };

            var lblPass = new Label { Text = "Broj pasoša:", Top = 40 + 2 * (height + gap), Left = labelLeft, Width = labelWidth, Height = height, Font = new Font("Segoe UI", 12, FontStyle.Regular) };
            var txtPass = new TextBox { Text = client.PassportNumber, Top = 40 + 2 * (height + gap), Left = txtLeft, Width = txtWidth, Height = height, Font = new Font("Segoe UI", 12, FontStyle.Regular) };

            var lblEmail = new Label { Text = "Email:", Top = 40 + 3 * (height + gap), Left = labelLeft, Width = labelWidth, Height = height, Font = new Font("Segoe UI", 12, FontStyle.Regular) };
            var txtEmail = new TextBox { Text = client.Email, Top = 40 + 3 * (height + gap), Left = txtLeft, Width = txtWidth, Height = height, Font = new Font("Segoe UI", 12, FontStyle.Regular) };

            var lblTel = new Label { Text = "Telefon:", Top = 40 + 4 * (height + gap), Left = labelLeft, Width = labelWidth, Height = height, Font = new Font("Segoe UI", 12, FontStyle.Regular) };
            var txtTel = new TextBox { Text = client.Phone, Top = 40 + 4 * (height + gap), Left = txtLeft, Width = txtWidth, Height = height, Font = new Font("Segoe UI", 12, FontStyle.Regular) };

            var lblDate = new Label { Text = "Datum rođenja:", Top = 40 + 5 * (height + gap), Left = labelLeft, Width = labelWidth, Height = height, Font = new Font("Segoe UI", 12, FontStyle.Regular) };
            var dtp = new DateTimePicker { Value = client.BirthDate, Top = 40 + 5 * (height + gap), Left = txtLeft, Width = txtWidth, Height = height, Font = new Font("Segoe UI", 12, FontStyle.Regular) };

            var btnSave = new Button
            {
                Text = "Sačuvaj",
                Top = 40 + 6 * (height + gap) + 20,
                Left = txtLeft,
                Width = 150,
                Height = 45,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };

            btnSave.Click += (ss, ee) =>
            {
                client.FirstName = txtIme.Text;
                client.LastName = txtPrezime.Text;
                client.PassportNumber = txtPass.Text;
                client.BirthDate = dtp.Value;
                client.Email = txtEmail.Text;
                client.Phone = txtTel.Text;
                _db.UpdateClient(client);
                f.Close();
                LoadClients();
            };

            f.Controls.AddRange(new Control[] { lblIme, txtIme, lblPrezime, txtPrezime, lblPass, txtPass, lblEmail, txtEmail, lblTel, txtTel, lblDate, dtp, btnSave });
            f.ShowDialog();
        }

        private void PrikaziRezervacijeZaKlijenta()
        {
            if (grid.SelectedRows.Count == 0) return;
            var client = grid.SelectedRows[0].DataBoundItem as Client;
            if (client == null) return;
            var form = new Form { Text = $"Rezervacije za {client.FirstName} {client.LastName}", Width = 700, Height = 400 };
            var gridRez = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AutoGenerateColumns = true,
                Font = new Font("Segoe UI", 11, FontStyle.Regular)
            };
            gridRez.DataSource = _db.GetReservationsByClient(client.Id);
            gridRez.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            gridRez.AutoResizeColumns();
            gridRez.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            form.Controls.Add(gridRez);
            form.ShowDialog();
        }
    }
}
