using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TouristAgencyApp.Models;
using TouristAgencyApp.Utils;

namespace TouristAgencyApp.Services
{
    public class SQLiteDatabaseService : IDatabaseService
    {
        private readonly string _connectionString;

        public SQLiteDatabaseService(string connectionString)
        {
            _connectionString = connectionString;
            EnsureDatabase();
        }

        // Pravljenje baze i tabela, samo prvi put
        private void EnsureDatabase()
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
CREATE TABLE IF NOT EXISTS Clients (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    FirstName TEXT,
    LastName TEXT,
    PassportNumber TEXT,
    BirthDate TEXT,
    Email TEXT,
    Phone TEXT
);
CREATE TABLE IF NOT EXISTS TravelPackages (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT,
    Price REAL,
    Type TEXT,
    Details TEXT
);
CREATE TABLE IF NOT EXISTS Reservations (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    ClientId INTEGER,
    PackageId INTEGER,
    NumPersons INTEGER,
    ReservationDate TEXT,
    ExtraServices TEXT
);";
            cmd.ExecuteNonQuery();
        }

        public List<Client> GetAllClients()
        {
            var result = new List<Client>();
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Clients";
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result.Add(new Client
                {
                    Id = reader.GetInt32(0),
                    FirstName = reader.GetString(1),
                    LastName = reader.GetString(2),
                    PassportNumber = EncryptionService.Decrypt(reader.GetString(3)),
                    BirthDate = DateTime.Parse(reader.GetString(4)),
                    Email = reader.GetString(5),
                    Phone = reader.GetString(6)
                });
            }
            return result;
        }

        public void AddClient(Client client)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO Clients (FirstName, LastName, PassportNumber, BirthDate, Email, Phone)
VALUES ($fn, $ln, $pn, $bd, $em, $ph)";
            cmd.Parameters.AddWithValue("$fn", client.FirstName);
            cmd.Parameters.AddWithValue("$ln", client.LastName);
            cmd.Parameters.AddWithValue("$pn", EncryptionService.Encrypt(client.PassportNumber));
            cmd.Parameters.AddWithValue("$bd", client.BirthDate.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("$em", client.Email);
            cmd.Parameters.AddWithValue("$ph", client.Phone);
            cmd.ExecuteNonQuery();
        }

        public void UpdateClient(Client client)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = @"UPDATE Clients SET FirstName=$fn, LastName=$ln, PassportNumber=$pn, BirthDate=$bd, Email=$em, Phone=$ph WHERE Id=$id";
            cmd.Parameters.AddWithValue("$fn", client.FirstName);
            cmd.Parameters.AddWithValue("$ln", client.LastName);
            cmd.Parameters.AddWithValue("$pn", EncryptionService.Encrypt(client.PassportNumber));
            cmd.Parameters.AddWithValue("$bd", client.BirthDate.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("$em", client.Email);
            cmd.Parameters.AddWithValue("$ph", client.Phone);
            cmd.Parameters.AddWithValue("$id", client.Id);
            cmd.ExecuteNonQuery();
        }
    
    public List<TravelPackage> GetAllPackages()
        {
            var result = new List<TravelPackage>();
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM TravelPackages";
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string name = reader.GetString(1);
                decimal price = reader.GetDecimal(2);
                string type = reader.GetString(3);
                string details = reader.GetString(4);
                //MessageBox.Show("Detalji: == =" + details);
                TravelPackage pkg = type switch
                {
                    "Sea" => JsonSerializer.Deserialize<SeaPackage>(details) ?? new SeaPackage(),
                    "Mountain" => JsonSerializer.Deserialize<MountainPackage>(details) ?? new MountainPackage(),
                    "Excursion" => JsonSerializer.Deserialize<ExcursionPackage>(details) ?? new ExcursionPackage(),
                    "Cruise" => JsonSerializer.Deserialize<CruisePackage>(details) ?? new CruisePackage(),
                    _ => new SeaPackage(),
                };

                pkg.Id = id;
                pkg.Name = name;
                pkg.Price = price;
                pkg.Type = type;
                pkg.Details = pkg.ToString();
                //MessageBox.Show("LALALA" + pkg.ToString());
                result.Add(pkg);
            }
            return result;
        }

        public void AddPackage(TravelPackage package)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO TravelPackages (Name, Price, Type, Details)
