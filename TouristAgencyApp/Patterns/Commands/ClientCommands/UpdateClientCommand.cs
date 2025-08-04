using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouristAgencyApp.Models;
using TouristAgencyApp.Patterns.Memento;
using TouristAgencyApp.Services;

namespace TouristAgencyApp.Patterns.Commands.ClientCommands
{
    public class UpdateClientCommand : ICommand
    {
        private readonly IDatabaseService _db;
        private readonly ClientMemento _beforeMemento;
        private readonly ClientMemento _afterMemento;
        private int clientId;
        private bool _executed;
        private bool _undone;
        private bool _redone;
        public UpdateClientCommand(IDatabaseService db, Client client)
        {
            _db = db;
            var currentClient = _db.GetClientById(client.Id);
            _beforeMemento = currentClient.CreateMemento();
            _afterMemento = client.CreateMemento();
        }
        public void Execute()
        {
            if (_executed) return;
            _db.UpdateClient(_afterMemento.GetState());
            _executed = true;
            _undone = false;
            _redone = false;
        }

        public void Undo()
        {
            if (!_executed || _undone)
                return;
            _db.UpdateClient(_beforeMemento.GetState());
            _undone = true;
            _redone = false;
        }
        public void Redo()
        {
            if (!_executed || !_undone || _redone)
                return;
            _db.UpdateClient(_afterMemento.GetState());//Mada moze i _updated ja mislim, ali neka ga ovako.
            _undone = false;
            _redone = true;
        }
    }
}
