===============================================================================
                    BUILDER PATTERN - TURISTIČKI PAKETI
===============================================================================

📋 OPIS:
Builder pattern je implementiran za modularno kreiranje različitih tipova turističkih 
paketa u aplikaciji. Omogućava postepeno kreiranje kompleksnih objekata kroz 
chain-ovanje metoda.

===============================================================================
🏗️ STRUKTURA BUILDER PATTERN-A
===============================================================================

1. APSTRAKTNI BUILDER (PackageBuilder)
   - Definiše osnovne metode za sve tipove paketa
   - Metode: SetName(), SetPrice(), Build()

2. KONKRETNI BUILDERI:
   - SeaPackageBuilder - za kreiranje morskih paketa
   - MountainPackageBuilder - za kreiranje planinskih paketa  
   - ExcursionPackageBuilder - za kreiranje izletničkih paketa
   - CruisePackageBuilder - za kreiranje krstarenja

3. DIRECTOR (PackageDirector)
   - Upravlja procesom kreiranja paketa
   - Sadrži gotove metode za kreiranje standardnih konfiguracija

===============================================================================
📦 TIPOVI PAKETA I NJIHOVI PARAMETRI
===============================================================================

🌊 SEA PACKAGE (Morski paket):
   - Name (naziv)
   - Price (cena)
   - Destination (destinacija)
   - Accommodation (smeštaj)
   - Transport (prevoz)

⛰️ MOUNTAIN PACKAGE (Planinski paket):
   - Name (naziv)
   - Price (cena)
   - Destination (destinacija)
   - Accommodation (smeštaj)
   - Transport (prevoz)
   - Activities (dodatne aktivnosti)

🚌 EXCURSION PACKAGE (Izletnički paket):
   - Name (naziv)
   - Price (cena)
   - Destination (destinacija)
   - Transport (prevoz)
   - Guide (vodič)
   - Duration (trajanje u danima)

🚢 CRUISE PACKAGE (Krstarenje):
   - Name (naziv)
   - Price (cena)
   - Ship (brod)
   - Route (ruta)
   - DepartureDate (datum polaska)
   - CabinType (tip kabine)

===============================================================================
💻 PRIMERI KORIŠĆENJA
===============================================================================

1. DIREKTNO KORIŠĆENJE BUILDER-A:
```csharp
// Kreiranje morskog paketa
var seaPackage = new SeaPackageBuilder()
    .SetName("Letovanje u Grčkoj")
    .SetPrice(500.00m)
    .SetDestination("Santorini")
    .SetAccommodation("Hotel 4*")
    .SetTransport("Avion")
    .Build();

// Kreiranje planinskog paketa
var mountainPackage = new MountainPackageBuilder()
    .SetName("Planinarenje u Alpima")
    .SetPrice(800.00m)
    .SetDestination("Zermatt")
    .SetAccommodation("Planinski hotel")
    .SetTransport("Autobus")
    .SetActivities("Skijanje, šetanje, restoran")
    .Build();
```

2. KORIŠĆENJE DIRECTOR KLASE:
```csharp
// Kreiranje paketa preko Director-a
var seaPackage = PackageDirector.CreateSeaPackage(
    "Letovanje u Grčkoj", 
    500.00m, 
    "Santorini", 
    "Hotel 4*", 
    "Avion"
);

var mountainPackage = PackageDirector.CreateMountainPackage(
    "Planinarenje u Alpima",
    800.00m,
    "Zermatt",
    "Planinski hotel",
    "Autobus",
    "Skijanje, šetanje, restoran"
);
```

3. KREIRANJE PAKETA ZA IZMENU:
```csharp
// Kreiranje paketa sa ID-om za izmenu
var updatedPackage = PackageDirector.CreateSeaPackageForUpdate(
    existingPackage.Id,
    "Novi naziv paketa",
    600.00m,
    "Nova destinacija",
    "Novi smeštaj",
    "Novi prevoz"
);
```

===============================================================================
🔧 IMPLEMENTACIJA U FORMI
===============================================================================

U PackageForm.cs se koristi u sledećim metodama:

1. DodajPaket() - kreiranje novih paketa
2. IzmeniPaket() - izmena postojećih paketa

Primer iz koda:
```csharp
btnSave.Click += (ss, ee) =>
{
    TravelPackage pkg = cbType.SelectedItem.ToString() switch
    {
        "Sea" => PackageDirector.CreateSeaPackage(
            txtName.Text, 
            numPrice.Value, 
            txtDestination.Text, 
            txtAcc.Text, 
            txtTransport.Text),
        "Mountain" => PackageDirector.CreateMountainPackage(
            txtName.Text, 
            numPrice.Value, 
            txtDestination.Text, 
            txtAcc.Text, 
            txtTransport.Text, 
            txtActivities.Text),
        // ... ostali tipovi
        _ => throw new Exception("Unknown package type")
    };
    
    int id = _packageManager.AddPackage(pkg);
    // ...
};
```

===============================================================================
✅ PREDNOSTI BUILDER PATTERN-A
===============================================================================

1. 📖 ČITLJIVOST: Kod je jasniji i lakši za razumevanje
2. 🔧 FLEKSIBILNOST: Možeš postaviti samo potrebne parametre
3. ✅ VALIDACIJA: Možeš dodati validaciju u svaku metodu
4. 🔒 IMUTABILITY: Objekat se kreira tek na kraju sa .Build()
5. ♻️ REUSABILITY: Director klasa omogućava kreiranje standardnih konfiguracija
6. 🛡️ TYPE SAFETY: Svaki builder ima svoje specifične metode
7. 🔗 METHOD CHAINING: Lepo chain-ovanje metoda

===============================================================================
📁 FAJLOVI
===============================================================================

- Patterns/PackageBuilder.cs - Implementacija Builder pattern-a
- Forms/PackageForm.cs - Korišćenje Builder pattern-a u UI-u
- Models/TravelPackage.cs - Modeli paketa

===============================================================================
🎯 ZAKLJUČAK
===============================================================================

Builder pattern je uspešno implementiran i omogućava:
- Modularno kreiranje različitih tipova paketa
- Čist i čitljiv kod
- Fleksibilnost u kreiranju objekata
- Lako održavanje i proširivanje

Pattern se koristi u celoj aplikaciji za kreiranje i izmenu turističkih paketa,
što čini kod organizovanijim i lakšim za razumevanje. 