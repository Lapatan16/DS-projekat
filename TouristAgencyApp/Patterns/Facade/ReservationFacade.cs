using System.Collections.Generic;
using TouristAgencyApp.Models;
using TouristAgencyApp.Patterns.Observer.ReservationObserver;
using TouristAgencyApp.Services;

namespace TouristAgencyApp.Patterns
{
    public class ReservationFacade
    {
        private readonly ReservationManager _manager;
        private readonly IDatabaseService _db;
        private readonly ReservationSubject _subject;

        public ReservationFacade(IDatabaseService dbService)
        {
            _db = dbService;
            _manager = new ReservationManager(dbService);
            _subject = new ReservationSubject();
            _subject.Attach(new ReservationLogger());
            _subject.Attach(new ReservationNotifier());
            
            _subject.SubscribeToManager(_manager);
        }

        public List<Reservation> GetReservationsByClient(int clientId)
        {
            var list = _db.GetReservationsByClient(clientId);
            var packages = _db.GetAllPackages();

            foreach (var r in list)
            {
                var pkg = packages.FirstOrDefault(p => p.Id == r.PackageId);
                r.PackageName = pkg != null ? pkg.Name : "(nepoznato)";
                r.Destination = pkg != null ? pkg.Destination : "(nepoznato)";
            }

            return list;
        }

        public List<TravelPackage> GetAllPackages()
        {
            return _db.GetAllPackages();
        }

        public int AddReservation(Reservation reservation)
        {
            int id = _manager.AddReservation(reservation);
            //_subject.AddReservation(reservation, id);
            return id;
        }

        public void UpdateReservation(int reservationId, int numPersons, string extra)
        {
            _manager.UpdateReservation(reservationId, numPersons, extra);
            //_subject.UpdateReservation(reservationId);
        }

        public void RemoveReservation(int reservationId)
        {
            _manager.RemoveReservation(reservationId);
            //_subject.RemoveReservation(reservationId);
        }

        public void Undo()
        {
            _manager.UndoLastAction();
        }

        public void Redo()
        {
            _manager.RedoLastAction();
        }

        public List<Client> GetAllClients()
        {
            return _db.GetAllClients();
        }
    }
}
