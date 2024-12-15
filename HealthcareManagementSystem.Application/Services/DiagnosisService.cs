using HealthcareManagementSystem.Domain.Repository;
using HealthcareManagementSystem.Domain;

using HealthcareManagementSystem.Domain.Services;
using System.Linq.Expressions;
using HealthcareManagementSystem.Infrastructure.ListFilters;
using HealthcareManagementSystem.Infrastructure.Exceptions;

namespace HealthcareManagementSystem.Application.Services
{
    public class DiagnosisService : IDiagnosisService
    {
        private readonly IRepository<Diagnosis> _diagnosisRepository;
        private readonly IMedicalRecordService  _medicalRecordService;


        public DiagnosisService(IRepository<Diagnosis> diagnosisRepository, IMedicalRecordService medicalRecordService)
        {
            _diagnosisRepository = diagnosisRepository;
            _medicalRecordService = medicalRecordService;
        }

     

        public async Task CreateDiagnosisAsync(Diagnosis diagnosis)
        {
            var isExsist =await _diagnosisRepository.ExistsAsync(_=>_.Name.ToLower() == diagnosis.Name.ToLower().Trim());
            if (isExsist)
            {
                ExceptionManager.ThrowConflictException("Name", $"the Name {diagnosis.Name} already exsist");
            }
            _diagnosisRepository.Add(diagnosis);
            await _diagnosisRepository.SaveAsync();
        }

        /// <summary>
        /// Add a list of Diagnosis and retuen list of exsisting items
        /// </summary>
        /// <param name="diagnoses"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<IEnumerable<Diagnosis>> CreateDiagnosesAsync(IEnumerable<Diagnosis> diagnoses)
        {
            if (diagnoses == null || !diagnoses.Any())
            {
                throw new ArgumentException("The diagnosis list is empty.", nameof(diagnoses));
            }

            diagnoses= diagnoses.Distinct().ToList();   
            var skippedDiagnoses = new List<Diagnosis>();

            foreach (var diagnosis in diagnoses)
            {
                try
                {
                    // Check if the diagnosis already exists (case-insensitive check)
                    var isExists = await _diagnosisRepository.ExistsAsync(_ => _.Name.ToLower() == diagnosis.Name.ToLower().Trim());
                    if (isExists)
                    {
                        skippedDiagnoses.Add(diagnosis);
                        continue;
                    }

                    // Add diagnosis to the repository
                    _diagnosisRepository.Add(diagnosis);
                }
                catch (Exception ex)
                {
                    // Handle specific diagnosis errors (if needed, log them, etc.)
                 
                }
            }

            // Save changes to the repository
            await _diagnosisRepository.SaveAsync();

            // Inform about skipped diagnoses
            if (skippedDiagnoses.Any())
            {
                var skippedMessage = string.Join(", ", skippedDiagnoses);
                Console.WriteLine($"The following diagnoses were skipped  to add: {skippedMessage}");
            }
            else
            {
                Console.WriteLine("All diagnoses were successfully added.");
            }
            return skippedDiagnoses;
        }



        public async Task<IEnumerable<Diagnosis>> GetAllDiagnosesAsync()
        {
            var filters = new List<Expression<Func<Diagnosis, bool>>>();

         return   await _diagnosisRepository.GetItemsAsync();
          
        }

        public async Task<IEnumerable<(string DiagnosisName, int Count)>> GetMostCommonDiagnosesAsync(int healthCenterId)
        {
            var filter= new MedicalRecordListFilter { HealthCenterId = healthCenterId };
            var visits = await _medicalRecordService.GetMedicalRecordByFilterAsync(filter);

            var commonDiagnoses = visits
        .Where(v => v.Diagnosis != null && v.Diagnosis.Any()) // Ensure there's at least one diagnosis
        .SelectMany(v => v.Diagnosis) // Flatten the List<Diagnosis> into individual diagnoses
        .GroupBy(d => d.Name) // Group by diagnosis name
        .Select(g => (DiagnosisName: g.Key, Count: g.Count()))
        .OrderByDescending(d => d.Count); // order

            return commonDiagnoses;
        }
    }

}
