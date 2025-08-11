using Org.BouncyCastle.Bcpg.OpenPgp;
using System.Linq;
using TouristAgencyApp.Models;
using TouristAgencyApp.Patterns;
using TouristAgencyApp.Patterns.Observer.ReservationObserver;
using TouristAgencyApp.Services;

namespace TouristAgencyApp.Forms
{
    public partial class ReservationsForm : Form
    {
        private readonly ReservationFacade _reservationFacade;
        private DataGridView grid;
        private ComboBox cbClients;
        private Button btnUndo;
        private Button btnRedo;
        private Button btnAdd;
        private Button btnRemove;
        private Button btnEdit;
        public ReservationsForm(IDatabaseService dbService)
        {
            _reservationFacade = new ReservationFacade(dbService);

            InitializeForm();
            CreateModernUI();
            LoadClients();
        }

        private void InitializeForm()
        {
            this.Text = "🛎️ Upravljanje rezervacijama";
            this.Width = 1260;
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
            btnUndo.Width = 160;

            btnRedo = CreateModernButton("Redo ↪️", Color.FromArgb(255, 255, 165, 0));
            btnRedo.Location = new Point(1060, 15);
            btnRedo.TextAlign = ContentAlignment.MiddleCenter;
            btnRedo.Click += (s, e) => NazoviAkciju();
            btnRedo.Width = 160;
  
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
                DataPropertyName = "Id",
                HeaderText = "Id",
                Width = 0,
                Visible = false,
            });
            grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Destination",
                HeaderText = "Destinacija",
                Width = 160
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
            var clientsList = _reservationFacade.GetAllClients();
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
                var reservations = _reservationFacade.GetReservationsByClient(c.Id);

                grid.DataSource = null;
                grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                grid.DataSource = reservations;
                grid.ClearSelection();
                grid.AutoResizeColumns();
            }
        }
        private void OpozoviAkciju()
        {
            _reservationFacade.Undo();
            LoadReservations();
        }

        private void NazoviAkciju()
        {
            _reservationFacade.Redo();
            LoadReservations();
        }
        private void DodajRezervaciju()
        {
            if (!(cbClients.SelectedItem is Client c)) return;

            var allPackages = _reservationFacade.GetAllPackages();

            if (allPackages.Count == 0)
            {
                MessageBox.Show("Nema dostupnih paketa.", "Upozorenje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var f = new Form
            {
                Text = "Nova rezervacija",
                Width = 420,
                Height = 420,
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = Color.FromArgb(248, 249, 250)
            };

            // Destination selector
            var lblDestinations = new Label { Text = "Destinacija:", Left = 20, Top = 20, Width = 180, Font = new Font("Segoe UI", 11) };
            var cbDestinations = new ComboBox
            {
                Left = 20,
                Top = 50,
                Width = 360,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 11)
            };
            
            
            var uniqueDestinations = allPackages
                .Select(p => p.Destination)
                .Where(d => !string.IsNullOrWhiteSpace(d))
                .Distinct()
                .ToList();
            foreach (var pk in allPackages)
            {
                if (pk is CruisePackage)
                {
                    string temp = ((CruisePackage)pk).Route.Split(',').Last();
                    temp.Trim();
                    if (!uniqueDestinations.Contains(temp)) uniqueDestinations.Add(temp);
                }
            }

            cbDestinations.DataSource = uniqueDestinations;

            // Packages dropdown (filtered by selected destination)
            var lblPackages = new Label { Text = "Paket:", Left = 20, Top = 95, Width = 180, Font = new Font("Segoe UI", 11) };
            var cbPackages = new ComboBox
            {
                Left = 20,
                Top = 125,
                Width = 360,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 11)
            };

            void UpdatePackageDropdown()
            {
                if (cbDestinations.SelectedItem is not string selectedDestination) return;

                var filteredPackages = allPackages
                    .Where(p => p.Destination == selectedDestination)
                    .ToList();
                foreach(var pk in allPackages)
                {
                    if(pk is CruisePackage)
                    {
                        string temp = ((CruisePackage)pk).Route.Split(',').Last();
                        temp.Trim();
                        if ( temp == selectedDestination) filteredPackages.Add(pk);

                    }
                }
                cbPackages.DataSource = filteredPackages;
                cbPackages.DisplayMember = "Name";
            }

            cbDestinations.SelectedIndexChanged += (s, e) => UpdatePackageDropdown();

            var lblPersons = new Label { Text = "Broj osoba:", Left = 20, Top = 170, Width = 180, Font = new Font("Segoe UI", 11) };
            var numPersons = new NumericUpDown { Left = 20, Top = 200, Width = 120, Minimum = 1, Maximum = 30, Value = 1, Font = new Font("Segoe UI", 11) };


            var btnSave = new Button
            {
                Text = "Sačuvaj",
                Left = 20,
                Top = 315,
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
                    if (cbDestinations.SelectedItem is not string selectedDestination || string.IsNullOrWhiteSpace(selectedDestination))
                    {
                        MessageBox.Show("Molimo odaberite destinaciju.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (numPersons.Value < 1 || numPersons.Value > numPersons.Maximum)
                    {
                        MessageBox.Show($"Broj osoba mora biti između 1 i {numPersons.Maximum}.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    Reservation reservation = new Reservation
                    {
                        ClientId = c.Id,
                        ExtraServices = "",
                        NumPersons = (int)numPersons.Value,
                        PackageId = pkg.Id,
                        ReservationDate = DateTime.Now,
                        PackageName = pkg.Name
                    };
                    int id = _reservationFacade.AddReservation(reservation);
                    btnUndo.Visible = true;
                    f.Close();
                    LoadReservations();
                }
            };

            // Trigger filtering only after the form is shown
            f.Shown += (s, e) =>
            {
                
                 btnSave.Top = f.ClientSize.Height - btnSave.Height - 10;

                if (cbDestinations.Items.Count > 0)
                {
                    cbDestinations.SelectedIndex = 0;  // Select first destination
                    UpdatePackageDropdown();           // Filter packages for selected destination

                    if (cbPackages.Items.Count > 0)
                    {
                        cbPackages.SelectedIndex = 0; // Select first package
                        btnSave.Enabled = true;
                    }
                    else
                    {
                        btnSave.Enabled = false;      // No packages - disable save
                    }
                }
                else
                {
                    btnSave.Enabled = false;          // No destinations - disable save
                }
            };

            f.Controls.AddRange(new Control[] {
                lblDestinations, cbDestinations,
                lblPackages, cbPackages,
                lblPersons, numPersons,
                btnSave
            });

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

            var packages = _reservationFacade.GetAllPackages();




            var lblPersons = new Label { Text = "Broj osoba:", Left = 20, Top = 95, Width = 180, Font = new Font("Segoe UI", 11) };
            var numPersons = new NumericUpDown { Left = 20, Top = 125, Width = 120, Minimum = 1, Maximum = 30, Value = 1, Font = new Font("Segoe UI", 11) };
            numPersons.Value = Convert.ToDecimal(grid.SelectedRows[0].Cells[1].Value);

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
                int reservationId = Convert.ToInt32(grid.SelectedRows[0].Cells[3].Value);
                _reservationFacade.UpdateReservation(reservationId, (int)numPersons.Value, "");


                btnUndo.Visible = true;
                LoadReservations();
                f.Close();
            };
            f.Controls.AddRange(new Control[] { lblPackages, lblPersons, numPersons, btnSave });
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
                _reservationFacade.RemoveReservation(id);
                btnUndo.Visible = true;
                LoadReservations();
            }
        }
    }
}