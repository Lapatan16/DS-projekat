using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouristAgencyApp.Models;

namespace TouristAgencyApp.Patterns.Observer.ReservationObserver
{
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
}
