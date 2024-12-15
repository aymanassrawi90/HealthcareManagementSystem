using HealthcareManagementSystem.Domain.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthcareManagementSystem.Domain.Services
{
    public interface IVisitService
    {
        Task RegisterVisitAsync(Visit visit);
        Task<IEnumerable<Visit>> GetVisitsForPatientAsync(int patientId);
        Task<int> GetTotalVisitsForHealthCenterAsync(int healthCenterId);

        Task<IEnumerable<Visit>> GetAllVisitsAsync(int patientId);
        Task<IEnumerable<Visit>> GetVisitsByFilterAsync(ListFilter filter);
        Task<Visit> GetVisitsByIdAsync(int id);
    }
}
