# 🏖️ Turistička Agencija - .NET Windows Forms Aplikacija

## 📋 Opis projekta

Modern Windows Forms aplikacija za upravljanje turističkim aranžmanima. Aplikacija omogućava agenciji da upravlja klijentima, turističkim paketima i rezervacijama sa elegantnim grafičkim interfejsom.

## ✨ Glavne funkcionalnosti

### 👥 Upravljanje klijentima
- **Dodavanje novih klijenata** sa kompletnim podacima
- **Izmena postojećih klijenata**
- **Pretraga klijenata** po imenu, prezimenu ili broju pasoša
- **Prikaz rezervacija** za određenog klijenta (dupli klik)
- **Enkripcija brojeva pasoša** za sigurnost

### ✈️ Upravljanje turističkim paketima
- **4 tipa paketa:**
  - 🏖️ **Sea** - aranžmani za more (destinacija, smeštaj, prevoz)
  - 🏔️ **Mountain** - planinski aranžmani (destinacija, smeštaj, aktivnosti)
  - 🚌 **Excursion** - ekskurzije (destinacija, prevoz, vodič)
  - 🚢 **Cruise** - krstarenja (brod, ruta, datum polaska)
- **Dodavanje i izmena paketa**
- **Filtriranje po tipu paketa**
- **Prikaz detalja** svakog paketa

### 📋 Upravljanje rezervacijama
- **Kreiranje novih rezervacija** za klijente
- **Otkazivanje rezervacija**
- **Prikaz svih rezervacija** za određenog klijenta
- **Automatsko logovanje** svih promena
- **Statistike rezervacija**

### 💾 Backup sistem
- **Automatski backup** na svakih 24 sata
- **Ručno kreiranje backup-a**
- **Logovanje backup operacija**

## 🗄️ Baza podataka

### Podržane baze:
- **SQLite** (podrazumevano) - `agencija.db`
- **MySQL** - automatsko kreiranje baze i tabela

### Struktura baze:
```sql
-- Klijenti
Clients (Id, FirstName, LastName, PassportNumber, BirthDate, Email, Phone)

-- Turistički paketi
TravelPackages (Id, Name, Price, Type, Destination, Details)

-- Rezervacije
Reservations (Id, ClientId, PackageId, NumPersons, ReservationDate, ExtraServices)
```

## 🎨 Design Patterns

### Creational Patterns:
1. **Singleton** - `AppSettings` (čita config.txt jednom; globalni pristup preko AppSettings.Instance)
2. **Multiton**  - `DatabaseFactory` (po connection stringu vraća/kešira po jednu instancu IDatabaseService; Factory deo bira SQLiteDatabaseService ili MySQLDatabaseService)
3. **Builder** - `ReservationBuilder.cs` (pravljenje rezervacije korak po korak)

### Structural Patterns:
1. **Decorator** - `LoggingBackupService` (dodavanje logovanja backup-u)
2. **Facade** - `ReservationManager.cs` (jednostavan interfejs za sve operacije)

### Behavioral Patterns:
1. **Command** - `CommandPattern.cs`, `AddReservationCommand`, `RemoveReservationCommand` (undo/execute funkcionalnosti)
2. **Observer** - `ObserverPattern.cs`, `ReservationSubject` (praćenje promena rezervacija)
3. **Strategy** - `ReservationManager.cs`, `IDatabaseService.cs`, `MySQLDatabaseService.cs`, `SQLiteDatabaseService.cs` (aptraktna, konkretne strategije i korisnik)

## 🏢 Facade Patterns

U aplikaciji su implementirane sledeće **fasade** koje pojednostavljuju rad sa različitim domenima i skrivanje složenosti ispod:

| Naziv fasade     | Opis                                    | Glavni fajl                   |
|------------------|-----------------------------------------|------------------------------|
| **ClientFacade** | Upravljanje klijentima (dodavanje, izmena, brisanje, pretraga, rezervacije) | `Patterns/ClientFacade.cs`    |
| **PackageFacade** | Upravljanje turističkim paketima (dodavanje, izmena, keširanje, filtriranje) | `Patterns/PackageFacade.cs`   |
| **ReservationFacade** | Upravljanje rezervacijama (kreiranje, otkazivanje, update, logovanje) | `Patterns/ReservationFacade.cs` |
| **BackupFacade**  | Kreiranje i upravljanje backup-om baze sa logovanjem i porukama | `Patterns/Facade/BackupFacade.cs` |
| **StartupFacade** | Pokretanje aplikacije i učitavanje konfiguracije | `Patterns/StartupFacade.cs`   |

