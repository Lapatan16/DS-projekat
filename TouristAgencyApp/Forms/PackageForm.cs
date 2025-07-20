using TouristAgencyApp.Models;
using TouristAgencyApp.Patterns;
using TouristAgencyApp.Services;

public partial class PackagesForm : Form
{
    private readonly IDatabaseService _db;
    private DataGridView grid;
    string type;
    public PackagesForm(IDatabaseService dbService)
    {
        type = "Svi paketi";
        _db = dbService;
        this.Text = "Paketi";
        this.Width = 1100;
        this.Height = 560;
        this.StartPosition = FormStartPosition.CenterScreen;
        var comboBox = new ComboBox
        {
            DropDownStyle = ComboBoxStyle.DropDownList,
            Width = 100,
            Height = 200,
            Margin = new Padding(10, 10, 10, 10),
            Font = new Font("Segoe UI", 11, FontStyle.Regular)

        };
        comboBox.Items.AddRange(new String[]
        {
            "Svi paketi", "Sea", "Excursion", "Mountain", "Cruise"
        });
        comboBox.SelectedIndex = 0;
        comboBox.SelectedIndexChanged += (s, e) =>
        {
            type = comboBox.SelectedItem.ToString();
            LoadPackages();
        };
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

        var panel = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            Height = 60,
            FlowDirection = FlowDirection.LeftToRight,
            Padding = new Padding(0, 10, 0, 10)
        };

        var btnAdd = new Button
        {
            Text = "Dodaj paket",
            Width = 180,
            Height = 40,
            Font = new Font("Segoe UI", 11, FontStyle.Regular),
            TextAlign = ContentAlignment.MiddleCenter,
            Margin = new Padding(10, 10, 10, 10)
        };
        btnAdd.Click += (s, e) => DodajPaket();
        panel.Controls.Add(btnAdd);

        var btnEdit = new Button
        {
            Text = "Izmeni paket",
            Width = 180,
            Height = 40,
            Font = new Font("Segoe UI", 11, FontStyle.Regular),
            TextAlign = ContentAlignment.MiddleCenter,
            Margin = new Padding(10, 10, 10, 10)
        };
        btnEdit.Click += (s, e) => IzmeniPaket();
        panel.Controls.Add(btnEdit);
        panel.Controls.Add(comboBox);
        CreateModernUI();

        LoadPackages();
    }

   

