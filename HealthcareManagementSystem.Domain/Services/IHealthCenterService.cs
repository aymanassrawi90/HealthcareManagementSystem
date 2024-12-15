using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthcareManagementSystem.Domain.Services
{
    public interface IHealthCenterService
    {
        Task<HealthCenter> GetHealthCenterByIdAsync(int id);
        Task<IEnumerable<HealthCenter>> GetAllHealthCentersAsync();
        Task AddHealthCenterAsync(HealthCenter healthCenter);
        Task UpdateHealthCenterAsync(HealthCenter healthCenter);
    }
}
