using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouristAgencyApp.Models;
using TouristAgencyApp.Services;

namespace TouristAgencyApp.Forms
{
    //public partial class ClientsForm : Form
    //{
    //    private readonly IDatabaseService _db;
    //    private DataGridView grid;

    //    public ClientsForm(IDatabaseService dbService)
    //    {
    //        _db = dbService;
    //        //InitializeComponent();
    //        this.Text = "Klijenti";
    //        this.Width = 800; this.Height = 500;



    //        grid = new DataGridView
    //        {
    //            Dock = DockStyle.Fill,
    //            ReadOnly = true,
    //            AutoGenerateColumns = true,
    //            SelectionMode = DataGridViewSelectionMode.FullRowSelect
    //        };
    //        this.Controls.Add(grid);

    //        var btnAdd = new Button { Text = "Dodaj klijenta", Dock = DockStyle.Top };
    //        btnAdd.Click += (s, e) => DodajKlijenta();

    //        var btnEdit = new Button { Text = "Izmeni klijenta", Dock = DockStyle.Top };
    //        btnEdit.Click += (s, e) => IzmeniKlijenta();

    //        var panel = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 40 };
    //        panel.Controls.Add(btnAdd);
    //        panel.Controls.Add(btnEdit);
    //        this.Controls.Add(panel);

    //        LoadClients();
    //    }

    //    private void LoadClients()
    //    {
    //        grid.DataSource = null;
    //        grid.DataSource = _db.GetAllClients().ToList();
    //    }

    //    private void DodajKlijenta()
    //    {
    //        var f = new Form { Text = "Novi klijent", Width = 320, Height = 370 };
    //        var txtIme = new TextBox { PlaceholderText = "Ime", Top = 20, Left = 20, Width = 250 };
    //        var txtPrezime = new TextBox { PlaceholderText = "Prezime", Top = 60, Left = 20, Width = 250 };
    //        var txtPass = new TextBox { PlaceholderText = "Broj pasoša", Top = 100, Left = 20, Width = 250 };
    //        var txtEmail = new TextBox { PlaceholderText = "Email", Top = 140, Left = 20, Width = 250 };
    //        var txtTel = new TextBox { PlaceholderText = "Telefon", Top = 180, Left = 20, Width = 250 };
    //        var dtp = new DateTimePicker { Top = 220, Left = 20, Width = 250 };
    //        var btnSave = new Button { Text = "Sačuvaj", Top = 260, Left = 20, Width = 80 };

    //        btnSave.Click += (ss, ee) =>
    //        {
    //            var c = new Client
    //            {
    //                FirstName = txtIme.Text,
    //                LastName = txtPrezime.Text,
    //                PassportNumber = txtPass.Text,
    //                BirthDate = dtp.Value,
    //                Email = txtEmail.Text,
    //                Phone = txtTel.Text
    //            };
    //            _db.AddClient(c);
    //            f.Close();
    //            LoadClients();
    //        };

    //        f.Controls.AddRange(new Control[] { txtIme, txtPrezime, txtPass, txtEmail, txtTel, dtp, btnSave });
    //        f.ShowDialog();
    //    }

    //    private void IzmeniKlijenta()
    //    {
    //        if (grid.SelectedRows.Count == 0) return;
    //        var client = grid.SelectedRows[0].DataBoundItem as Client;
    //        if (client == null) return;

    //        var f = new Form { Text = "Izmeni klijenta", Width = 320, Height = 370 };
    //        var txtIme = new TextBox { Text = client.FirstName, Top = 20, Left = 20, Width = 250 };
    //        var txtPrezime = new TextBox { Text = client.LastName, Top = 60, Left = 20, Width = 250 };
    //        var txtPass = new TextBox { Text = client.PassportNumber, Top = 100, Left = 20, Width = 250 };
    //        var txtEmail = new TextBox { Text = client.Email, Top = 140, Left = 20, Width = 250 };
    //        var txtTel = new TextBox { Text = client.Phone, Top = 180, Left = 20, Width = 250 };
    //        var dtp = new DateTimePicker { Value = client.BirthDate, Top = 220, Left = 20, Width = 250 };
    //        var btnSave = new Button { Text = "Sačuvaj", Top = 260, Left = 20, Width = 80 };

    //        btnSave.Click += (ss, ee) =>
    //        {
    //            client.FirstName = txtIme.Text;
    //            client.LastName = txtPrezime.Text;
    //            client.PassportNumber = txtPass.Text;
    //            client.BirthDate = dtp.Value;
    //            client.Email = txtEmail.Text;
    //            client.Phone = txtTel.Text;
    //            _db.UpdateClient(client);
    //            f.Close();
    //            LoadClients();
    //        };

