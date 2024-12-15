using static System.Net.Mime.MediaTypeNames;

namespace HealthcareManagementSystem.Domain
{
    public class Diagnosis
    {
        public string Name { get;  set; }
        public string Severity { get;  set; }

        public Diagnosis()
        {

        }
        public Diagnosis(string diagnosisName, string severity)
        {
            if (string.IsNullOrWhiteSpace(diagnosisName))
                throw new ArgumentException("Diagnosis name cannot be empty.", nameof(diagnosisName));

            Name = diagnosisName;
            Severity = severity;
        }

        public override string ToString()
        {
            return $"name: {Name}, Severity: {Severity}";
        }
    }
}
