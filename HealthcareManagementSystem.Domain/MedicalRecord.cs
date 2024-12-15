using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HealthcareManagementSystem.Domain
{
    // MedicalRecord class to store medical details
    public class MedicalRecord
    {
        public int Id { get; private set; }
        public List<Diagnosis> Diagnosis { get; private set; }
        public Visit Visit { get; private set; }
        public List<string> Treatment { get; private set; }
        public List<string> MedicalTests { get; private set; }

        public MedicalRecord()
        {
            Diagnosis= new List<Diagnosis>();
            Treatment = new List<string>();
            MedicalTests = new List<string>();
        }
        public MedicalRecord(  Visit visit)
        {
          
            if (visit == null)
                throw new ArgumentNullException( nameof(Visit));

            
            Diagnosis = new List<Diagnosis>();
            MedicalTests = new List<string>();
            Visit=visit;
            Treatment= new List<string>();
        }

        // Method to add a medical test result
        public void AddMedicalTest(string testName)
        {
            if (string.IsNullOrEmpty(testName))
                throw new ArgumentException("Test name cannot be empty", nameof(testName));

            MedicalTests.Add(testName);
        }

        // Method to update treatment
        public void AddTreatment(string newTreatment)
        {
            if (string.IsNullOrEmpty(newTreatment))
                throw new ArgumentException("Treatment cannot be empty", nameof(newTreatment));

            Treatment.Add ( newTreatment);
        }

        public void AddDiagnosis(Diagnosis diagnosis)
        {
            if(diagnosis == null) throw new ArgumentNullException(nameof(diagnosis));
            Diagnosis.Add (diagnosis);  
        }

        public void RemoveDiagnosis(Diagnosis diagnosis)
        {
            if (diagnosis == null) throw new ArgumentNullException(nameof(diagnosis));
            Diagnosis.Remove(diagnosis);
        }
    }
}
