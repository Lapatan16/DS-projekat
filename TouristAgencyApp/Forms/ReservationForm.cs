using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouristAgencyApp.Models;
using TouristAgencyApp.Services;
using TouristAgencyApp.Patterns;

namespace TouristAgencyApp.Forms
{
    public partial class ReservationsForm : Form
    {
        private readonly IDatabaseService _db;
        private readonly ReservationSubject _reservationSubject;
        private DataGridView grid;
        private ComboBox cbClients;
        private Button btnAdd;
        private Button btnRemove;

        public ReservationsForm(IDatabaseService dbService)
        {
            _db = dbService;
            _reservationSubject = new ReservationSubject();
            
            // Dodaj observerse
            _reservationSubject.Attach(new ReservationLogger());
            _reservationSubject.Attach(new ReservationNotifier());
            _reservationSubject.Attach(new ReservationStatistics());
            this.Text = "Rezervacije";
            this.Width = 1000;
            this.Height = 600;
            this.StartPosition = FormStartPosition.CenterParent;
               
            var headerPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 40, // PROMENI SA 60 na 40 (ili čak na 36)
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(0, 0, 0, 0) // PROMENI SA (10, 10, 10, 10) na (0, 0, 0, 0)
            };

            cbClients = new ComboBox
            {
                Width = 300,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 12)
            };

            var clientsList = _db.GetAllClients();
            cbClients.DataSource = clientsList;
            cbClients.DisplayMember = "FullName";
            cbClients.ValueMember = "Id";
        
            cbClients.SelectedIndexChanged += (s, e) => LoadReservations();
            var btnAdd = new Button
            {
                Text = "Nova rezervacija",
                Width = 180,
                Height = 40,
                Font = new Font("Segoe UI", 11, FontStyle.Regular),
                Margin = new Padding(16, 0, 0, 0)
            };
            btnAdd.Click += (s, e) => DodajRezervaciju();

            var btnRemove = new Button
            {
                Text = "Otkaži rezervaciju",
                Width = 180,
                Height = 40,
                Font = new Font("Segoe UI", 11, FontStyle.Regular),
                Margin = new Padding(16, 0, 0, 0)
            };
            btnRemove.Click += (s, e) => OtkaziRezervaciju();

            headerPanel.Controls.AddRange(new Control[] { cbClients, btnAdd, btnRemove });
            

            grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AutoGenerateColumns = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                RowTemplate = { Height = 36 },
                AllowUserToAddRows = false,
                MultiSelect = false
            };
            
            this.Controls.Add(grid);
            this.Controls.Add(headerPanel);


            // Definiši kolone ručno da budu jasne i široke
            grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Id",
                HeaderText = "ID",
                Width = 60
            });
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

            // Na kraju:
            //if (clientsList.Count > 0) cbClients.SelectedIndex = 0;
            LoadReservations();
            this.Shown += (s, e) =>
            {
                if(clientsList.Count > 0)cbClients.SelectedIndex = 0;
                LoadReservations();
            };
        }


        private void LoadReservations()
        {
            //if (cbClients.SelectedIndex == -1) cbClients.SelectedIndex = 0;
            if (cbClients.SelectedItem is Client c)
            {
                var reservations = _db.GetReservationsByClient(c.Id).ToList();

                // Popuni naziv paketa za prikaz u gridu
                var packages = _db.GetAllPackages();
                foreach (var r in reservations)
                {
                    var pkg = packages.FirstOrDefault(p => p.Id == r.PackageId);
                    r.PackageName = pkg != null ? pkg.Name : "(nepoznato)";
                }

                // OVDE JE TRIK:
                grid.DataSource = null;
                //grid.Rows.Clear(); // <--- Dodaj i ovo za svaki slučaj
                grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                grid.DataSource = reservations;
                grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
                grid.ColumnHeadersHeight = 40;
                grid.ClearSelection(); // Opciono: da ništa nije selektovano na početku
                grid.AutoResizeColumns();
            }
        }

        private void DodajRezervaciju()
        {
            if (!(cbClients.SelectedItem is Client c)) return;

            var f = new Form { Text = "Nova rezervacija", Width = 420, Height = 350, StartPosition = FormStartPosition.CenterParent };
            var cbPackages = new ComboBox
            {
                Left = 20,
                Top = 25,
                Width = 340,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 11)
            };
            var packages = _db.GetAllPackages();
            cbPackages.DataSource = packages;
            cbPackages.DisplayMember = "Name";

            var lblPackages = new Label { Text = "Paket:", Left = 20, Top = 0, Width = 180 };
            var numPersons = new NumericUpDown { Left = 20, Top = 80, Width = 120, Minimum = 1, Maximum = 30, Value = 1, Font = new Font("Segoe UI", 11) };
            var lblPersons = new Label { Text = "Broj osoba:", Left = 20, Top = 60, Width = 120 };
            var txtExtra = new TextBox { Left = 20, Top = 140, Width = 340, PlaceholderText = "Dodatne usluge", Font = new Font("Segoe UI", 11) };
            var lblExtra = new Label { Text = "Dodatne usluge:", Left = 20, Top = 120, Width = 180 };
            var btnSave = new Button { Text = "Sačuvaj", Left = 20, Top = 190, Width = 120, Height = 40, Font = new Font("Segoe UI", 11, FontStyle.Bold) };

            btnSave.Click += (ss, ee) =>
            {
                if (cbPackages.SelectedItem is TravelPackage pkg)
                {
                    var reservation = new Reservation
                    {
                        ClientId = c.Id,
                        PackageId = pkg.Id,
                        NumPersons = (int)numPersons.Value,
                        ReservationDate = DateTime.Now,
                        ExtraServices = txtExtra.Text
                    };
                    
                    _db.AddReservation(reservation);
                    _reservationSubject.AddReservation(reservation);
                    f.Close();
                    LoadReservations();
                }
            };

            f.Controls.AddRange(new Control[] { lblPackages, cbPackages, lblPersons, numPersons, lblExtra, txtExtra, btnSave });
            f.ShowDialog();
        }

        private void OtkaziRezervaciju()
        {
            if (grid.SelectedRows.Count == 0)
            {
                MessageBox.Show("Prvo izaberi rezervaciju za otkazivanje!", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var id = Convert.ToInt32(grid.SelectedRows[0].Cells[0].Value);
            var confirm = MessageBox.Show("Da li ste sigurni da želite da otkažete ovu rezervaciju?", "Potvrda", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                _db.RemoveReservation(id);
                _reservationSubject.RemoveReservation(id);
                LoadReservations();
            }
        }
    }
}
