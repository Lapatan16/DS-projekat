using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using TouristAgencyApp.Patterns.Memento;
using TouristAgencyApp.Patterns.Memento.PackageMemento;

namespace TouristAgencyApp.Models
{
    public abstract class TravelPackage
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public decimal Price { get; set; }
        public string Type { get; set; } = "";
        public string Details { get; set; } = "";
        public abstract TravelPackageMemento CreateMemento();
    }

    public class SeaPackage : TravelPackage
    {
        public string Destination { get; set; } = "";
        public string Accommodation { get; set; } = "";
        public string Transport { get; set; } = "";
        public override TravelPackageMemento CreateMemento()
        {
            return new SeaPackageMemento(Id, Name, Price, Type, Destination, Accommodation, Transport, Details);
        }

        public void Restore(SeaPackageMemento memento)
        {
            Id = memento.Id;
            Name = memento.Name;
            Price = memento.Price;
            Type = memento.Type;
            Destination = memento.Destination;
            Accommodation = memento.Accommodation;
            Transport = memento.Transport;
            Details = memento.Details;
        }
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
        public override TravelPackageMemento CreateMemento()
        {
            return new MountainPackageMemento(Id, Name, Price, Type, Destination, Accommodation, Transport, Activities,Details);
        }

        public void Restore(MountainPackageMemento memento)
        {
            Id = memento.Id;
            Name = memento.Name;
            Price = memento.Price;
            Type = memento.Type;
            Destination = memento.Destination;
            Accommodation = memento.Accommodation;
            Transport = memento.Transport;
            Details = memento.Details;
            Activities = memento.Activities;
        }
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
        public override TravelPackageMemento CreateMemento()
        {
            return new ExcursionPackageMemento(Id, Name, Price, Type, Destination, Details, Transport, Guide, Duration);
        }

        public void Restore(ExcursionPackageMemento memento)
        {
            Id = memento.Id;
            Name = memento.Name;
            Price = memento.Price;
            Type = memento.Type;
            Destination = memento.Destination;
            Guide = memento.Guide;
            Transport = memento.Transport;
            Details = memento.Details;
            Duration = memento.Duration;
        }
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
        public override TravelPackageMemento CreateMemento()
        {
            return new CruisePackageMemento(Id, Name, Price, Type, Details, Ship, Route, DepartureDate, CabinType);
        }
        public void Restore(CruisePackageMemento memento)
        {
            Id = memento.Id;
            Name = memento.Name;
            Price = memento.Price;
            Type = memento.Type;
            Ship = memento.Ship;
            Route = memento.Route;
            DepartureDate = memento.DepartureDate;
            CabinType = memento.CabinType;
        }
    }
}
