using System;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using TouristAgencyApp.Models;
using TouristAgencyApp.Patterns;
using TouristAgencyApp.Patterns.Facade;
using TouristAgencyApp.Services;
using TouristAgencyApp.Utils;

namespace TouristAgencyApp.Forms

{
    public partial class PackagesForm : Form
    {
        private readonly PackageFacade _packageFacade;
        private string type;

        public PackagesForm(IDatabaseService dbService)
        {
            InitializeComponent();
            _packageFacade = new PackageFacade(dbService);
            type = "Svi paketi";
            btnEdit.Click += (s, e) => Izmeni();
            btnAdd.Click += (s, e) => DodajPaket();
            btnUndo.Click += (s, e) => OpozoviAkciju();
            btnRedo.Click += (s, e) => NazoviAkciju();
            LoadPackages();
        }

        private void LoadPackages()
        {
            grid.DataSource = null;
            grid.Columns.Clear();
            
            var data = _packageFacade.GetAllPackages();
            List<TravelPackage> lista = new List<TravelPackage>();

            foreach (var pkg in data)
            {
                if (type == "Svi paketi" || type == pkg.Type)
                    lista.Add(pkg);
            }
            setupGrid(lista);
            
        }
        private void OpozoviAkciju()
        {
            _packageFacade.Undo();
            LoadPackages();
        }
        private void NazoviAkciju()
        {
            _packageFacade.Redo();
            LoadPackages();
        }
        private void Izmeni()
        {

            PackageEditContext context = IzmeniPaket();
            TravelPackage pkg = context.Package;
            if (pkg.Type == "Sea" && pkg is SeaPackage sea)
            {
                context.BtnSave.Click += (ss, ee) =>
                {
                    var updatedPackage = PackageDirector.CreateSeaPackageForUpdate(
                        sea.Id,
                        context.TxtName.Text,
                        context.NumPrice.Value,
                        context.TxtDestination.Text,
                        context.TxtAcc.Text,
                        context.TxtTransport.Text);

                    _packageFacade.UpdatePackage(updatedPackage);
                    context.Form.Close();
                    LoadPackages();
                };
            }
            else if(pkg.Type == "Mountain" && pkg is MountainPackage mountain)
            {
                context.BtnSave.Click += (ss, ee) =>
                {
                    var updatedPackage = PackageDirector.CreateMountainPackageForUpdate(
                        mountain.Id,
                        context.TxtName.Text,
                        context.NumPrice.Value,
                        context.TxtDestination.Text,
                        context.TxtAcc.Text,
                        context.TxtTransport.Text,
                        context.TxtActivities.Text);

                    _packageFacade.UpdatePackage(updatedPackage);
                    context.Form.Close();
                    LoadPackages();
                };
            }
            else if(pkg.Type == "Cruise" && pkg is CruisePackage cruise)
            {
                context.BtnSave.Click += (ss, ee) =>
                {
                    var updatedPackage = PackageDirector.CreateCruisePackageForUpdate(
                        cruise.Id,
                        context.TxtName.Text,
                        context.NumPrice.Value,
                        context.TxtShip.Text,
                        context.TxtRoute.Text,
                        context.DtDeparture.Value,
                        context.TxtCabin.Text);

                    _packageFacade.UpdatePackage(updatedPackage);
                    context.Form.Close();
                    LoadPackages();
                };
            }
            else if (pkg.Type == "Excursion" && pkg is ExcursionPackage excursion)
            {
                context.BtnSave.Click += (ss, ee) =>
                {
                    var updatedPackage = PackageDirector.CreateExcursionPackageForUpdate(
                        excursion.Id,
                        context.TxtName.Text,
                        context.NumPrice.Value,
                        context.TxtDestination.Text,
                        context.TxtTransport.Text,
                        context.TxtGuide.Text,
                        (int)context.NumDuration.Value);

                    _packageFacade.UpdatePackage(updatedPackage);
                    context.Form.Close();
                    LoadPackages();
                };
            }
            context.Form.ShowDialog();
        }
    }
    
}