private void CreateModernUI()
{
    
    var headerPanel = new Panel
    {
        Dock = DockStyle.Top,
        Height = 80,
        BackColor = Color.FromArgb(52, 152, 219)
    };

    var lblTitle = new Label
    {
        Text = "Upravljanje paketima",
        Font = new Font("Segoe UI", 18, FontStyle.Bold),
        ForeColor = Color.White,
        AutoSize = false,
        Dock = DockStyle.Fill,
        TextAlign = ContentAlignment.MiddleCenter
    };
    headerPanel.Controls.Add(lblTitle);

    
    var toolbarPanel = new Panel
    {
        Dock = DockStyle.Top,
        Height = 60,
        BackColor = Color.White,
        Padding = new Padding(20, 10, 20, 10)
    };

    var btnAdd = CreateModernButton("➕ Dodaj paket", Color.FromArgb(46, 204, 113));
    btnAdd.Click += (s, e) => DodajPaket();

    var btnEdit = CreateModernButton("✏️ Izmeni paket", Color.FromArgb(52, 152, 219));
    btnEdit.Click += (s, e) => IzmeniPaket();

    var comboBox = new ComboBox
    {
        DropDownStyle = ComboBoxStyle.DropDownList,
        Width = 180,
        Font = new Font("Segoe UI", 10),
        Left = 380,
        Top = 15
    };
    comboBox.Items.AddRange(new string[] { "Svi paketi", "Sea", "Excursion", "Mountain", "Cruise" });
    comboBox.SelectedIndex = 0;
    comboBox.SelectedIndexChanged += (s, e) =>
    {
        type = comboBox.SelectedItem.ToString();
        LoadPackages();
    };

    btnAdd.Location = new Point(20, 10);
    btnEdit.Location = new Point(200, 10);

    toolbarPanel.Controls.AddRange(new Control[] { btnAdd, btnEdit, comboBox });

    
    grid.Dock = DockStyle.Fill;
    grid.BackgroundColor = Color.White;
    grid.BorderStyle = BorderStyle.None;
    grid.Font = new Font("Segoe UI", 10);
    grid.RowTemplate.Height = 35;
    grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
    grid.GridColor = Color.LightGray;
    grid.EnableHeadersVisualStyles = false;
    grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(52, 73, 94);
    grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
    grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
    grid.ColumnHeadersHeight = 40;

    
    this.Controls.Add(grid);
    this.Controls.Add(toolbarPanel);
    this.Controls.Add(headerPanel);
    }

    private Button CreateModernButton(string text, Color color)
    {
    return new Button
    {
        Text = text,
        BackColor = color,
        ForeColor = Color.White,
        Font = new Font("Segoe UI", 10, FontStyle.Bold),
        Width = 160,
        Height = 35,
        FlatStyle = FlatStyle.Flat,
        FlatAppearance = { BorderSize = 0 },
        Cursor = Cursors.Hand
    };
    }

    private void LoadPackages()
    {
        grid.DataSource = null;
    grid.Columns.Clear();

    var data = _db.GetAllPackages().ToList();
    List<TravelPackage> lista = new List<TravelPackage>();

    foreach (var pkg in data)
        pkg.Details = pkg.ToString();

    foreach (var pkg in data)
    {
        if (type == "Svi paketi" || type == pkg.Type)
            lista.Add(pkg);
    }

    grid.AutoGenerateColumns = true;
    grid.DataSource = lista;
    grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
    grid.AutoResizeColumns();
    grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
    grid.ColumnHeadersHeight = 40;

    if (grid.Columns.Contains("Id"))
        grid.Columns["Id"].Visible = false;

    if (grid.Columns.Contains("Name"))
    grid.Columns["Name"].HeaderText = "Naziv";
if (grid.Columns.Contains("Price"))
    grid.Columns["Price"].HeaderText = "Cena";
if (grid.Columns.Contains("Type"))
    grid.Columns["Type"].HeaderText = "Tip";
if (grid.Columns.Contains("Details"))
    grid.Columns["Details"].HeaderText = "Detalji";
    }

    private void DodajPaket()
    {
        var f = new Form { Text = "Novi paket", Width = 440, Height = 700, StartPosition = FormStartPosition.CenterParent };

        var cbType = new ComboBox { Left = 20, Top = 20, Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
        cbType.Items.AddRange(new[] { "Sea", "Mountain", "Excursion", "Cruise" });
        cbType.SelectedIndex = 0;
        var lblType = new Label { Text = "Tip paketa", Left = 20, Top = 0, Width = 200 };

        var txtName = new TextBox { PlaceholderText = "Naziv paketa", Top = 60, Left = 20, Width = 300 };
        var numPrice = new NumericUpDown { Top = 100, Left = 20, Width = 300, DecimalPlaces = 2, Maximum = 1000000 };

        var txtDestination = new TextBox { PlaceholderText = "Destinacija", Top = 140, Left = 20, Width = 300 };
        var txtAcc = new TextBox { PlaceholderText = "Smeštaj", Top = 180, Left = 20, Width = 300 };
        var txtTransport = new TextBox { PlaceholderText = "Prevoz", Top = 220, Left = 20, Width = 300 };
        var txtActivities = new TextBox { PlaceholderText = "Dodatne aktivnosti (planine)", Top = 260, Left = 20, Width = 300 };
        var txtGuide = new TextBox { PlaceholderText = "Vodič (ekskurzija)", Top = 300, Left = 20, Width = 300 };
        var numDuration = new NumericUpDown { Top = 340, Left = 20, Width = 300, Maximum = 60, Minimum = 0 };
        var txtShip = new TextBox { PlaceholderText = "Brod (krstarenje)", Top = 380, Left = 20, Width = 300 };
        var txtRoute = new TextBox { PlaceholderText = "Ruta (krstarenje)", Top = 420, Left = 20, Width = 300 };
        var dtDeparture = new DateTimePicker { Top = 460, Left = 20, Width = 300 };
        var txtCabin = new TextBox { PlaceholderText = "Tip kabine (krstarenje)", Top = 500, Left = 20, Width = 300 };

        var btnSave = new Button { Text = "Sačuvaj", Top = 540, Left = 20, Width = 120, Height = 40, Font = new Font("Segoe UI", 11, FontStyle.Bold), TextAlign = ContentAlignment.MiddleCenter };

        void UpdateFields()
        {
            f.Controls.Clear();
            f.Controls.AddRange(new Control[] { lblType, cbType, txtName, numPrice, btnSave });
            var type = cbType.SelectedItem.ToString();
            if (type == "Sea")
                f.Controls.AddRange(new Control[] { txtDestination, txtAcc, txtTransport });
            if (type == "Mountain")
                f.Controls.AddRange(new Control[] { txtDestination, txtAcc, txtTransport, txtActivities });
            if (type == "Excursion")
                f.Controls.AddRange(new Control[] { txtDestination, txtTransport, txtGuide, numDuration });
            if (type == "Cruise")
                f.Controls.AddRange(new Control[] { txtShip, txtRoute, dtDeparture, txtCabin });
        }
        cbType.SelectedIndexChanged += (ss, ee) => UpdateFields();
        UpdateFields();


        btnSave.Click += (ss, ee) =>
        {
            PackageFactory factory = cbType.SelectedItem.ToString() switch
            {
                "Sea" => new SeaPackageFactory(),
                "Mountain" => new MountainPackageFactory(),
                "Excursion" => new ExcursionPackageFactory(),
                "Cruise" => new CruisePackageFactory(),
                _ => throw new Exception("Unknown package type")
            };

            TravelPackage pkg = factory.CreatePackage();

            pkg.Name = txtName.Text;
            pkg.Price = numPrice.Value;

            switch (pkg)
            {
                case SeaPackage sea:
                    sea.Destination = txtDestination.Text;
                    sea.Accommodation = txtAcc.Text;
                    sea.Transport = txtTransport.Text;
                    break;
                case MountainPackage mountain:
                    mountain.Destination = txtDestination.Text;
                    mountain.Accommodation = txtAcc.Text;
                    mountain.Transport = txtTransport.Text;
                    mountain.Activities = txtActivities.Text;
                    break;
                case ExcursionPackage excursion:
                    excursion.Destination = txtDestination.Text;
                    excursion.Transport = txtTransport.Text;
                    excursion.Guide = txtGuide.Text;
                    excursion.Duration = (int)numDuration.Value;
                    break;
                case CruisePackage cruise:
                    cruise.Ship = txtShip.Text;
                    cruise.Route = txtRoute.Text;
                    cruise.DepartureDate = dtDeparture.Value;
                    cruise.CabinType = txtCabin.Text;
                    break;
            }

            //MessageBox.Show(pkg.GetType().FullName, "Tip objekta koji dodajem");
            //MessageBox.Show(System.Text.Json.JsonSerializer.Serialize(pkg), "Sadržaj objekta koji šaljem");
            //MessageBox.Show((ExcursionPackage)pkg.)
            //MessageBox.Show("Vodic: " + txtGuide.Text);
            _db.AddPackage(pkg);
            f.Close();
            LoadPackages();
        };

        f.ShowDialog();
    }

    private void IzmeniPaket()
    {
        if (grid.SelectedRows.Count == 0) return;
        var pkg = grid.SelectedRows[0].DataBoundItem as TravelPackage;
        if (pkg == null) return;

        var f = new Form { Text = "Izmeni paket", Width = 440, Height = 700, StartPosition = FormStartPosition.CenterParent };

        var txtName = new TextBox { Text = pkg.Name, Top = 20, Left = 20, Width = 300 };
        var numPrice = new NumericUpDown
        {
            Top = 60,
            Left = 20,
            Width = 300,
            DecimalPlaces = 2,
            Minimum = 0,
            Maximum = 1000000
        };
        numPrice.Value = pkg.Price;

        if (pkg.Type == "Sea" && pkg is SeaPackage sea)
        {
            var txtDestination = new TextBox { Text = sea.Destination, Top = 100, Left = 20, Width = 300 };
            var txtAcc = new TextBox { Text = sea.Accommodation, Top = 140, Left = 20, Width = 300 };
            var txtTransport = new TextBox { Text = sea.Transport, Top = 180, Left = 20, Width = 300 };
            var btnSave = new Button { Text = "Sačuvaj", Top = 240, Left = 20, Width = 120, Height = 40, Font = new Font("Segoe UI", 11, FontStyle.Bold), TextAlign = ContentAlignment.MiddleCenter };
            btnSave.Click += (ss, ee) =>
            {
                sea.Name = txtName.Text;
                sea.Price = numPrice.Value;
                sea.Destination = txtDestination.Text;
                sea.Accommodation = txtAcc.Text;
                sea.Transport = txtTransport.Text;
                _db.UpdatePackage(sea);
                f.Close();
                LoadPackages();
            };
            f.Controls.AddRange(new Control[] { txtName, numPrice, txtDestination, txtAcc, txtTransport, btnSave });
        }
        else if (pkg.Type == "Mountain" && pkg is MountainPackage mountain)
        {
            var txtDestination = new TextBox { Text = mountain.Destination, Top = 100, Left = 20, Width = 300 };
            var txtAcc = new TextBox { Text = mountain.Accommodation, Top = 140, Left = 20, Width = 300 };
            var txtTransport = new TextBox { Text = mountain.Transport, Top = 180, Left = 20, Width = 300 };
            var txtActivities = new TextBox { Text = mountain.Activities, Top = 220, Left = 20, Width = 300 };
            var btnSave = new Button { Text = "Sačuvaj", Top = 270, Left = 20, Width = 120, Height = 40, Font = new Font("Segoe UI", 11, FontStyle.Bold), TextAlign = ContentAlignment.MiddleCenter };
            btnSave.Click += (ss, ee) =>
            {
                mountain.Name = txtName.Text;
                mountain.Price = numPrice.Value;
                mountain.Destination = txtDestination.Text;
                mountain.Accommodation = txtAcc.Text;
                mountain.Transport = txtTransport.Text;
                mountain.Activities = txtActivities.Text;
                _db.UpdatePackage(mountain);
                f.Close();
                LoadPackages();
            };
            f.Controls.AddRange(new Control[] { txtName, numPrice, txtDestination, txtAcc, txtTransport, txtActivities, btnSave });
        }
        else if (pkg.Type == "Excursion" && pkg is ExcursionPackage excursion)
        {
            var txtDestination = new TextBox { Text = excursion.Destination, Top = 100, Left = 20, Width = 300 };
            var txtTransport = new TextBox { Text = excursion.Transport, Top = 140, Left = 20, Width = 300 };
            var txtGuide = new TextBox { Text = excursion.Guide, Top = 180, Left = 20, Width = 300 };
            var numDuration = new NumericUpDown { Value = excursion.Duration, Top = 220, Left = 20, Width = 300, Maximum = 60, Minimum = 0 };
            var btnSave = new Button { Text = "Sačuvaj", Top = 270, Left = 20, Width = 120, Height = 40, Font = new Font("Segoe UI", 11, FontStyle.Bold), TextAlign = ContentAlignment.MiddleCenter };
            btnSave.Click += (ss, ee) =>
            {
                excursion.Name = txtName.Text;
                excursion.Price = numPrice.Value;
                excursion.Destination = txtDestination.Text;
                excursion.Transport = txtTransport.Text;
                excursion.Guide = txtGuide.Text;
                excursion.Duration = (int)numDuration.Value;
                _db.UpdatePackage(excursion);
                f.Close();
                LoadPackages();
            };
            f.Controls.AddRange(new Control[] { txtName, numPrice, txtDestination, txtTransport, txtGuide, numDuration, btnSave });
        }
        else if (pkg.Type == "Cruise" && pkg is CruisePackage cruise)
        {
            var txtShip = new TextBox { Text = cruise.Ship, Top = 100, Left = 20, Width = 300 };
            var txtRoute = new TextBox { Text = cruise.Route, Top = 140, Left = 20, Width = 300 };
            var dtDeparture = new DateTimePicker { Value = cruise.DepartureDate, Top = 180, Left = 20, Width = 300 };
            var txtCabin = new TextBox { Text = cruise.CabinType, Top = 220, Left = 20, Width = 300 };
            var btnSave = new Button { Text = "Sačuvaj", Top = 270, Left = 20, Width = 120, Height = 40, Font = new Font("Segoe UI", 11, FontStyle.Bold), TextAlign = ContentAlignment.MiddleCenter };
            btnSave.Click += (ss, ee) =>
            {
                cruise.Name = txtName.Text;
                cruise.Price = numPrice.Value;
                cruise.Ship = txtShip.Text;
                cruise.Route = txtRoute.Text;
                cruise.DepartureDate = dtDeparture.Value;
                cruise.CabinType = txtCabin.Text;
                _db.UpdatePackage(cruise);
                f.Close();
                LoadPackages();
            };
            f.Controls.AddRange(new Control[] { txtName, numPrice, txtShip, txtRoute, dtDeparture, txtCabin, btnSave });
        }
        f.ShowDialog();
    }
}