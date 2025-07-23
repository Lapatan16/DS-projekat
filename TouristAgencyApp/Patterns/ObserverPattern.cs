using System;
using System.Collections.Generic;
using TouristAgencyApp.Models;

namespace TouristAgencyApp.Patterns
{

    public interface IObserver
    {
        void Update(string message);
    }

    public interface ISubject
    {
        void Attach(IObserver observer);
        void Detach(IObserver observer);
        void Notify(string message);
    }

    public class ReservationSubject : ISubject
    {
        private readonly List<IObserver> _observers = new();
        private readonly List<Reservation> _reservations = new();
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

        public void AddReservation(Reservation reservation, int id)
        {
            _reservations.Add(reservation);
            //MessageBox.Show(reservation.Id.ToString());
            Notify($"Nova rezervacija dodana: KorisnikId:{reservation.ClientId} | PaketId: {reservation.PackageId} | ReservacijaId: {id}");
        }
        public void UpdateReservation(int id)
        {
            _reservations.Add(new Reservation { Id = id });
            Notify($"Rezervacija sa id = {id} je azurirana");
        }
        public void RemoveReservation(int reservationId)
        {
            var reservation = _reservations.Find(r => r.Id == reservationId);
            if (reservation != null)
            {
                _reservations.Remove(reservation);
                Notify($"Rezervacija uklonjena: {reservationId}");
            }
        }
    }
    public class ClientSubject : ISubject
    {
        private readonly List<IObserver> _observers = new();
        private readonly List<Client> _clients = new();
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

        public void AddClient(Client client, int id)
        {
            _clients.Add(client);
            Notify($"Novi klijent dodat: email: {client.Email} | id: {id} ");
        }
        public void UpdateClient(Client client)
        {
            _clients.Add(client);
            Notify($"Klijent sa id = {client.Id} je azuriran");
        }
        public void RemoveClient(int clientId)
        {
            var client = _clients.Find(c => c.Id == clientId);
            if (client != null)
            {
                _clients.Remove(client);
                Notify($"Klijent uklonjen: {clientId}");
            }
        }
    }
    public class ReservationLogger : IObserver
    {
        private readonly string _logFile;

        public ReservationLogger(string logFile = "reservations.log")
        {
            _logFile = logFile;
        }

        public void Update(string message)
        {
            var logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";
            File.AppendAllText(_logFile, logEntry + Environment.NewLine);
        }
    }
    public class ClientLogger : IObserver
    {
        private readonly string _logFile;

        public ClientLogger(string logFile = "clients.log")
        {
            _logFile = logFile;
        }

        public void Update(string message)
        {
            var logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";
            File.AppendAllText(_logFile, logEntry + Environment.NewLine);
        }
    }

    public class ReservationNotifier : IObserver
    {
        public void Update(string message)
        {
            Console.WriteLine($"NOTIFIKACIJA: {message}");
        }
    }
    public class ClientNotifier : IObserver
    {
        public void Update(string message)
        {
            Console.WriteLine($"NOTIFIKACIJA: {message}");
        }
    }

    public class ReservationStatistics : IObserver
    {
        private int _totalReservations = 0;
        private int _cancelledReservations = 0;

        public void Update(string message)
        {
            if (message.Contains("Nova rezervacija"))
                _totalReservations++;
            else if (message.Contains("Rezervacija uklonjena"))
                _cancelledReservations++;

            Console.WriteLine($"STATISTIKA: Ukupno rezervacija: {_totalReservations}, Otkazano: {_cancelledReservations}");
        }
    }
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
            //MessageBox.Show(reservation.Id.ToString());
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
    public class PackageLogger : IObserver
    {
        private readonly string _logFile;

        public PackageLogger(string logFile = "packages.log")
        {
            _logFile = logFile;
        }

        public void Update(string message)
        {
            var logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";
            File.AppendAllText(_logFile, logEntry + Environment.NewLine);
        }
    }
    public class PackageNotifier : IObserver
    {
        public void Update(string message)
        {
            Console.WriteLine($"NOTIFIKACIJA: {message}");
        }
    }
} 