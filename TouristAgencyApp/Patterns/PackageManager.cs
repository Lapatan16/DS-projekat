using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouristAgencyApp.Models;
using TouristAgencyApp.Services;

namespace TouristAgencyApp.Patterns
{
    class PackageManager
    {
        private readonly CommandInvoker _invoker = new();
        private readonly IDatabaseService _dbService;

        public PackageManager(IDatabaseService dbService)
        {
            _dbService = dbService;
        }
        public int AddPackage(TravelPackage package)
        {
            var addCommand = new AddPackageCommand(_dbService, package);
            _invoker.ExecuteCommand(addCommand);
            MessageBox.Show("Package added.");
            return addCommand.PackageId;
        }
        public void UpdatePackage(TravelPackage package)
        {
            var updateCommand = new UpdatePackageCommand(_dbService, package);
            _invoker.ExecuteCommand(updateCommand);
            MessageBox.Show("Package updated.");
        }
        public void RemovePackage(int packageId)
        {
            //Po zahtevima za zadatak nije potrebno.
        }
        public void UndoLastAction()
        {
            _invoker.UndoLastCommand();
            MessageBox.Show("Poslednja akcija uspesno opozvana!");
        }
    }
}
