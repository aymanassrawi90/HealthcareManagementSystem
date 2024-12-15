using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthcareManagementSystem.Domain
{
    public abstract class Doctor : Person
    {
        public string Specialty { get; private set; }

        public Doctor(int id, string name, int age, string gender, string phone, string address, string specialty)
            : base( name, age, gender, phone, address)
        {
            if (string.IsNullOrEmpty(specialty))
                throw new ArgumentException("Specialty cannot be empty", nameof(specialty));

            Specialty = specialty;
        }
        public abstract void AssignDiagnosis(Patient patient, Diagnosis diagnosis);
    }

    public class GeneralDoctor : Doctor
    {
        public GeneralDoctor(int id, string name, int age, string gender, string phone, string address, string specialty) : base(id, name, age, gender, phone, address, specialty)
        {
        }

        public override void AssignDiagnosis(Patient patient, Diagnosis diagnosis)
        {
            Console.WriteLine($"General Doctor {Name} has assigned the diagnosis: {diagnosis.Name} to patient {patient.Name}.");

        }
    }
    public class Specialist : Doctor
    {
        public string AreaOfExpertise { get; private set; }

        public Specialist(int id, string name, int age, string gender, string phone, string address, string specialty, string areaOfExpertise)
            : base(id, name, age, gender, phone, address, specialty)
        {
            if (string.IsNullOrEmpty(areaOfExpertise))
                throw new ArgumentException("Area of expertise cannot be empty", nameof(areaOfExpertise));

            AreaOfExpertise = areaOfExpertise;
        }

        public override void AssignDiagnosis(Patient patient, Diagnosis diagnosis)
        {
            Console.WriteLine($"Specialist {Name}, with expertise in cardiology, has assigned the diagnosis: {diagnosis.Name} to patient {patient.Name}.");
        }
    }
}
