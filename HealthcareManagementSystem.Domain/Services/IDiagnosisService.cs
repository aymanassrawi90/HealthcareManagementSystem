using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthcareManagementSystem.Domain.Services
{
    // Domain Layer: Interfaces/IDiagnosisService.cs
    public interface IDiagnosisService
    {
        Task CreateDiagnosisAsync(Diagnosis diagnosis);
        Task<IEnumerable<Diagnosis>> GetAllDiagnosesAsync();
         Task<IEnumerable<(string DiagnosisName, int Count)>> GetMostCommonDiagnosesAsync(int healthCenterId);
        Task<IEnumerable<Diagnosis>> CreateDiagnosesAsync(IEnumerable<Diagnosis> diagnoses);
    }
}
