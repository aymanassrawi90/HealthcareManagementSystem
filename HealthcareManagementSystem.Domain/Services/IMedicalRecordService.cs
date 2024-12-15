using HealthcareManagementSystem.Domain.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthcareManagementSystem.Domain.Services
{
    // Domain Layer: Interfaces/IMedicalRecordService.cs
    public interface IMedicalRecordService
    {
        Task CreateMedicalRecordAsync(MedicalRecord medicalRecord);
        Task UpdateMedicalRecordAsync(MedicalRecord medicalRecord);
        Task<MedicalRecord> GetMedicalRecordByVisitIdAsync(int visitId);
        Task<IEnumerable<MedicalRecord>> GetMedicalRecordByFilterAsync(ListFilter filter);
    }

}
