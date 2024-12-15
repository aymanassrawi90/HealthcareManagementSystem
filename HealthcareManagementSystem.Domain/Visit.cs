using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthcareManagementSystem.Domain
{
    public class Visit
    {
        public int Id { get; set; }
        public Patient Patient { get; private set; }
        public HealthCenter HealthCenter { get; private set; }
        public DateTime Date { get; set; }
        public string Reason { get; set; }
      //  public Diagnosis Diagnosis { get; private set; }


        public Visit()
        {

        }
        public Visit( Patient patient, HealthCenter healthCenter,  string reason )
        {
           
            Patient = patient;
            HealthCenter = healthCenter;
            Date = DateTime.Now;
            Reason = reason;
        }
        public void SetPatient(Patient patient)
        {
            if (patient == null) throw new ArgumentNullException("");
            Patient = patient;  
        }
        public void AddHealthCenter(HealthCenter healthCenter)
        {
            if(healthCenter == null) throw new ArgumentNullException(nameof(HealthCenter));
            HealthCenter = healthCenter;
        }

        //public void AddDiagnosis(Diagnosis diagnosis)
        //{
        //    if (diagnosis == null) throw new ArgumentNullException(nameof(Diagnosis));
        //    Diagnosis = diagnosis;
        //}


    }
}
