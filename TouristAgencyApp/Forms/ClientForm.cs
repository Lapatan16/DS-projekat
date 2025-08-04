using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouristAgencyApp.Models;
using TouristAgencyApp.Patterns;
using TouristAgencyApp.Services;

namespace TouristAgencyApp.Forms
{
    public partial class ClientsForm : Form
    {
        private readonly IDatabaseService _db;
        private DataGridView grid;
        private readonly ClientSubject _clientSubject;
        private readonly ClientManager _clientManager;
        private Button btnUndo;
        private Button btnRedo;
        public ClientsForm(IDatabaseService dbService)
        {
            _clientManager = new ClientManager(dbService);
            _clientSubject = new ClientSubject();
            _clientSubject.Attach(new ClientLogger());
            _clientSubject.Attach(new ClientNotifier());

            _db = dbService;
            InitializeForm();
            CreateModernUI();
            LoadClients();
        }

        private void InitializeForm()
        {
            this.Text = "👥 Upravljanje klijentima";
            this.Width = 1400;
            this.Height = 700;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(248, 249, 250);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
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

            btnUndo = CreateModernButton(" Opozovi", Color.FromArgb(255, 255, 165, 0));
            btnUndo.Location = new Point(880, 15);
            btnUndo.TextAlign = ContentAlignment.MiddleCenter;
            btnUndo.Click += (s, e) => OpozoviAkciju();
            btnUndo.Width = 100;
            // btnUndo.Visible = false;

            btnRedo = CreateModernButton(" Nazovi", Color.FromArgb(255, 255, 165, 0));
            btnRedo.Location = new Point(980, 15);
            btnRedo.TextAlign = ContentAlignment.MiddleCenter;
            btnRedo.Click += (s, e) => NapredAkcija();
            btnRedo.Width = 100;
            // btnRedo.Visible = false;

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
                BorderStyle = BorderStyle.FixedSingle,
             
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

            toolbarPanel.Controls.AddRange(new Control[] { btnAdd, btnEdit, txtPretraga, btnUndo,btnRedo });

            var contentPanel = new Panel
            {
            Dock = DockStyle.Fill,
            BackColor = Color.White
            };

           
            grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AutoGenerateColumns = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                Font = new Font("Segoe UI", 11, FontStyle.Regular),
                RowTemplate = { Height = 40 },
                AllowUserToAddRows = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                GridColor = Color.FromArgb(224, 224, 224),
                EnableHeadersVisualStyles = false
            };

            grid.DefaultCellStyle.BackColor = Color.White;
            grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(52, 152, 219);
            grid.DefaultCellStyle.SelectionForeColor = Color.White;
            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250);

            var headerStyle = new DataGridViewCellStyle
            {
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Alignment = DataGridViewContentAlignment.MiddleCenter,
                BackColor = Color.FromArgb(52, 73, 94),
                ForeColor = Color.White
            };
            grid.ColumnHeadersDefaultCellStyle = headerStyle;
            grid.ColumnHeadersHeight = 45;

            contentPanel.Controls.Add(grid);
            grid.CellDoubleClick += (s, e) => PrikaziRezervacijeZaKlijenta();

