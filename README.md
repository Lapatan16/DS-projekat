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
TravelPackages (Id, Name, Price, Type, Details)

-- Rezervacije
Reservations (Id, ClientId, PackageId, NumPersons, ReservationDate, ExtraServices)
```

## 🎨 Design Patterns

### Creational Patterns:
1. **Singleton** - `ConfigManager` (konfiguracija aplikacije)
2. **Factory** - `PackageFactory` (kreiranje različitih tipova paketa)

### Structural Patterns:
1. **Decorator** - `LoggingBackupService` (dodavanje logovanja backup-u)
2. **Facade** - `AgencyFacade` (jednostavan interfejs za sve operacije)

### Behavioral Patterns:
1. **Command** - `AddReservationCommand`, `RemoveReservationCommand` (undo/redo funkcionalnost)
2. **Observer** - `ReservationSubject` (praćenje promena rezervacija)

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
- ✅ Modern, responzivan dizajn
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
│   └── ReservationForm.cs # Upravljanje rezervacijama
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
│   ├── CommandPattern.cs
│   ├── ObserverPattern.cs
│   ├── PackageFactory.cs
│   ├── IBackupService.cs
│   └── AgencyFacade.cs
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
- **Design patterns:** 6 implementiranih
- **Forme:** 4 glavne forme
- **Modeli:** 3 osnovna modela
- **Servisi:** 4 servisa
- **Patterns:** 5 pattern implementacija

## 🎉 Zaključak

Aplikacija predstavlja kompletan sistem za upravljanje turističkom agencijom sa modernim dizajnom, sigurnošću i skalabilnošću. Implementirani su svi zahtevani design patterns i funkcionalnosti iz zadatka.