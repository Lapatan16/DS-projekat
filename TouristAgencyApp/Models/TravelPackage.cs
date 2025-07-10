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

    public class SeaPackage : TravelPackage
    {
        public string Destination { get; set; } = "";
        public string Accommodation { get; set; } = "";
        public string Transport { get; set; } = "";

        public override string ToString()
        {
            return $"Destinacija: {Destination}, Smeštaj: {Accommodation}, Prevoz: {Transport}";
        }
    }

    public class MountainPackage : TravelPackage
    {
        public string Destination { get; set; } = "";
        public string Accommodation { get; set; } = "";
        public string Transport { get; set; } = "";
        public string Activities { get; set; } = "";

        public override string ToString()
        {
            return $"Destinacija: {Destination}, Smeštaj: {Accommodation}, Prevoz: {Transport}, Aktivnosti: {Activities}";
        }
    }

    public class ExcursionPackage : TravelPackage
    {
        public string Destination { get; set; } = "";
        public string Transport { get; set; } = "";
        public string Guide { get; set; } = "";
        public int Duration { get; set; }

        public override string ToString()
        {
            return $"Destinacija: {Destination}, Prevoz: {Transport}, Vodič: {Guide}, Trajanje: {Duration} dana";
        }
    }

    public class CruisePackage : TravelPackage
    {
        public string Ship { get; set; } = "";
        public string Route { get; set; } = "";
        public DateTime DepartureDate { get; set; }
        public string CabinType { get; set; } = "";

        public override string ToString()
        {
            return $"Brod: {Ship}, Ruta: {Route}, Polazak: {DepartureDate:dd.MM.yyyy}, Kabina: {CabinType}";
        }
    }
}