VALUES ($n, $p, $t, $d)";
            cmd.Parameters.AddWithValue("$n", package.Name);
            cmd.Parameters.AddWithValue("$p", package.Price);
            cmd.Parameters.AddWithValue("$t", package.Type);
            // Ovde koristi JsonSerializer!
            //MessageBox.Show("Test213:= " + package.Details);
            if(package is ExcursionPackage) cmd.Parameters.AddWithValue("$d", System.Text.Json.JsonSerializer.Serialize((ExcursionPackage)package));
            if(package is SeaPackage) cmd.Parameters.AddWithValue("$d", System.Text.Json.JsonSerializer.Serialize((SeaPackage)package));
            if(package is MountainPackage) cmd.Parameters.AddWithValue("$d", System.Text.Json.JsonSerializer.Serialize((MountainPackage)package));
            if(package is CruisePackage) cmd.Parameters.AddWithValue("$d", System.Text.Json.JsonSerializer.Serialize((CruisePackage)package));

            cmd.ExecuteNonQuery();
            //MessageBox.Show(System.Text.Json.JsonSerializer.Serialize(package), "Šta šaljem u bazu kao Details");
        }

        public void UpdatePackage(TravelPackage package)
        {

            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = @"UPDATE TravelPackages SET Name=$n, Price=$p, Type=$t, Details=$d WHERE Id=$id";
            cmd.Parameters.AddWithValue("$n", package.Name);
            cmd.Parameters.AddWithValue("$p", package.Price);
            cmd.Parameters.AddWithValue("$t", package.Type);
            cmd.Parameters.AddWithValue("$d", JsonSerializer.Serialize(package));
            cmd.Parameters.AddWithValue("$id", package.Id);
            cmd.ExecuteNonQuery();
        }

        public void RemovePackage(int packageId)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = "DELETE FROM TravelPackages WHERE Id=$id";
            cmd.Parameters.AddWithValue("$id", packageId);
            cmd.ExecuteNonQuery();
        }

        public List<Reservation> GetReservationsByClient(int clientId)
        {
            var result = new List<Reservation>();
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Reservations WHERE ClientId = $cid";
            cmd.Parameters.AddWithValue("$cid", clientId);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                result.Add(new Reservation
                {
                    Id = reader.GetInt32(0),
                    ClientId = reader.GetInt32(1),
                    PackageId = reader.GetInt32(2),
                    NumPersons = reader.GetInt32(3),
                    ReservationDate = DateTime.Parse(reader.GetString(4)),
                    ExtraServices = reader.GetString(5)
                });
            }
            return result;
        }

        public void AddReservation(Reservation reservation)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO Reservations (ClientId, PackageId, NumPersons, ReservationDate, ExtraServices)
VALUES ($cid, $pid, $num, $date, $extra)";
            cmd.Parameters.AddWithValue("$cid", reservation.ClientId);
            cmd.Parameters.AddWithValue("$pid", reservation.PackageId);
            cmd.Parameters.AddWithValue("$num", reservation.NumPersons);
            cmd.Parameters.AddWithValue("$date", reservation.ReservationDate.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("$extra", reservation.ExtraServices);
            cmd.ExecuteNonQuery();
        }

        public void RemoveReservation(int reservationId)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = "DELETE FROM Reservations WHERE Id = $id";
            cmd.Parameters.AddWithValue("$id", reservationId);
            cmd.ExecuteNonQuery();
        }
    }
}