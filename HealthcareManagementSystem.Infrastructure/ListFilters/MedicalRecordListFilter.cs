using HealthcareManagementSystem.Domain.Filters;

namespace HealthcareManagementSystem.Infrastructure.ListFilters
{
    public class MedicalRecordListFilter : ListFilter
    {
        public int? HealthCenterId { get; set; }
        public int? VisitId { get; set; }
        public string Treatment { get; set; }
        
        public int? PatientId { get; set; }

    }
}
