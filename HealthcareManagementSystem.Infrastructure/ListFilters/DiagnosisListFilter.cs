using HealthcareManagementSystem.Domain.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthcareManagementSystem.Infrastructure.ListFilters
{
    public class DiagnosisListFilter : ListFilter
    {
        public string DiagnosisName { get; set; }
        public string Severity { get; set; }
    }
}
