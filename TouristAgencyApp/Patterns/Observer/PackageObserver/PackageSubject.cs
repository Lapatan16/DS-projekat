using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouristAgencyApp.Models;

namespace TouristAgencyApp.Patterns.Observer.PackageObserver
{
    public class PackageSubject : ISubject
    {
        private readonly List<IObserver> _observers = new();
        private readonly List<TravelPackage> _packages = new();
        public void Attach(IObserver observer)
        {
            if (!_observers.Contains(observer))
                _observers.Add(observer);
        }

        public void Detach(IObserver observer)
        {
            _observers.Remove(observer);
        }

        public void Notify(string message)
        {
            foreach (var observer in _observers)
            {
                observer.Update(message);
            }
        }

        public void AddPackage(TravelPackage package, int id)
        {
            _packages.Add(package);
            Notify($"Novi paket dodat: {package.Name} | id: {id}");
        }
        public void UpdatePackage(TravelPackage package)
        {
            _packages.Add(package);
            Notify($"Paket sa id = {package.Id} je azuriran");
        }
        public void RemovePackage(int reservationId)
        {
            var package = _packages.Find(r => r.Id == reservationId);
            if (package != null)
            {
                _packages.Remove(package);
                Notify($"Paket uklonjen: {reservationId}");
            }
        }
    }
}
