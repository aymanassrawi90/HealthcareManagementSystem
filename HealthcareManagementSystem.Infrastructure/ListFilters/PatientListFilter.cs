using HealthcareManagementSystem.Domain.Filters;

namespace HealthcareManagementSystem.Infrastructure.ListFilters
{
    public class PatientListFilter : ListFilter
    {
        public string Name { get; set; }
        public string Phone { get; set; }
    }
}
