using Org.BouncyCastle.Pqc.Crypto.Picnic;
using System;
using System.Collections.Generic;
using TouristAgencyApp.Models;
using TouristAgencyApp.Services;

namespace TouristAgencyApp.Patterns
{
    // Command Pattern - Behavioral
    public interface ICommand
    {
        void Execute();
        void Undo();
    }

    public class AddReservationCommand : ICommand
    {
        private readonly IDatabaseService _db;
        private readonly Reservation _reservation;
        private int _reservationId;
        public int ReservationId => _reservationId;

        public AddReservationCommand(IDatabaseService db, Reservation reservation)
        {
            _db = db;
            _reservation = reservation;
        }

        public void Execute()
        {
            int id = _db.AddReservation(_reservation);
           
            _reservationId = id; 
        }

        public void Undo()
        {
            if (_reservationId > 0)
                _db.RemoveReservation(_reservationId);
        }
    }
    public class UpdateClientCommand : ICommand
    {
        private readonly IDatabaseService _db;
        private readonly Client _client;
        private readonly Client _updated;
        private int clientId;
        public UpdateClientCommand(IDatabaseService db, Client client)
        {
            List<Client> clients = db.GetAllClients();// Ovde bi bilo bolje da postoji funkcija za dohvatanje klijenta po Id;
            foreach (Client c in clients)
            {
                if (c.Id == client.Id) _client = c; // Cuvamo stare podatke o klijentu
            }
            _updated = client;
            _db = db;
        }
        public void Execute()
        {
            _db.UpdateClient(_updated);
        }

        public void Undo()
        {
            _db.UpdateClient(_client);
        }
    }
    public class AddClientCommand : ICommand
    {
        private readonly IDatabaseService _db;
        private readonly Client _client;
        private int _clientId;
        public int ClientId => _clientId;

        public AddClientCommand(IDatabaseService db, Client client)
        {
            _db = db;
            _client = client;
        }

        public void Execute()
        {
            int id = _db.AddClient(_client);

            _clientId = id; 
        }

        public void Undo()
        {
            if (_clientId > 0)
                _db.RemoveClient(_clientId);
        }
    }
    public class AddPackageCommand : ICommand
    {
        private readonly IDatabaseService _db;
        private readonly TravelPackage _package;
        private int _packageId;
        public int PackageId => _packageId;

        public AddPackageCommand(IDatabaseService db, TravelPackage package)
        {
            _db = db;
            _package = package;
        }

        public void Execute()
        {
            int id = _db.AddPackage(_package);
           
            _packageId= id; 
        }

        public void Undo()
        {
            if (_packageId> 0)
                _db.RemovePackage(_packageId);
        }
    }

    public class UpdatePackageCommand : ICommand
    {
        private readonly IDatabaseService _db;
        private readonly TravelPackage _package;
        private readonly TravelPackage _updated;
        public UpdatePackageCommand(IDatabaseService db, TravelPackage package)
        {
            _db = db;
            _updated = package;
            List<TravelPackage> packages = _db.GetAllPackages();
            foreach(TravelPackage pkg in packages)
            {
                if(pkg.Id == package.Id)
                {
                    _package = pkg;
                    break;
                }
            }
            
        }
        public void Execute()
        {
            _db.UpdatePackage(_updated);
        }

        public void Undo()
        {
            _db.UpdatePackage(_package);
        }
    }
    public class RemoveReservationCommand : ICommand
    {
        private readonly IDatabaseService _db;
        private readonly Reservation _reservation;
        private readonly int _reservationId;

        public RemoveReservationCommand(IDatabaseService db, int reservationId)
        {
            _db = db;
            _reservationId = reservationId;

            _reservation = _db.GetReservationById(reservationId);
        }

        public void Execute()
        {
            _db.RemoveReservation(_reservationId);
        }

        public void Undo()
        {
            _db.AddReservation(_reservation);
        }
    }
    public class UpdateReservationCommand : ICommand
    {
        private readonly IDatabaseService _db;
        private readonly Reservation _reservation;
        private readonly Reservation _updated;
        public UpdateReservationCommand(IDatabaseService db, int id, int numPerson,string extra)
        {
            _db = db;
            _reservation = _db.GetReservationById(id);
            _updated = new Reservation
            {
                Id = id,
                NumPersons = numPerson,
                ExtraServices = extra
            };
        }

        public void Execute()
        {
            _db.UpdateReservation(_updated.Id, _updated.NumPersons,_updated.ExtraServices);
        }

        public void Undo()
        {
            _db.UpdateReservation(_reservation.Id, _reservation.NumPersons, _reservation.ExtraServices);
        }
    }
    // Command Invoker
    public class CommandInvoker
    {
        private readonly Stack<ICommand> _commandHistory = new();

        public void ExecuteCommand(ICommand command)
        {
            command.Execute();
            _commandHistory.Push(command);
        }

        public void UndoLastCommand()
        {
            if (_commandHistory.Count > 0)
            {
                var command = _commandHistory.Pop();
                command.Undo();
            }
        }
    }
} 