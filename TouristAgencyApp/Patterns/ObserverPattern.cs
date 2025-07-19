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

        public void AddReservation(Reservation reservation)
        {
            _reservations.Add(reservation);
            Notify($"Nova rezervacija dodana: {reservation.Id}");
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

    public class ReservationNotifier : IObserver
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
} 