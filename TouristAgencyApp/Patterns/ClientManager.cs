using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouristAgencyApp.Models;
using TouristAgencyApp.Patterns.Commands;
using TouristAgencyApp.Services;

namespace TouristAgencyApp.Patterns
{
    class ClientManager
    {
        private readonly CommandInvoker _invoker = new();
        private readonly IDatabaseService _dbService;

        public ClientManager(IDatabaseService dbService)
        {
            _dbService = dbService;
        }
        public int AddClient(Client client)
        {
            var addCommand = new AddClientCommand(_dbService, client);
            _invoker.ExecuteCommand(addCommand);
            MessageBox.Show("Client added.");
            return addCommand.ClientId;
        }
        public void RemoveClient(int clientId)
        {
           //Po zahtevima za zadatak nije potrebno, pise samo prikaz/dodavanje klijenata.
        }
        public void updateClient(Client client)
        {
            var updateCommand = new UpdateClientCommand(_dbService, client);
            _invoker.ExecuteCommand(updateCommand);
            MessageBox.Show("Client updated.");
        }
        public void UndoLastAction()
        {
            _invoker.UndoLastCommand();
            MessageBox.Show("Poslednja akcija uspesno opozvana!");
        }
        public void RedoLastAction()
        {
            _invoker.RedoLastAction();
            MessageBox.Show("Poslednja akcija uspesno redo-ovana!");
        }
    }
}
