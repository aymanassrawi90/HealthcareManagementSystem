using HealthcareManagementSystem.Domain.Repository;
using HealthcareManagementSystem.Domain.Services;
using HealthcareManagementSystem.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthcareManagementSystem.Infrastructure.Repositories;

namespace HealthcareManagementSystem.Application.Services
{
    // Application Layer: Services/HealthCenterService.cs
    public class HealthCenterService : IHealthCenterService
    {
        private readonly IRepository<HealthCenter> _healthCenterRepository;

        public HealthCenterService(IRepository<HealthCenter> healthCenterRepository)
        {
            _healthCenterRepository = healthCenterRepository;
        }

        public async Task AddHealthCenterAsync(HealthCenter healthCenter)
        {
             _healthCenterRepository.Add(healthCenter);
            await _healthCenterRepository.SaveAsync();
        }

     

        public async Task<IEnumerable<HealthCenter>> GetAllHealthCentersAsync()
        {
            return await _healthCenterRepository.GetItemsAsync();
        }

        public async Task<HealthCenter> GetHealthCenterByIdAsync(int id)
        {
           return await _healthCenterRepository.GetItemByIdAsync(id);
        }

        public async Task UpdateHealthCenterAsync(HealthCenter healthCenter)
        {
             _healthCenterRepository.Update(healthCenter);
            await _healthCenterRepository.SaveAsync();
        }
    }

}
