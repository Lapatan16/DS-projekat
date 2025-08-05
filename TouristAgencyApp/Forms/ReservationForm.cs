using TouristAgencyApp.Models;
using TouristAgencyApp.Patterns;
using TouristAgencyApp.Patterns.Observer.ReservationObserver;
using TouristAgencyApp.Services;

namespace TouristAgencyApp.Forms
{
    public partial class ReservationsForm : Form
    {
        private readonly IDatabaseService _db;
        private readonly ReservationSubject _reservationSubject;
        private ReservationManager _reservationManager;
        private DataGridView grid;
        private ComboBox cbClients;
        private Button btnUndo;
        private Button btnRedo;
        private Button btnAdd;
        private Button btnRemove;
        private Button btnEdit;
        public ReservationsForm(IDatabaseService dbService)
        {
            _db = dbService;
            _reservationSubject = new ReservationSubject();
            _reservationManager = new ReservationManager(dbService);

            _reservationSubject.Attach(new ReservationLogger());
            _reservationSubject.Attach(new ReservationNotifier());
            _reservationSubject.Attach(new ReservationStatistics());

            InitializeForm();
            CreateModernUI();
            LoadClients();
        }

        private void InitializeForm()
        {
            this.Text = "🛎️ Upravljanje rezervacijama";
            this.Width = 1100;
            this.Height = 600;
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
                BackColor = Color.FromArgb(155, 89, 182)
            };

            var lblTitle = new Label
            {
                Text = "Upravljanje rezervacijama",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = false,
                Width = this.Width,
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


            cbClients = new ComboBox
            {
                Width = 300,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 11, FontStyle.Regular),
                Location = new Point(20, 20)
            };
            cbClients.SelectedIndexChanged += (s, e) => LoadReservations();


            btnAdd = CreateModernButton("➕ Nova rezervacija", Color.FromArgb(46, 204, 113));
            btnAdd.Location = new Point(340, 15);
            btnAdd.Click += (s, e) => DodajRezervaciju();


            btnRemove = CreateModernButton("✕ Otkaži rezervaciju", Color.FromArgb(231, 76, 60));
            btnRemove.Location = new Point(520, 15);
            btnRemove.TextAlign = ContentAlignment.MiddleCenter;

            btnRemove.Click += (s, e) => OtkaziRezervaciju();

            btnEdit = CreateModernButton(" Azuriraj rezervaciju", Color.FromArgb(255, 0, 0, 255));
            btnEdit.Location = new Point(700, 15);
            btnEdit.TextAlign = ContentAlignment.MiddleCenter;
            btnEdit.Click += (s, e) => AzurirajRezervaciju();

            btnUndo = CreateModernButton("↩️ Undo", Color.FromArgb(255, 255, 165, 0));
            btnUndo.Location = new Point(880, 15);
            btnUndo.TextAlign = ContentAlignment.MiddleCenter;
            btnUndo.Click += (s, e) => OpozoviAkciju();
            btnUndo.Width = 100;

            btnRedo = CreateModernButton("Redo ↪️", Color.FromArgb(255, 255, 165, 0));
            btnRedo.Location = new Point(990, 15);
            btnRedo.TextAlign = ContentAlignment.MiddleCenter;
            btnRedo.Click += (s, e) => NazoviAkciju();
            btnRedo.Width = 100;
  
            toolbarPanel.Controls.AddRange(new Control[] { cbClients, btnAdd, btnRemove, btnEdit, btnUndo, btnRedo });


            var contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };


            grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AutoGenerateColumns = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                Font = new Font("Segoe UI", 11, FontStyle.Regular),
                RowTemplate = { Height = 35 },
                AllowUserToAddRows = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                GridColor = Color.FromArgb(224, 224, 224),
                EnableHeadersVisualStyles = false
            };



            grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "PackageName",
                HeaderText = "Paket",
                Width = 200
            });
            grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "NumPersons",
                HeaderText = "Broj osoba",
                Width = 120
            });
            grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ReservationDate",
                HeaderText = "Datum rezervacije",
                Width = 160
            });
            grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ExtraServices",
                HeaderText = "Dodatne usluge",
                Width = 200
            });
            grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Id",
                HeaderText = "Id",
                Width = 0,
                Visible = false,
            });

            var headerStyle = new DataGridViewCellStyle
            {
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Alignment = DataGridViewContentAlignment.MiddleCenter,
                BackColor = Color.FromArgb(52, 73, 94),
                ForeColor = Color.White
            };
            grid.ColumnHeadersDefaultCellStyle = headerStyle;
            grid.ColumnHeadersHeight = 45;

            grid.DefaultCellStyle.BackColor = Color.White;
            grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(52, 152, 219);
            grid.DefaultCellStyle.SelectionForeColor = Color.White;
            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250);

            contentPanel.Controls.Add(grid);


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
                Width = this.Width,
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
                Cursor = Cursors.Hand,
                TextAlign = ContentAlignment.MiddleCenter
            };

            button.MouseEnter += (s, e) =>
            {
                button.BackColor = ControlPaint.Light(baseColor);
            };

            button.MouseLeave += (s, e) =>
            {
                button.BackColor = baseColor;
            };

            return button;
        }

        private void LoadClients()
        {
            var clientsList = _db.GetAllClients();
            cbClients.DataSource = clientsList;
            cbClients.DisplayMember = "FullName";
            cbClients.ValueMember = "Id";

            if (cbClients.Items.Count > 0)
                cbClients.SelectedIndex = 0;
            LoadReservations();
        }

        private void LoadReservations()
        {
            if (cbClients.SelectedItem is Client c)
            {
                var reservations = _db.GetReservationsByClient(c.Id).ToList();

                var packages = _db.GetAllPackages();
                foreach (var r in reservations)
                {
                    var pkg = packages.FirstOrDefault(p => p.Id == r.PackageId);
                    r.PackageName = pkg != null ? pkg.Name : "(nepoznato)";
                }

                grid.DataSource = null;
                grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                grid.DataSource = reservations;
                grid.ClearSelection();
                grid.AutoResizeColumns();
            }
        }
        private void OpozoviAkciju()
        {
            _reservationManager.UndoLastAction();
            LoadReservations();
        }

        private void NazoviAkciju()
        {
            _reservationManager.RedoLastAction();
            LoadReservations();
        }
        private void DodajRezervaciju()
        {
            if (!(cbClients.SelectedItem is Client c)) return;

            var f = new Form
            {
                Text = "Nova rezervacija",
                Width = 420,
                Height = 350,
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = Color.FromArgb(248, 249, 250)
            };

            var lblPackages = new Label { Text = "Paket:", Left = 20, Top = 20, Width = 180, Font = new Font("Segoe UI", 11) };
            var cbPackages = new ComboBox
            {
                Left = 20,
                Top = 50,
                Width = 360,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 11)
            };
            var packages = _db.GetAllPackages();
            cbPackages.DataSource = packages;
            cbPackages.DisplayMember = "Name";

            var lblPersons = new Label { Text = "Broj osoba:", Left = 20, Top = 95, Width = 180, Font = new Font("Segoe UI", 11) };
            var numPersons = new NumericUpDown { Left = 20, Top = 125, Width = 120, Minimum = 1, Maximum = 30, Value = 1, Font = new Font("Segoe UI", 11) };

            var lblExtra = new Label { Text = "Dodatne usluge:", Left = 20, Top = 165, Width = 180, Font = new Font("Segoe UI", 11) };
            var txtExtra = new TextBox { Left = 20, Top = 195, Width = 360, Font = new Font("Segoe UI", 11) };

            var btnSave = new Button
            {
                Text = "Sačuvaj",
                Left = 20,
                Top = 240,
                Width = 150,
                Height = 40,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };

            btnSave.FlatAppearance.BorderSize = 0;

            btnSave.Click += (ss, ee) =>
            {
                if (cbPackages.SelectedItem is TravelPackage pkg)
                {

                    Reservation reservation = new ReservationBuilder()
                       .SetClient(c)
                       .SetPackage(pkg)
                       .SetNumPersons((int)numPersons.Value)
                       .SetExtraServices(txtExtra.Text)
                       .SetReservationDate(DateTime.Now)
                       .Build();
              
                    int id = _reservationManager.AddReservation(reservation);
                    _reservationSubject.AddReservation(reservation, id);
                    btnUndo.Visible = true;
                    f.Close();
                    LoadReservations();
                }
            };

            f.Controls.AddRange(new Control[] { lblPackages, cbPackages, lblPersons, numPersons, lblExtra, txtExtra, btnSave });
            f.ShowDialog();
        }
        private void AzurirajRezervaciju()
        {
            if (grid.SelectedRows.Count == 0)
            {
                MessageBox.Show("Prvo izaberi rezervaciju za azuriranje!", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var f = new Form
            {
                Text = "Azuriraj rezervaciju",
                Width = 420,
                Height = 350,
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = Color.FromArgb(248, 249, 250)
            };

            var lblPackages = new Label { Text = "Paket: " + grid.SelectedRows[0].Cells[0].Value.ToString(), Left = 20, Top = 20, Width = 180, Font = new Font("Segoe UI", 11) };

            var packages = _db.GetAllPackages();



            var lblPersons = new Label { Text = "Broj osoba:", Left = 20, Top = 95, Width = 180, Font = new Font("Segoe UI", 11) };
            var numPersons = new NumericUpDown { Left = 20, Top = 125, Width = 120, Minimum = 1, Maximum = 30, Value = 1, Font = new Font("Segoe UI", 11) };
            numPersons.Value = Convert.ToDecimal(grid.SelectedRows[0].Cells[1].Value);
            var lblExtra = new Label { Text = "Dodatne usluge:", Left = 20, Top = 165, Width = 180, Font = new Font("Segoe UI", 11) };
            var txtExtra = new TextBox { Text = grid.SelectedRows[0].Cells[3].Value.ToString(), Left = 20, Top = 195, Width = 360, Font = new Font("Segoe UI", 11) };

            var btnSave = new Button
            {
                Text = "Sačuvaj",
                Left = 20,
                Top = 240,
                Width = 150,
                Height = 40,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };

            btnSave.FlatAppearance.BorderSize = 0;

            btnSave.Click += (ss, ee) =>
            {
                int reservationId = Convert.ToInt32(grid.SelectedRows[0].Cells[4].Value);
                _reservationManager.UpdateReservation(reservationId,(int) numPersons.Value, txtExtra.Text);
                _reservationSubject.UpdateReservation(reservationId);

                btnUndo.Visible = true;
                LoadReservations();
                f.Close();
            };
            f.Controls.AddRange(new Control[] { lblPackages, lblPersons, numPersons, lblExtra, txtExtra, btnSave });
            f.ShowDialog();


        }
        private void OtkaziRezervaciju()
        {
            if (grid.SelectedRows.Count == 0)
            {
                MessageBox.Show("Prvo izaberi rezervaciju za otkazivanje!", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var id = Convert.ToInt32(grid.SelectedRows[0].Cells[4].Value);
            var confirm = MessageBox.Show("Da li ste sigurni da želite da otkažete ovu rezervaciju?", "Potvrda", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                _reservationManager.RemoveReservation(id);
                _reservationSubject.RemoveReservation(id);
                btnUndo.Visible = true;
                LoadReservations();
            }
        }
    }
}