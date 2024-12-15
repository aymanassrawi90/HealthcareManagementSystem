using HealthcareManagementSystem.Domain.Filters;

namespace HealthcareManagementSystem.Infrastructure.ListFilters
{
    public class VisitListFilter : ListFilter
    {
        public DateTime? VisitDate { get; set; }
        public int? PatientId { get; set; }
        public int? HealthCenterId { get; set; }
        public int? DiagnosisId { get; set; }
        public string ReasonForVisit { get; set; } = string.Empty;
    }
}
