using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouristAgencyApp.Models;
using TouristAgencyApp.Services;

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
            MessageBox.Show("Added");
            return addCommand.ReservationId;
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
    }
}
