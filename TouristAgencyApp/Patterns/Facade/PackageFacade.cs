using System.Collections.Generic;
using System.Linq;
using TouristAgencyApp.Models;
using TouristAgencyApp.Services;
using TouristAgencyApp.Patterns.Observer.PackageObserver;

namespace TouristAgencyApp.Patterns
{
    public class PackageFacade
    {
        private readonly IDatabaseService _dbService;
        private readonly PackageManager _manager;
        private readonly PackageSubject _subject;

        public PackageFacade(IDatabaseService dbService)
        {
            _dbService = dbService;
            _manager = new PackageManager(dbService);
            _subject = new PackageSubject();
            _subject.Attach(new PackageLogger());
            _subject.Attach(new PackageNotifier());
        }

        public List<TravelPackage> GetAllPackages()
        {
            var data = _dbService.GetAllPackages().ToList();
            foreach (var pkg in data)
                pkg.Details = pkg.ToString();
            return data;
        }

        public int AddPackage(TravelPackage package)
        {
            int id = _manager.AddPackage(package);
            _subject.AddPackage(package, id);
            return id;
        }

        public void UpdatePackage(TravelPackage package)
        {
            _manager.UpdatePackage(package);
            _subject.UpdatePackage(package);
        }

        public void Undo()
        {
            _manager.UndoLastAction();
        }

        public void Redo()
        {
            _manager.RedoLastAction();
        }
    }
}
