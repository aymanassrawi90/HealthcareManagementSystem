using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthcareManagementSystem.Domain
{
    public abstract class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

        public Person()
        {

        }
        public Person( string name, int age, string gender, string phone, string address)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Name cannot be empty", nameof(name));
            if (age <= 0)
                throw new ArgumentException("Age must be greater than zero", nameof(age));
            if (string.IsNullOrEmpty(phone))
                throw new ArgumentException("Phone cannot be empty", nameof(phone));

          
            Name = name;
            Age = age;
            Gender = gender;
            Phone = phone;
            Address = address;
        }

        public override string ToString()
        {
            return $"ID: {Id}, Name: {Name}, Phone: {Phone}, Age: {Age}, Gender: {Gender}";
        }
    }
}
