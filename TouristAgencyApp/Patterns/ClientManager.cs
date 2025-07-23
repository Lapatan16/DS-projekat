using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouristAgencyApp.Models;
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
            MessageBox.Show("Added");
            return addCommand.ClientId;
        }
        public void RemoveClient(int clientId)
        {
           //Po zahtevima za zadatak nije potrebno, pise samo prikaz/dodavanje klijenata, ali neka ga ovako.
        }
        public void updateClient(Client client)
        {
            var updateCommand = new UpdateClientCommand(_dbService, client);
            _invoker.ExecuteCommand(updateCommand);
            MessageBox.Show("Update cl");
        }
        public void UndoLastAction()
        {
            _invoker.UndoLastCommand();
            MessageBox.Show("Poslednja akcija uspesno opozvana!");
        }
    }
}
