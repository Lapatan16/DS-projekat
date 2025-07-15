using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TouristAgencyApp.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int PackageId { get; set; }
        public int NumPersons { get; set; }
        public DateTime ReservationDate { get; set; }
        public string ExtraServices { get; set; } = "";
        public string PackageName { get; set; } = "";
    }
}