            var statusBar = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 30,
                BackColor = Color.FromArgb(52, 73, 94)
            };

            var lblStatus = new Label
            {
                Text = "Spreman za rad",
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                ForeColor = Color.White,
                AutoSize = false,
                Width = 1400,
                Height = 30,
                Top = 0,
                Left = 0,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(10, 0, 0, 0)
            };
            statusBar.Controls.Add(lblStatus);

            this.Controls.Add(statusBar);
            this.Controls.Add(contentPanel);
            this.Controls.Add(toolbarPanel);
            this.Controls.Add(headerPanel);
        }

        private Button CreateModernButton(string text, Color baseColor)
        {
            var button = new Button
            {
                Text = text,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = baseColor,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 },
                Width = 160,
                Height = 40,
                Cursor = Cursors.Hand
            };

            button.MouseEnter += (s, e) => {
                button.BackColor = ControlPaint.Light(baseColor);
            };

            button.MouseLeave += (s, e) => {
                button.BackColor = baseColor;
            };

            return button;
        }
        private void OpozoviAkciju()
        {
            _clientManager.UndoLastAction();
            LoadClients();
        }

        private void NapredAkcija()
        {
            _clientManager.RedoLastAction();
            LoadClients();
        }
        private void LoadClients()
        {
            grid.DataSource = null;
            grid.DataSource = _db.GetAllClients().ToList();

            if (grid.Columns.Contains("Id"))
                grid.Columns["Id"].Visible = false;
        
            if (grid.Columns.Contains("FullName"))
                grid.Columns["FullName"].Visible = false;

            if (grid.Columns.Contains("FirstName"))
                grid.Columns["FirstName"].HeaderText = "Ime";
            if (grid.Columns.Contains("LastName"))
                grid.Columns["LastName"].HeaderText = "Prezime";
            if (grid.Columns.Contains("PassportNumber"))
                grid.Columns["PassportNumber"].HeaderText = "Broj pasoša";
            if (grid.Columns.Contains("BirthDate"))
                grid.Columns["BirthDate"].HeaderText = "Datum rođenja";
            if (grid.Columns.Contains("Email"))
                grid.Columns["Email"].HeaderText = "Email";
            if (grid.Columns.Contains("Phone"))
                grid.Columns["Phone"].HeaderText = "Telefon";

            AutoSizeGrid();
        }

        private void AutoSizeGrid()
        {
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            grid.AutoResizeColumns();
        }

        private void DodajKlijenta()
        {
            var f = new Form
            {
                Text = "➕ Novi klijent",
                Width = 500,
                Height = 550,
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = Color.FromArgb(248, 249, 250)
            };

            
            var headerLabel = new Label
            {
                Text = "Dodavanje novog klijenta",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94),
                AutoSize = false,
                Width = 500,
                Height = 40,
                Top = 20,
                Left = 0,
                TextAlign = ContentAlignment.MiddleCenter
            };
            f.Controls.Add(headerLabel);

            int labelLeft = 30, txtLeft = 200;
            int labelWidth = 150, txtWidth = 250, height = 35, gap = 20;
            int startTop = 80;

            var lblIme = new Label { Text = "Ime:", Top = startTop, Left = labelLeft, Width = labelWidth, Height = height, Font = new Font("Segoe UI", 12, FontStyle.Regular) };
            var txtIme = new TextBox { PlaceholderText = "Unesite ime", Top = startTop, Left = txtLeft, Width = txtWidth, Height = height, Font = new Font("Segoe UI", 12, FontStyle.Regular) };

            var lblPrezime = new Label { Text = "Prezime:", Top = startTop + height + gap, Left = labelLeft, Width = labelWidth, Height = height, Font = new Font("Segoe UI", 12, FontStyle.Regular) };
            var txtPrezime = new TextBox { PlaceholderText = "Unesite prezime", Top = startTop + height + gap, Left = txtLeft, Width = txtWidth, Height = height, Font = new Font("Segoe UI", 12, FontStyle.Regular) };

            var lblPass = new Label { Text = "Broj pasoša:", Top = startTop + 2 * (height + gap), Left = labelLeft, Width = labelWidth, Height = height, Font = new Font("Segoe UI", 12, FontStyle.Regular) };
            var txtPass = new TextBox { PlaceholderText = "Unesite broj pasoša", Top = startTop + 2 * (height + gap), Left = txtLeft, Width = txtWidth, Height = height, Font = new Font("Segoe UI", 12, FontStyle.Regular) };

            var lblEmail = new Label { Text = "Email:", Top = startTop + 3 * (height + gap), Left = labelLeft, Width = labelWidth, Height = height, Font = new Font("Segoe UI", 12, FontStyle.Regular) };
            var txtEmail = new TextBox { PlaceholderText = "Unesite email", Top = startTop + 3 * (height + gap), Left = txtLeft, Width = txtWidth, Height = height, Font = new Font("Segoe UI", 12, FontStyle.Regular) };

            var lblTel = new Label { Text = "Telefon:", Top = startTop + 4 * (height + gap), Left = labelLeft, Width = labelWidth, Height = height, Font = new Font("Segoe UI", 12, FontStyle.Regular) };
            var txtTel = new TextBox { PlaceholderText = "Unesite telefon", Top = startTop + 4 * (height + gap), Left = txtLeft, Width = txtWidth, Height = height, Font = new Font("Segoe UI", 12, FontStyle.Regular) };

            var lblDate = new Label { Text = "Datum rođenja:", Top = startTop + 5 * (height + gap), Left = labelLeft, Width = labelWidth, Height = height, Font = new Font("Segoe UI", 12, FontStyle.Regular) };
            var dtp = new DateTimePicker { Top = startTop + 5 * (height + gap), Left = txtLeft, Width = txtWidth, Height = height, Font = new Font("Segoe UI", 12, FontStyle.Regular) };

            var btnSave = new Button
            {
                Text = "💾 Sačuvaj",
                Top = startTop + 6 * (height + gap) + 20,
                Left = txtLeft,
                Width = 150,
                Height = 45,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 },
                Cursor = Cursors.Hand
            };

            btnSave.Click += (ss, ee) =>
            {
                if (string.IsNullOrWhiteSpace(txtIme.Text) || string.IsNullOrWhiteSpace(txtPrezime.Text))
                {
                    MessageBox.Show("Ime i prezime su obavezni!", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var c = new Client
                {
                    FirstName = txtIme.Text.Trim(),
                    LastName = txtPrezime.Text.Trim(),
                    PassportNumber = txtPass.Text.Trim(),
                    BirthDate = dtp.Value,
                    Email = txtEmail.Text.Trim(),
                    Phone = txtTel.Text.Trim()
                };
                
                try
                {
                    int id = _clientManager.AddClient(c);
                    _clientSubject.AddClient(c, id);
                    f.Close();
                    LoadClients();
                    MessageBox.Show("Klijent uspešno dodat!", "Uspeh", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Greška pri dodavanju klijenta: {ex.Message}", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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
                _clientManager.updateClient(client);
                _clientSubject.UpdateClient(client);

                f.Close();
                LoadClients();
            };

            f.Controls.AddRange(new Control[] { lblIme, txtIme, lblPrezime, txtPrezime, lblPass, txtPass, lblEmail, txtEmail, lblTel, txtTel, lblDate, dtp, btnSave });
            f.ShowDialog();
        }

        private void PrikaziRezervacijeZaKlijenta()
        {
            var paketi = _db.GetAllPackages();

            if (grid.SelectedRows.Count == 0) return;

            var client = grid.SelectedRows[0].DataBoundItem as Client;

            if (client == null) return;

            var form = new Form
            {
                Text = $"Rezervacije za {client.FirstName} {client.LastName}",
                Width = 1000, 
                Height = 500,
                StartPosition = FormStartPosition.CenterParent,
                BackColor = Color.White,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false
            };

            var headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.FromArgb(42, 204, 113)
            };

            var lblHeader = new Label
            {
                Text = "📋 Spisak rezervacija korisnika",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = false,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };
            headerPanel.Controls.Add(lblHeader);

            var gridRez = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AutoGenerateColumns = true,
                Font = new Font("Segoe UI", 11, FontStyle.Regular),
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                GridColor = Color.LightGray,
                EnableHeadersVisualStyles = false,
                RowTemplate = { Height = 35 },
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AllowUserToAddRows = false
            };

            gridRez.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(52, 73, 94),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Alignment = DataGridViewContentAlignment.MiddleCenter
            };
            gridRez.ColumnHeadersHeight = 40;

            gridRez.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250);
            gridRez.DefaultCellStyle.SelectionBackColor = Color.FromArgb(52, 152, 219);
            gridRez.DefaultCellStyle.SelectionForeColor = Color.White;

            var rezervacije = _db.GetReservationsByClient(client.Id);

            var data = rezervacije.Select(r => new
            {
                NazivPaketa = paketi.FirstOrDefault(p => p.Id == r.PackageId)?.Name ?? "(nepoznato)",
                DatumRezervacije = r.ReservationDate.ToString("dd.MM.yyyy"),
                BrojOsoba = r.NumPersons,
                DodatneUsluge = r.ExtraServices != null ? string.Join(", ", r.ExtraServices) : ""
            }).ToList();

            gridRez.DataSource = data;

                foreach (DataGridViewColumn col in gridRez.Columns)
            {
                col.HeaderText = col.Name switch
                {
                    "NazivPaketa" => "Naziv paketa",
                    "DatumRezervacije" => "Datum rezervacije",
                    "BrojOsoba" => "Broj osoba",
                    "DodatneUsluge" => "Dodatne usluge",
                    _ => col.HeaderText
                };
            }
            gridRez.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            gridRez.AutoResizeColumns();

            form.Controls.Add(gridRez);
            form.Controls.Add(headerPanel);

            form.ShowDialog();
        }
    }
}
