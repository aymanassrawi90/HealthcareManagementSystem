using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthcareManagementSystem.Domain
{
    public class HealthCenter
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Type { get; set; }

        public HealthCenter() { }
        public HealthCenter( string name, string location, string type)
        {
         
            Name = name;
            Location = location;
            Type = type;
        }

        public override string ToString()
        {
            return $"ID: {Id}, Name:{Name}, Location: {Location}, Type:{Type}";
        }
    }
}
