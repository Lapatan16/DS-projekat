using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouristAgencyApp.Models;
using TouristAgencyApp.Services;

namespace TouristAgencyApp.Patterns
{
    public class AgencyFacade
    {
        private readonly IDatabaseService _db;
        public AgencyFacade(IDatabaseService dbService) { _db = dbService; }
        public List<Client> GetAllClients() => _db.GetAllClients();
        public void AddClient(Client c) => _db.AddClient(c);
        public List<TravelPackage> GetAllPackages() => _db.GetAllPackages();
        public void AddPackage(TravelPackage p) => _db.AddPackage(p);
        public List<Reservation> GetReservationsByClient(int id) => _db.GetReservationsByClient(id);
        public void AddReservation(Reservation r) => _db.AddReservation(r);
    }
}
