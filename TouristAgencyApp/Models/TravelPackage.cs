using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TouristAgencyApp.Models
{
    public abstract class TravelPackage
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public decimal Price { get; set; }
        public string Type { get; set; } = "";
        public string Details { get; set; } = ""; // JSON za specifična polja
    }

    // More
    public class SeaPackage : TravelPackage
    {
        public string Destination { get; set; } = "";
        public string Accommodation { get; set; } = "";
        public string Transport { get; set; } = "";
    }

    // Planine
    public class MountainPackage : TravelPackage
    {
        public string Destination { get; set; } = "";
        public string Accommodation { get; set; } = "";
        public string Transport { get; set; } = "";
        public string Activities { get; set; } = "";
    }

    // Ekskurzija
    public class ExcursionPackage : TravelPackage
    {
        public string Destination { get; set; } = "";
        public string Transport { get; set; } = "";
        public string Guide { get; set; } = "";
        public int Duration { get; set; }
    }

    // Krstarenje
    public class CruisePackage : TravelPackage
    {
        public string Ship { get; set; } = "";
        public string Route { get; set; } = "";
        public DateTime DepartureDate { get; set; }
        public string CabinType { get; set; } = "";
    }
}