    //        f.Controls.AddRange(new Control[] { txtIme, txtPrezime, txtPass, txtEmail, txtTel, dtp, btnSave });
    //        f.ShowDialog();
    //    }
    //}
    public partial class ClientsForm : Form
    {
        private readonly IDatabaseService _db;
        private DataGridView grid;

        public ClientsForm(IDatabaseService dbService)
        {
            _db = dbService;
            this.Text = "Klijenti";
            this.Width = 800; this.Height = 500;

            var panel = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 40 };

            var btnAdd = new Button { Text = "Dodaj klijenta" };
            btnAdd.Click += (s, e) => DodajKlijenta();
            panel.Controls.Add(btnAdd);

            var btnEdit = new Button { Text = "Izmeni klijenta" };
            btnEdit.Click += (s, e) => IzmeniKlijenta();
            panel.Controls.Add(btnEdit);

            var txtPretraga = new TextBox { PlaceholderText = "Pretraga...", Width = 200 };
            txtPretraga.TextChanged += (s, e) =>
            {
                var svi = _db.GetAllClients();
                var filter = txtPretraga.Text.ToLower();
                grid.DataSource = svi.Where(x =>
                    x.FirstName.ToLower().Contains(filter) ||
                    x.LastName.ToLower().Contains(filter) ||
                    x.PassportNumber.ToLower().Contains(filter)
                ).ToList();
            };
            panel.Controls.Add(txtPretraga);

            this.Controls.Add(panel);

            grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AutoGenerateColumns = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            this.Controls.Add(grid);

            grid.CellDoubleClick += (s, e) => PrikaziRezervacijeZaKlijenta();

            LoadClients();
        }

        private void LoadClients()
        {
            grid.DataSource = null;
            grid.DataSource = _db.GetAllClients().ToList();
        }

        private void DodajKlijenta()
        {
            var f = new Form { Text = "Novi klijent", Width = 320, Height = 370 };
            var txtIme = new TextBox { PlaceholderText = "Ime", Top = 20, Left = 20, Width = 250 };
            var txtPrezime = new TextBox { PlaceholderText = "Prezime", Top = 60, Left = 20, Width = 250 };
            var txtPass = new TextBox { PlaceholderText = "Broj pasoša", Top = 100, Left = 20, Width = 250 };
            var txtEmail = new TextBox { PlaceholderText = "Email", Top = 140, Left = 20, Width = 250 };
            var txtTel = new TextBox { PlaceholderText = "Telefon", Top = 180, Left = 20, Width = 250 };
            var dtp = new DateTimePicker { Top = 220, Left = 20, Width = 250 };
            var btnSave = new Button { Text = "Sačuvaj", Top = 260, Left = 20, Width = 80 };

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

            f.Controls.AddRange(new Control[] { txtIme, txtPrezime, txtPass, txtEmail, txtTel, dtp, btnSave });
            f.ShowDialog();
        }

        private void IzmeniKlijenta()
        {
            if (grid.SelectedRows.Count == 0) return;
            var client = grid.SelectedRows[0].DataBoundItem as Client;
            if (client == null) return;

            var f = new Form { Text = "Izmeni klijenta", Width = 320, Height = 370 };
            var txtIme = new TextBox { Text = client.FirstName, Top = 20, Left = 20, Width = 250 };
            var txtPrezime = new TextBox { Text = client.LastName, Top = 60, Left = 20, Width = 250 };
            var txtPass = new TextBox { Text = client.PassportNumber, Top = 100, Left = 20, Width = 250 };
            var txtEmail = new TextBox { Text = client.Email, Top = 140, Left = 20, Width = 250 };
            var txtTel = new TextBox { Text = client.Phone, Top = 180, Left = 20, Width = 250 };
            var dtp = new DateTimePicker { Value = client.BirthDate, Top = 220, Left = 20, Width = 250 };
            var btnSave = new Button { Text = "Sačuvaj", Top = 260, Left = 20, Width = 80 };

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

            f.Controls.AddRange(new Control[] { txtIme, txtPrezime, txtPass, txtEmail, txtTel, dtp, btnSave });
            f.ShowDialog();
        }

        private void PrikaziRezervacijeZaKlijenta()
        {
            if (grid.SelectedRows.Count == 0) return;
            var client = grid.SelectedRows[0].DataBoundItem as Client;
            if (client == null) return;
            var form = new Form { Text = $"Rezervacije za {client.FirstName} {client.LastName}", Width = 700, Height = 400 };
            var gridRez = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AutoGenerateColumns = true };
            gridRez.DataSource = _db.GetReservationsByClient(client.Id);
            form.Controls.Add(gridRez);
            form.ShowDialog();
        }
    }
}
