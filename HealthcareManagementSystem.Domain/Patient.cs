using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthcareManagementSystem.Domain
{
    public class Patient : Person
    {
        public List<MedicalRecord> MedicalRecords { get; private set; }
        public Patient()
            :base()
        {
            MedicalRecords = new List<MedicalRecord>();
        }
        public Patient( string name, int age, string gender, string phone, string address)
            : base( name, age, gender, phone, address)
        {
            MedicalRecords = new List<MedicalRecord>();
        }
       
        // Method to add a medical record to the patient
        public void AddMedicalRecord(MedicalRecord record)
        {
            if (record == null)
                throw new ArgumentNullException(nameof(record));

            MedicalRecords.Add(record);
        }

        public IEnumerable<MedicalRecord> GetAllMedicalRecords()
        {
            return MedicalRecords;
        }
    }
}
