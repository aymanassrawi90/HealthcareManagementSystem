using HealthcareManagementSystem.Domain.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthcareManagementSystem.Domain.Services
{
    public interface IPatientService
    {
        Task AddPatientAsync(Patient patient);
        Task UpdatePatientAsync(Patient patient);
        Task<Patient> GetPatientByIdAsync(int id);
        Task<IEnumerable<Patient>> Search(ListFilter filter);
        Task<IEnumerable<Patient>> GetAllPatientsAsync();
    }
}
