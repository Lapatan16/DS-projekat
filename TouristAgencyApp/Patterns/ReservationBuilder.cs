using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouristAgencyApp.Models;

namespace TouristAgencyApp.Patterns
{
    class ReservationBuilder
    {
        private Reservation _reservation = new Reservation();

        public ReservationBuilder SetClient(Client client)
        {
            _reservation.ClientId = client.Id;
            return this;
        }
        public ReservationBuilder SetPackage(TravelPackage travelPackage)
        {
            _reservation.PackageId = travelPackage.Id;
            _reservation.PackageName = travelPackage.Name;
            return this;
        }
        public ReservationBuilder SetNumPersons(int num)
        {
            _reservation.NumPersons = num;
            return this;
        }
        public ReservationBuilder SetExtraServices(string services)
        {
            _reservation.ExtraServices = services;
            return this;
        }
        public ReservationBuilder SetReservationDate(DateTime date)
        {
            _reservation.ReservationDate = date;
            return this;
        }
        public Reservation Build()
        {
            return _reservation;
        }
    }
}
