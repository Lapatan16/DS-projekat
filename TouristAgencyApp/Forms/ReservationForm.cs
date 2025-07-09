using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouristAgencyApp.Models;
using TouristAgencyApp.Services;

namespace TouristAgencyApp.Forms
{
    //public partial class ReservationsForm : Form
    //{
    //    private readonly IDatabaseService _db;
    //    private DataGridView grid;
    //    private ComboBox cbClients;
    //    private Button btnAdd;

    //    public ReservationsForm(IDatabaseService dbService)
    //    {
    //        _db = dbService;
    //        //InitializeComponent();
    //        this.Text = "Rezervacije";
    //        this.Width = 900; this.Height = 500;

    //        cbClients = new ComboBox { Left = 20, Top = 20, Width = 300, DropDownStyle = ComboBoxStyle.DropDownList };
    //        cbClients.DataSource = _db.GetAllClients();
    //        cbClients.DisplayMember = "FirstName";
    //        cbClients.SelectedIndexChanged += (s, e) => LoadReservations();

    //        btnAdd = new Button { Text = "Nova rezervacija", Left = 350, Top = 20, Width = 150 };
    //        btnAdd.Click += (s, e) => DodajRezervaciju();

    //        grid = new DataGridView
    //        {
    //            Dock = DockStyle.Bottom,
    //            ReadOnly = true,
    //            AutoGenerateColumns = true,
    //            Height = 400
    //        };

    //        this.Controls.Add(cbClients);
    //        this.Controls.Add(btnAdd);
    //        this.Controls.Add(grid);

    //        LoadReservations();
    //    }

    //    private void LoadReservations()
    //    {
    //        if (cbClients.SelectedItem is Client c)
    //            grid.DataSource = _db.GetReservationsByClient(c.Id).ToList();
    //    }

    //    private void DodajRezervaciju()
    //    {
    //        if (!(cbClients.SelectedItem is Client c)) return;

    //        var f = new Form { Text = "Nova rezervacija", Width = 380, Height = 340 };
    //        var cbPackages = new ComboBox { Left = 20, Top = 20, Width = 300, DropDownStyle = ComboBoxStyle.DropDownList };
    //        cbPackages.DataSource = _db.GetAllPackages();
    //        cbPackages.DisplayMember = "Name";
    //        var numPersons = new NumericUpDown { Left = 20, Top = 70, Width = 80, Minimum = 1, Maximum = 30 };
    //        var txtExtra = new TextBox { Left = 20, Top = 110, Width = 300, PlaceholderText = "Dodatne usluge" };
    //        var btnSave = new Button { Text = "Sačuvaj", Left = 20, Top = 150 };

    //        btnSave.Click += (ss, ee) =>
    //        {
    //            if (cbPackages.SelectedItem is TravelPackage pkg)
    //            {
    //                _db.AddReservation(new Reservation
    //                {
    //                    ClientId = c.Id,
    //                    PackageId = pkg.Id,
    //                    NumPersons = (int)numPersons.Value,
    //                    ReservationDate = DateTime.Now,
    //                    ExtraServices = txtExtra.Text
    //                });
    //                f.Close();
    //                LoadReservations();
    //            }
    //        };

    //        f.Controls.AddRange(new Control[] { cbPackages, numPersons, txtExtra, btnSave });
    //        f.ShowDialog();
    //    }
    //}
    public partial class ReservationsForm : Form
    {
        private readonly IDatabaseService _db;
        private DataGridView grid;
        private ComboBox cbClients;

        public ReservationsForm(IDatabaseService dbService)
        {
            _db = dbService;
            this.Text = "Rezervacije";
            this.Width = 900; this.Height = 500;

            cbClients = new ComboBox { Left = 20, Top = 20, Width = 300, DropDownStyle = ComboBoxStyle.DropDownList };
            cbClients.DataSource = _db.GetAllClients();
            cbClients.DisplayMember = "FirstName";
            cbClients.SelectedIndexChanged += (s, e) => LoadReservations();

            var btnAdd = new Button { Text = "Nova rezervacija", Left = 350, Top = 20, Width = 150 };
            btnAdd.Click += (s, e) => DodajRezervaciju();

            var btnRemove = new Button { Text = "Otkaži rezervaciju", Left = 520, Top = 20, Width = 180 };
            btnRemove.Click += (s, e) => OtkaziRezervaciju();

            grid = new DataGridView
            {
                Dock = DockStyle.Bottom,
                ReadOnly = true,
                AutoGenerateColumns = true,
                Height = 400
            };

            this.Controls.Add(cbClients);
            this.Controls.Add(btnAdd);
            this.Controls.Add(btnRemove);
            this.Controls.Add(grid);

            LoadReservations();
        }

        private void LoadReservations()
        {
            if (cbClients.SelectedItem is Client c)
                grid.DataSource = _db.GetReservationsByClient(c.Id).ToList();
        }

        private void DodajRezervaciju()
        {
            if (!(cbClients.SelectedItem is Client c)) return;

            var f = new Form { Text = "Nova rezervacija", Width = 380, Height = 340 };
            var cbPackages = new ComboBox { Left = 20, Top = 20, Width = 300, DropDownStyle = ComboBoxStyle.DropDownList };
            cbPackages.DataSource = _db.GetAllPackages();
            cbPackages.DisplayMember = "Name";
            var numPersons = new NumericUpDown { Left = 20, Top = 70, Width = 80, Minimum = 1, Maximum = 30 };
            var txtExtra = new TextBox { Left = 20, Top = 110, Width = 300, PlaceholderText = "Dodatne usluge" };
            var btnSave = new Button { Text = "Sačuvaj", Left = 20, Top = 150 };

            btnSave.Click += (ss, ee) =>
            {
                if (cbPackages.SelectedItem is TravelPackage pkg)
                {
                    _db.AddReservation(new Reservation
                    {
                        ClientId = c.Id,
                        PackageId = pkg.Id,
                        NumPersons = (int)numPersons.Value,
                        ReservationDate = DateTime.Now,
                        ExtraServices = txtExtra.Text
                    });
                    f.Close();
                    LoadReservations();
                }
            };

            f.Controls.AddRange(new Control[] { cbPackages, numPersons, txtExtra, btnSave });
            f.ShowDialog();
        }

        private void OtkaziRezervaciju()
        {
            if (grid.SelectedRows.Count == 0)
            {
                MessageBox.Show("Prvo izaberi rezervaciju za otkazivanje!");
                return;
            }
            var reservation = grid.SelectedRows[0].DataBoundItem as Reservation;
            if (reservation == null) return;
            var confirm = MessageBox.Show("Da li ste sigurni da želite da otkažete ovu rezervaciju?", "Potvrda", MessageBoxButtons.YesNo);
            if (confirm == DialogResult.Yes)
            {
                _db.RemoveReservation(reservation.Id);
                LoadReservations();
            }
        }
    }
}
