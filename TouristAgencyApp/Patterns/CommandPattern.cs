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
            // U realnoj aplikaciji bi se vratio ID
           
            _reservationId = id; // Placeholder
        }

        public void Undo()
        {
            if (_reservationId > 0)
                _db.RemoveReservation(_reservationId);
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
            // U realnoj aplikaciji bi se učitao reservation
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