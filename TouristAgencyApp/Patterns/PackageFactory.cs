using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouristAgencyApp.Models;

namespace TouristAgencyApp.Patterns
{
    public abstract class PackageFactory
    {
        public abstract TravelPackage CreatePackage();
    }

    public class SeaPackageFactory : PackageFactory
    {
        public override TravelPackage CreatePackage()
        {
            return new SeaPackage { Type = "Sea" };
        }
    }

    public class MountainPackageFactory : PackageFactory
    {
        public override TravelPackage CreatePackage()
        {
            return new MountainPackage { Type = "Mountain" };
        }
    }

    public class ExcursionPackageFactory : PackageFactory
    {
        public override TravelPackage CreatePackage()
        {
            return new ExcursionPackage { Type = "Excursion" };
        }
    }

    public class CruisePackageFactory : PackageFactory
    {
        public override TravelPackage CreatePackage()
        {
            return new CruisePackage { Type = "Cruise" };
        }
    }
}
