using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouristAgencyApp.Models;
using TouristAgencyApp.Utils;

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
        public void SubscribeToManager(PackageManager manager)
        {
            manager.PackageChanged += OnPackageChanged;
        }
        public void UnsubscribeToManager(PackageManager manager)
        {
            manager.PackageChanged -= OnPackageChanged;
        }
        private void OnPackageChanged(object? sender, PackageChangedEventArgs e)
        {
            string message = e.Action switch
            {
                "Added" => $"Novi paket dodat: {e.Package.Name} | id: {e.Id}",
                "Updated" => $"Paket sa id = {e.Package.Id} je azuriran",
                _ => "Nepoznata akcija"
            };

            foreach (var observer in _observers)
            {
                observer.Update(message);
            }
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

        //public void AddPackage(TravelPackage package, int id)
        //{
        //    _packages.Add(package);
        //    Notify($"Novi paket dodat: {package.Name} | id: {id}");
        //}
        //public void UpdatePackage(TravelPackage package)
        //{
        //    _packages.Add(package);
        //    Notify($"Paket sa id = {package.Id} je azuriran");
        //}
        //public void RemovePackage(int reservationId)
        //{
        //    var package = _packages.Find(r => r.Id == reservationId);
        //    if (package != null)
        //    {
        //        _packages.Remove(package);
        //        Notify($"Paket uklonjen: {reservationId}");
        //    }
        //}
    }
}