### Kratak opis svake fasade

- **ClientFacade** olakšava rad sa klijentima koristeći interne menadžere i observer pattern za logovanje i notifikacije.
- **PackageFacade** enkapsulira kompleksnost upravljanja paketima i keširanjem.
- **ReservationFacade** omogućava rad sa rezervacijama i obezbeđuje observer funkcionalnosti za logovanje promena.
- **BackupFacade** pruža jednostavan interfejs za kreiranje rezervnih kopija baze podataka sa prikazom poruka korisniku.
- **StartupFacade** upravlja inicijalizacijom aplikacije, učitavanjem konfiguracije i pravljenjem glavnog prozora.

---

Ovaj pattern omogućava jasnu separaciju odgovornosti i pojednostavljuje korišćenje kompleksnih sistema u aplikaciji.



## 🏗️ BUILDER PATTERN - TURISTIČKI PAKETI

### 📋 OPIS:
Builder pattern je implementiran za modularno kreiranje različitih tipova turističkih 
paketa u aplikaciji. Omogućava postepeno kreiranje kompleksnih objekata kroz 
chain-ovanje metoda.

### 🏗️ STRUKTURA BUILDER PATTERN-A

1. **APSTRAKTNI BUILDER (PackageBuilder)**
   - Definiše osnovne metode za sve tipove paketa
   - Metode: SetName(), SetPrice(), Build()

2. **KONKRETNI BUILDERI:**
   - SeaPackageBuilder - za kreiranje morskih paketa
   - MountainPackageBuilder - za kreiranje planinskih paketa  
   - ExcursionPackageBuilder - za kreiranje izletničkih paketa
   - CruisePackageBuilder - za kreiranje krstarenja

3. **DIRECTOR (PackageDirector)**
   - Upravlja procesom kreiranja paketa
   - Sadrži gotove metode za kreiranje standardnih konfiguracija

### 📦 TIPOVI PAKETA I NJIHOVI PARAMETRI

🌊 **SEA PACKAGE (Morski paket):**
   - Name (naziv)
   - Price (cena)
   - Destination (destinacija)
   - Accommodation (smeštaj)
   - Transport (prevoz)

⛰️ **MOUNTAIN PACKAGE (Planinski paket):**
   - Name (naziv)
   - Price (cena)
   - Destination (destinacija)
   - Accommodation (smeštaj)
   - Transport (prevoz)
   - Activities (dodatne aktivnosti)

🚌 **EXCURSION PACKAGE (Izletnički paket):**
   - Name (naziv)
   - Price (cena)
   - Destination (destinacija)
   - Transport (prevoz)
   - Guide (vodič)
   - Duration (trajanje u danima)

🚢 **CRUISE PACKAGE (Krstarenje):**
   - Name (naziv)
   - Price (cena)
   - Ship (brod)
   - Route (ruta)
   - DepartureDate (datum polaska)
   - CabinType (tip kabine)

### 💻 PRIMERI KORIŠĆENJA

1. **DIREKTNO KORIŠĆENJE BUILDER-A:**
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

2. **KORIŠĆENJE DIRECTOR KLASE:**
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

3. **KREIRANJE PAKETA ZA IZMENU:**
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

### 🔧 IMPLEMENTACIJA U FORMI

U PackageForm.cs se koristi u sledećim metodama:

1. **DodajPaket()** - kreiranje novih paketa
2. **IzmeniPaket()** - izmena postojećih paketa

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

### ✅ PREDNOSTI BUILDER PATTERN-A

1. 📖 **ČITLJIVOST:** Kod je jasniji i lakši za razumevanje
2. 🔧 **FLEKSIBILNOST:** Možeš postaviti samo potrebne parametre
3. ✅ **VALIDACIJA:** Možeš dodati validaciju u svaku metodu
4. 🔒 **IMUTABILITY:** Objekat se kreira tek na kraju sa .Build()
5. ♻️ **REUSABILITY:** Director klasa omogućava kreiranje standardnih konfiguracija
6. 🛡️ **TYPE SAFETY:** Svaki builder ima svoje specifične metode
7. 🔗 **METHOD CHAINING:** Lepo chain-ovanje metoda

