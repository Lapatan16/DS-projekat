using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouristAgencyApp.Models;
namespace TouristAgencyApp.Patterns.Observer.ClientObserver
{
    public class ClientSubject : ISubject
    {
        private readonly List<IObserver> _observers = new();
        private readonly List<Client> _clients = new();
        public void Attach(IObserver observer)
        {
            if (!_observers.Contains(observer))
                _observers.Add(observer);
        }

        public void Detach(IObserver observer)
        {
            _observers.Remove(observer);
        }

        public void Notify(string message)
        {
            foreach (var observer in _observers)
            {
                observer.Update(message);
            }
        }

        public void AddClient(Client client, int id)
        {
            _clients.Add(client);
            Notify($"Novi klijent dodat: email: {client.Email} | id: {id} ");
        }
        public void UpdateClient(Client client)
        {
            _clients.Add(client);
            Notify($"Klijent sa id = {client.Id} je azuriran");
        }
        public void RemoveClient(int clientId)
        {
            var client = _clients.Find(c => c.Id == clientId);
            if (client != null)
            {
                _clients.Remove(client);
                Notify($"Klijent uklonjen: {clientId}");
            }
        }
    }
}
