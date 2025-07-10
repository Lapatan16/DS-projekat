using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TouristAgencyApp.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string PassportNumber { get; set; } = ""; // Encrypted
        public DateTime BirthDate { get; set; }
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
        public string FullName => $"{FirstName} {LastName}";

    }
}
