using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouristAgencyApp.Models;
using TouristAgencyApp.Patterns.Commands;
using TouristAgencyApp.Patterns.Commands.ClientCommands;
using TouristAgencyApp.Services;
using TouristAgencyApp.Utils;

namespace TouristAgencyApp.Patterns
{
    public class ClientManager
    {
        private readonly CommandInvoker _invoker = new();
        private readonly IDatabaseService _dbService;
        public event EventHandler<ClientChangedEventArgs>? ClientChanged;
        public ClientManager(IDatabaseService dbService)
        {
            _dbService = dbService;
        }
        protected virtual void OnClientChanged(Client client, string action, int id)
        {
            var eventArgs = new ClientChangedEventArgs(client, action);
            eventArgs.Id = id;
            ClientChanged?.Invoke(this, eventArgs);
        }
        public int AddClient(Client client)
        {
            var addCommand = new AddClientCommand(_dbService, client);
            _invoker.ExecuteCommand(addCommand);
            OnClientChanged(client, "Added",addCommand.ClientId);

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
   
            OnClientChanged(client, "Updated",0);
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
