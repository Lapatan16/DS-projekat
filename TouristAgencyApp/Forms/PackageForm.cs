using TouristAgencyApp.Models;
using TouristAgencyApp.Patterns;
using TouristAgencyApp.Services;
using TouristAgencyApp.Patterns.Observer.PackageObserver;
public partial class PackagesForm : Form
{
    private readonly IDatabaseService _db;
    private readonly PackageSubject _packageSubject;
    private readonly PackageManager _packageManager;
    private DataGridView grid;
    private Button btnUndo;
    private Button btnRedo;
    string type;
    public PackagesForm(IDatabaseService dbService)
    {
        _packageManager = new PackageManager(dbService);
        _packageSubject = new PackageSubject();
        _packageSubject.Attach(new PackageNotifier());
        _packageSubject.Attach(new PackageLogger());

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
    btnUndo = CreateModernButton("↩️ Undo", Color.FromArgb(255, 255, 165, 0));
    btnUndo.Location = new Point(600, 15);
    btnUndo.TextAlign = ContentAlignment.MiddleCenter;
    btnUndo.Click += (s, e) => OpozoviAkciju();
    btnUndo.Width = 100;

    btnRedo = CreateModernButton("Redo ↪️", Color.FromArgb(255, 255, 165, 0));
    btnRedo.Location = new Point(720, 15);
    btnRedo.TextAlign = ContentAlignment.MiddleCenter;
    btnRedo.Click += (s, e) => NazoviAkciju();
    btnRedo.Width = 100;


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

    toolbarPanel.Controls.AddRange(new Control[] { btnAdd, btnEdit, comboBox, btnUndo, btnRedo });

    
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

    var lblType = new Label { Text = "Tip paketa", Left = 20, Top = 20, Width = 200 };
    var cbType = new ComboBox { Left = 20, Top = 45, Width = 300, DropDownStyle = ComboBoxStyle.DropDownList };
    cbType.Items.AddRange(new[] { "Sea", "Mountain", "Excursion", "Cruise" });
    cbType.SelectedIndex = 0;

    var lblName = new Label { Text = "Naziv paketa", Left = 20, Top = 80, Width = 200 };
    var txtName = new TextBox { Top = 105, Left = 20, Width = 300 };

    var lblPrice = new Label { Text = "Cena", Left = 20, Top = 140, Width = 200 };
    var numPrice = new NumericUpDown { Top = 165, Left = 20, Width = 300, DecimalPlaces = 2, Maximum = 1000000 };

    var lblDestination = new Label { Text = "Destinacija", Left = 20, Top = 200, Width = 200 };
    var txtDestination = new TextBox { Top = 225, Left = 20, Width = 300 };

    var lblAcc = new Label { Text = "Smeštaj", Left = 20, Top = 260, Width = 200 };
    var txtAcc = new TextBox { Top = 285, Left = 20, Width = 300 };

    var lblTransport = new Label { Text = "Prevoz", Left = 20, Top = 320, Width = 200 };
    var txtTransport = new TextBox { Top = 345, Left = 20, Width = 300 };

    var lblActivities = new Label { Text = "Dodatne aktivnosti", Left = 20, Top = 380, Width = 200 };
    var txtActivities = new TextBox { Top = 405, Left = 20, Width = 300 };

    var lblGuide = new Label { Text = "Vodič", Left = 20, Top = 260, Width = 200 };
    var txtGuide = new TextBox { Top = 285, Left = 20, Width = 300 };

    var lblDuration = new Label { Text = "Trajanje (dani)", Left = 20, Top = 380, Width = 200 };
    var numDuration = new NumericUpDown { Top = 405, Left = 20, Width = 300, Maximum = 60, Minimum = 0 };

    var lblShip = new Label { Text = "Brod", Left = 20, Top = 200, Width = 200 };
    var txtShip = new TextBox { Top = 225, Left = 20, Width = 300 };

    var lblRoute = new Label { Text = "Ruta", Left = 20, Top = 260, Width = 200 };
    var txtRoute = new TextBox { Top = 285, Left = 20, Width = 300 };

    var lblDeparture = new Label { Text = "Datum polaska", Left = 20, Top = 320, Width = 200 };
    var dtDeparture = new DateTimePicker { Top = 345, Left = 20, Width = 300 };

    var lblCabin = new Label { Text = "Tip kabine", Left = 20, Top = 380, Width = 200 };
    var txtCabin = new TextBox { Top = 405, Left = 20, Width = 300 };

    var btnSave = new Button { Text = "Sačuvaj", Top = 805, Left = 20, Width = 120, Height = 40, Font = new Font("Segoe UI", 11, FontStyle.Bold), TextAlign = ContentAlignment.MiddleCenter };

    void UpdateFields()
    {
        f.Controls.Clear();
        f.Controls.AddRange(new Control[] { 
            lblType, cbType, 
            lblName, txtName, 
            lblPrice, numPrice, 
            btnSave 
        });
        
        var type = cbType.SelectedItem.ToString();
        int baseHeight = 250; 
        int fieldHeight = 60;
        int buttonPadding = 40;

        if (type == "Sea")
    {
        f.Controls.AddRange(new Control[] { 
            lblDestination, txtDestination, 
            lblAcc, txtAcc, 
            lblTransport, txtTransport 
        });
        btnSave.Top = baseHeight + (3 * fieldHeight) - 40;
        f.Height = baseHeight + (3 * fieldHeight) + 40 + buttonPadding;
    }
    else if (type == "Mountain")
    {
        f.Controls.AddRange(new Control[] { 
            lblDestination, txtDestination, 
            lblAcc, txtAcc, 
            lblTransport, txtTransport,
            lblActivities, txtActivities
        });
        btnSave.Top = baseHeight + (4 * fieldHeight) - 40;
        f.Height = baseHeight + (4 * fieldHeight) + 40 + buttonPadding;
    }
    else if (type == "Excursion")
    {
        f.Controls.AddRange(new Control[] { 
            lblDestination, txtDestination, 
            lblTransport, txtTransport,
            lblGuide, txtGuide,
            lblDuration, numDuration
        });
        btnSave.Top = baseHeight + (4 * fieldHeight) - 40;
        f.Height = baseHeight + (4 * fieldHeight) + 40 + buttonPadding;
    }
    else if (type == "Cruise")
    {
        f.Controls.AddRange(new Control[] { 
            lblShip, txtShip, 
            lblRoute, txtRoute,
            lblDeparture, dtDeparture,
            lblCabin, txtCabin
        });
        btnSave.Top = baseHeight + (4 * fieldHeight) - 40;
        f.Height = baseHeight + (4 * fieldHeight) + 40 + buttonPadding;
    }
    }

    cbType.SelectedIndexChanged += (ss, ee) => UpdateFields();
    UpdateFields();

    btnSave.Click += (ss, ee) =>
    {
        TravelPackage pkg = cbType.SelectedItem.ToString() switch
        {
            "Sea" => PackageDirector.CreateSeaPackage(
                txtName.Text, 
                numPrice.Value, 
                txtDestination.Text, 
                txtAcc.Text, 
                txtTransport.Text),
            "Mountain" => PackageDirector.CreateMountainPackage(
                txtName.Text, 
                numPrice.Value, 
                txtDestination.Text, 
                txtAcc.Text, 
                txtTransport.Text, 
                txtActivities.Text),
            "Excursion" => PackageDirector.CreateExcursionPackage(
                txtName.Text, 
                numPrice.Value, 
                txtDestination.Text, 
                txtTransport.Text, 
                txtGuide.Text, 
                (int)numDuration.Value),
            "Cruise" => PackageDirector.CreateCruisePackage(
                txtName.Text, 
                numPrice.Value, 
                txtShip.Text, 
                txtRoute.Text, 
                dtDeparture.Value, 
                txtCabin.Text),
            _ => throw new Exception("Unknown package type")
        };
        //TravelPackage packege = factory.GetHashCode("", "", "")

        int id = _packageManager.AddPackage(pkg);
        _packageSubject.AddPackage(pkg, id);
        f.Close();
        btnUndo.Visible = true;
        LoadPackages();
    };

    f.ShowDialog();
}
    private void OpozoviAkciju()
    {
        _packageManager.UndoLastAction();
        LoadPackages();
    }
    private void NazoviAkciju()
    {
        _packageManager.RedoLastAction();
        LoadPackages();
    }
    private void IzmeniPaket()
{
    if (grid.SelectedRows.Count == 0) return;
    var pkg = grid.SelectedRows[0].DataBoundItem as TravelPackage;
    if (pkg == null) return;

    var f = new Form { Text = "Izmeni paket", Width = 440, StartPosition = FormStartPosition.CenterParent };

    var lblName = new Label { Text = "Naziv paketa", Left = 20, Top = 20, Width = 200 };
    var txtName = new TextBox { Text = pkg.Name, Top = 45, Left = 20, Width = 300 };

    var lblPrice = new Label { Text = "Cena", Left = 20, Top = 80, Width = 200 };
    var numPrice = new NumericUpDown
    {
        Top = 105,
        Left = 20,
        Width = 300,
        DecimalPlaces = 2,
        Minimum = 0,
        Maximum = 1000000,
        Value = pkg.Price
    };

    var btnSave = new Button { 
        Text = "Sačuvaj", 
        Left = 20, 
        Width = 120, 
        Height = 40, 
        Font = new Font("Segoe UI", 11, FontStyle.Bold), 
        TextAlign = ContentAlignment.MiddleCenter 
    };

    if (pkg.Type == "Sea" && pkg is SeaPackage sea)
    {
        var lblDestination = new Label { Text = "Destinacija", Left = 20, Top = 140, Width = 200 };
        var txtDestination = new TextBox { Text = sea.Destination, Top = 165, Left = 20, Width = 300 };
        
        var lblAcc = new Label { Text = "Smeštaj", Left = 20, Top = 200, Width = 200 };
        var txtAcc = new TextBox { Text = sea.Accommodation, Top = 225, Left = 20, Width = 300 };

        var lblTransport = new Label { Text = "Prevoz", Left = 20, Top = 260, Width = 200 };
        var txtTransport = new TextBox { Text = sea.Transport, Top = 285, Left = 20, Width = 300 };

        btnSave.Top = txtTransport.Bottom + 30;
        f.Height = btnSave.Bottom + 70;

        btnSave.Click += (ss, ee) =>
        {
            var updatedPackage = PackageDirector.CreateSeaPackageForUpdate(
                sea.Id,
                txtName.Text,
                numPrice.Value,
                txtDestination.Text,
                txtAcc.Text,
                txtTransport.Text);
            
            _packageManager.UpdatePackage(updatedPackage);
            _packageSubject.UpdatePackage(updatedPackage);
            f.Close();
            LoadPackages();
        };
        f.Controls.AddRange(new Control[] { 
            lblName, txtName, 
            lblPrice, numPrice, 
            lblDestination, txtDestination, 
            lblAcc, txtAcc, 
            lblTransport, txtTransport, 
            btnSave 
        });
    }
    else if (pkg.Type == "Mountain" && pkg is MountainPackage mountain)
    {
        var lblDestination = new Label { Text = "Destinacija", Left = 20, Top = 140, Width = 200 };
        var txtDestination = new TextBox { Text = mountain.Destination, Top = 165, Left = 20, Width = 300 };
        
        var lblAcc = new Label { Text = "Smeštaj", Left = 20, Top = 200, Width = 200 };
        var txtAcc = new TextBox { Text = mountain.Accommodation, Top = 225, Left = 20, Width = 300 };

        var lblTransport = new Label { Text = "Prevoz", Left = 20, Top = 260, Width = 200 };
        var txtTransport = new TextBox { Text = mountain.Transport, Top = 285, Left = 20, Width = 300 };

        var lblActivities = new Label { Text = "Dodatne aktivnosti", Left = 20, Top = 320, Width = 200 };
        var txtActivities = new TextBox { Text = mountain.Activities, Top = 345, Left = 20, Width = 300 };

        btnSave.Top = txtActivities.Bottom + 30;
        f.Height = btnSave.Bottom + 70;

        btnSave.Click += (ss, ee) =>
        {
            var updatedPackage = PackageDirector.CreateMountainPackageForUpdate(
                mountain.Id,
                txtName.Text,
                numPrice.Value,
                txtDestination.Text,
                txtAcc.Text,
                txtTransport.Text,
                txtActivities.Text);
            
            _packageManager.UpdatePackage(updatedPackage);
            _packageSubject.UpdatePackage(updatedPackage);
            f.Close();
            LoadPackages();
        };
        f.Controls.AddRange(new Control[] { 
            lblName, txtName, 
            lblPrice, numPrice, 
            lblDestination, txtDestination, 
            lblAcc, txtAcc, 
            lblTransport, txtTransport,
            lblActivities, txtActivities,
            btnSave 
        });
    }
    else if (pkg.Type == "Excursion" && pkg is ExcursionPackage excursion)
    {
        var lblDestination = new Label { Text = "Destinacija", Left = 20, Top = 140, Width = 200 };
        var txtDestination = new TextBox { Text = excursion.Destination, Top = 165, Left = 20, Width = 300 };
        
        var lblTransport = new Label { Text = "Prevoz", Left = 20, Top = 200, Width = 200 };
        var txtTransport = new TextBox { Text = excursion.Transport, Top = 225, Left = 20, Width = 300 };

        var lblGuide = new Label { Text = "Vodič", Left = 20, Top = 260, Width = 200 };
        var txtGuide = new TextBox { Text = excursion.Guide, Top = 285, Left = 20, Width = 300 };

        var lblDuration = new Label { Text = "Trajanje (dani)", Left = 20, Top = 320, Width = 200 };
        var numDuration = new NumericUpDown { Value = excursion.Duration, Top = 345, Left = 20, Width = 300, Maximum = 60, Minimum = 0 };

        btnSave.Top = numDuration.Bottom + 30;
        f.Height = btnSave.Bottom + 70;

        btnSave.Click += (ss, ee) =>
        {
            var updatedPackage = PackageDirector.CreateExcursionPackageForUpdate(
                excursion.Id,
                txtName.Text,
                numPrice.Value,
                txtDestination.Text,
                txtTransport.Text,
                txtGuide.Text,
                (int)numDuration.Value);
            
            _packageManager.UpdatePackage(updatedPackage);
            _packageSubject.UpdatePackage(updatedPackage);
            f.Close();
            LoadPackages();
        };
        f.Controls.AddRange(new Control[] { 
            lblName, txtName, 
            lblPrice, numPrice, 
            lblDestination, txtDestination, 
            lblTransport, txtTransport,
            lblGuide, txtGuide,
            lblDuration, numDuration,
            btnSave 
        });
    }
    else if (pkg.Type == "Cruise" && pkg is CruisePackage cruise)
    {
        var lblShip = new Label { Text = "Brod", Left = 20, Top = 140, Width = 200 };
        var txtShip = new TextBox { Text = cruise.Ship, Top = 165, Left = 20, Width = 300 };
        
        var lblRoute = new Label { Text = "Ruta", Left = 20, Top = 200, Width = 200 };
        var txtRoute = new TextBox { Text = cruise.Route, Top = 225, Left = 20, Width = 300 };

        var lblDeparture = new Label { Text = "Datum polaska", Left = 20, Top = 260, Width = 200 };
        var dtDeparture = new DateTimePicker { Value = cruise.DepartureDate, Top = 285, Left = 20, Width = 300 };

        var lblCabin = new Label { Text = "Tip kabine", Left = 20, Top = 320, Width = 200 };
        var txtCabin = new TextBox { Text = cruise.CabinType, Top = 345, Left = 20, Width = 300 };

        btnSave.Top = txtCabin.Bottom + 30;
        f.Height = btnSave.Bottom + 70;

        btnSave.Click += (ss, ee) =>
        {
            var updatedPackage = PackageDirector.CreateCruisePackageForUpdate(
                cruise.Id,
                txtName.Text,
                numPrice.Value,
                txtShip.Text,
                txtRoute.Text,
                dtDeparture.Value,
                txtCabin.Text);
            
            _packageManager.UpdatePackage(updatedPackage);
            _packageSubject.UpdatePackage(updatedPackage);
            f.Close();
            LoadPackages();
        };
        f.Controls.AddRange(new Control[] { 
            lblName, txtName, 
            lblPrice, numPrice, 
            lblShip, txtShip, 
            lblRoute, txtRoute,
            lblDeparture, dtDeparture,
            lblCabin, txtCabin,
            btnSave 
        });
    }
    f.ShowDialog();
}
}