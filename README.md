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
1. **Singleton** - `ConfigManager` (konfiguracija aplikacije, povezivanje sa bazom, thread-safe je koristi se Lazy<T> klasa)
2. **Factory** - `PackageFactory` (kreiranje različitih tipova paketa)
3. **Builder** - `ReservationBuilder.cs` (pravljenje rezervacije korak po korak)

### Structural Patterns:
1. **Decorator** - `LoggingBackupService` (dodavanje logovanja backup-u)
2. **Facade** - `ReservationManager.cs` (jednostavan interfejs za sve operacije)

### Behavioral Patterns:
1. **Command** - `CommandPattern.cs`, `AddReservationCommand`, `RemoveReservationCommand` (undo/execute funkcionalnosti)
2. **Observer** - `ObserverPattern.cs`, `ReservationSubject` (praćenje promena rezervacija)
3. **Strategy** - `ReservationManager.cs`, `IDatabaseService.cs`, `MySQLDatabaseService.cs`, `SQLiteDatabaseService.cs` (aptraktna, konkretne strategije i korisnik)

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
Uredite `config.txt` fajl:
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
