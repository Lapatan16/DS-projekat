using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using TouristAgencyApp.Models;
using TouristAgencyApp.Patterns;
using TouristAgencyApp.Services;

namespace TouristAgencyApp.Forms
{
    public partial class ReservationsForm : Form
    {
        private readonly ReservationFacade _reservationFacade;

        public ReservationsForm(IDatabaseService dbService)
        {
            InitializeComponent();

            _reservationFacade = new ReservationFacade(dbService);

            // Hook up event handlers
            cbClients.SelectedIndexChanged += (s, e) => LoadReservations();
            btnAdd.Click += (s, e) => DodajRezervaciju();
            btnRemove.Click += (s, e) => OtkaziRezervaciju();
            btnEdit.Click += (s, e) => AzurirajRezervaciju();
            btnUndo.Click += (s, e) => OpozoviAkciju();
            btnRedo.Click += (s, e) => NazoviAkciju();

            LoadClients();
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


            var uniqueDestinations = _reservationFacade.GetUniqueDestinations();


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

                var filteredPackages = _reservationFacade.GetPackagesByDestination(selectedDestination);

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
            var id = Convert.ToInt32(grid.SelectedRows[0].Cells[3].Value);
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
