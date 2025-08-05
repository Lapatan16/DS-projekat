using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TouristAgencyApp.Patterns.Observer.ReservationObserver
{
    public class ReservationStatistics : IObserver
    {
        private int _totalReservations = 0;
        private int _cancelledReservations = 0;

        public void Update(string message)
        {
            if (message.Contains("Nova rezervacija"))
                _totalReservations++;
            else if (message.Contains("Rezervacija uklonjena"))
                _cancelledReservations++;

            Console.WriteLine($"STATISTIKA: Ukupno rezervacija: {_totalReservations}, Otkazano: {_cancelledReservations}");
        }
    }
}
