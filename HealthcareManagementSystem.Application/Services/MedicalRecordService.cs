using HealthcareManagementSystem.Domain;
using HealthcareManagementSystem.Domain.Filters;
using HealthcareManagementSystem.Domain.Repository;
using HealthcareManagementSystem.Domain.Services;
using HealthcareManagementSystem.Infrastructure.ListFilters;
using System.Linq.Expressions;

namespace HealthcareManagementSystem.Application.Services
{
 
    public class MedicalRecordService : IMedicalRecordService
    {
        private readonly IRepository<MedicalRecord> _medicalRecordRepository;

        public MedicalRecordService(IRepository<MedicalRecord> medicalRecordRepository)
        {
            _medicalRecordRepository = medicalRecordRepository;
        }

        public async Task CreateMedicalRecordAsync(MedicalRecord medicalRecord)
        {
             _medicalRecordRepository.Add(medicalRecord);
            await _medicalRecordRepository.SaveAsync();
        }

        public async Task<IEnumerable<MedicalRecord>> GetMedicalRecordByFilterAsync(ListFilter filter)
        {
            var medicalFilter = filter as MedicalRecordListFilter;
            var filters = GetFilters(medicalFilter);
            return await _medicalRecordRepository.GetItemsAsync(filters,null, filter);
        }

        public async Task<MedicalRecord> GetMedicalRecordByVisitIdAsync(int visitId)
        {
            var records = await _medicalRecordRepository.GetItemsAsync  ();
            return records.FirstOrDefault(r => r.Visit != null && r.Visit.Id == visitId);
        }

        private static Expression<Func<MedicalRecord, bool>>[] GetFilters(
MedicalRecordListFilter filter)
        {
            var filters = new List<Expression<Func<MedicalRecord, bool>>>();
            if (!string.IsNullOrWhiteSpace(filter?.Treatment))
                filters.Add(e => e.Treatment.Contains(filter.Treatment.ToLower()));
            //if (!string.IsNullOrWhiteSpace(filter?.Diagnosis))
            //    filters.Add(e => e.Diagnosis!= null &&  e.Diagnosis.Contains(filter.Diagnosis));

            if (filter.Id.HasValue)
                filters.Add(e => e.Id == filter.Id);

            if (filter.HealthCenterId.HasValue)
                filters.Add(e => e.Visit != null && e.Visit.HealthCenter != null && e.Visit.HealthCenter.Id == filter.HealthCenterId);


            if (filter.VisitId.HasValue)
                filters.Add(e => e.Visit != null && e.Visit.Id== filter.VisitId);
            if (filter.PatientId.HasValue)
                filters.Add(e => e.Visit != null && e.Visit.Patient.Id== filter.PatientId);

            return filters.ToArray();
        }

        public async Task UpdateMedicalRecordAsync(MedicalRecord medicalRecord)
        {
           _medicalRecordRepository.Update(medicalRecord);
            await _medicalRecordRepository.SaveAsync();
        }
    }


}