### 📁 FAJLOVI

- **Patterns/PackageBuilder.cs** - Implementacija Builder pattern-a
- **Forms/PackageForm.cs** - Korišćenje Builder pattern-a u UI-u
- **Models/TravelPackage.cs** - Modeli paketa

### 🎯 ZAKLJUČAK BUILDER PATTERN-A

Builder pattern je uspešno implementiran i omogućava:
- Modularno kreiranje različitih tipova paketa
- Čist i čitljiv kod
- Fleksibilnost u kreiranju objekata
- Lako održavanje i proširivanje

Pattern se koristi u celoj aplikaciji za kreiranje i izmenu turističkih paketa,
što čini kod organizovanijim i lakšim za razumevanje.

## 🚀 Pokretanje aplikacije

### Preduslovi:
- .NET 8.0 SDK
- Windows OS
- MySQL (opciono, za MySQL bazu)

### Pokretanje:
```bash
cd TouristAgencyApp
dotnet run
```

### Konfiguracija:
Uredite `config*.txt` fajl:
```
Naziv agencije
Connection string
```

**Primeri connection string-ova:**
- SQLite: `Data Source=agencija.db;`
- MySQL: `Server=localhost;Database=turisticka_agencija;Uid=root;Pwd=;Port=3306;`

## 🎯 Ključne karakteristike

### Sigurnost:
- ✅ Enkripcija brojeva pasoša (AES-256)
- ✅ Validacija unosa
- ✅ Sigurno čuvanje podataka

### Korisničko iskustvo:
- ✅ Moderan, responzivan dizajn
- ✅ Intuitivan interfejs
- ✅ Tooltip-ovi i pomoć
- ✅ Status bar sa informacijama
- ✅ Gradijenti i hover efekti

### Performanse:
- ✅ Brza pretraga
- ✅ Optimizovane upite
- ✅ Automatsko kreiranje baze
- ✅ Backup optimizacija

## 📁 Struktura projekta

```
TouristAgencyApp/
├── Forms/                 # Windows Forms
│   ├── MainForm.cs       # Glavna forma
│   ├── ClientForm.cs     # Upravljanje klijentima
│   ├── PackageForm.cs    # Upravljanje paketima
│   ├── ReservationForm.cs # Upravljanje rezervacijama
|   └── BackupForm.cs     # Upravljanje rezervnim kopijama
├── Models/               # Modeli podataka
│   ├── Client.cs
│   ├── TravelPackage.cs
│   └── Reservation.cs
├── Services/             # Servisi
│   ├── IDatabaseService.cs
│   ├── SQLiteDatabaseService.cs
│   ├── MySQLDatabaseService.cs
│   └── ConfigManager.cs
├── Patterns/             # Design Patterns
|   ├── AppSettings.cs 
│   ├── ClientManager.cs
│   ├── CommandPattern.cs
│   ├── IBackupService.cs
│   ├── ObserverPattern.cs
│   ├── PackageFactory.cs
│   ├── PackageManager.cs
│   ├── ReservationBuilder.cs
│   └── ReservationManager.cs
├── Utils/               # Pomoćne klase
│   └── EncryptionService.cs
└── config.txt          # Konfiguracija
```

## 🔧 Tehnologije

- **.NET 8.0** - Framework
- **Windows Forms** - GUI
- **SQLite** - Lokalna baza
- **MySQL** - Server baza
- **AES-256** - Enkripcija
- **JSON** - Serijalizacija podataka

## 📊 Statistike projekta

- **Linija koda:** ~2000+
- **Design patterns:** 8 implementiranih
- **Forme:** 5 glavnih formi
- **Modeli:** 3 osnovna modela
- **Servisi:** 4 servisa

## 🎉 Zaključak

Aplikacija predstavlja kompletan sistem za upravljanje turističkom agencijom sa modernim dizajnom, sigurnošću i skalabilnošću. Implementirani su svi zahtevani design patterns i funkcionalnosti iz zadatka.