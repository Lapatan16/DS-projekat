using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouristAgencyApp.Models;
using TouristAgencyApp.Services;
using TouristAgencyApp.Patterns.Commands;
using TouristAgencyApp.Patterns.Commands.ReservationCommands;

namespace TouristAgencyApp.Patterns
{
    class ReservationManager
    {
        private readonly CommandInvoker _invoker = new();
        private readonly IDatabaseService _dbService;

        public ReservationManager(IDatabaseService dbService)
        {
            _dbService = dbService;
        }
        public int AddReservation(Reservation reservation)
        {
            var addCommand = new AddReservationCommand(_dbService, reservation);
            _invoker.ExecuteCommand(addCommand);
            MessageBox.Show("Reservation added.");
            return addCommand.ReservationId;
        }
        public void UpdateReservation(int reservationId, int numPersons, string txtExtra)
        {
            var updateCommand = new UpdateReservationCommand(_dbService, reservationId, numPersons, txtExtra);
            _invoker.ExecuteCommand(updateCommand);
            MessageBox.Show("Reservation updated.");
        }
        public void RemoveReservation(int reservationId)
        {
            var removeCommand = new RemoveReservationCommand(_dbService, reservationId);
            _invoker.ExecuteCommand(removeCommand);
            MessageBox.Show("Reservation removed!");
        }
        public void UndoLastAction()
        {
            _invoker.UndoLastCommand();
            MessageBox.Show("Last action undone!");
        }
        public void RedoLastAction()
        {
            _invoker.RedoLastAction();
            MessageBox.Show("Last action redone!");
        }
    }
}
