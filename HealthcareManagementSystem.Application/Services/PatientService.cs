using HealthcareManagementSystem.Domain;
using HealthcareManagementSystem.Domain.Filters;
using HealthcareManagementSystem.Domain.Repository;
using HealthcareManagementSystem.Domain.Services;
using HealthcareManagementSystem.Infrastructure.Exceptions;
using HealthcareManagementSystem.Infrastructure.ListFilters;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace HealthcareManagementSystem.Application.Services
{

    public class PatientService : IPatientService
    {
        private readonly IRepository<Patient> _patientRepository;

        public PatientService(IRepository<Patient> patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public async Task AddPatientAsync(Patient patient)
        {

            bool phoneExists = await _patientRepository.ExistsAsync(person => person.Phone == patient.Phone);

          var errors=  ValadationPatient(patient, phoneExists);
            if (errors.Count > 0) ExceptionManager.ThrowInvalidModelException(errors);
            _patientRepository.Add(patient);
            await _patientRepository.SaveAsync();
        }

        private static Dictionary<string, string> ValadationPatient(Patient patient, bool phoneExists)
        {
            Dictionary<string, string> errors = new Dictionary<string, string>();
            if (phoneExists)
            {
                errors.Add("phone", "Phone number already exists");
            }
            if (patient.Age <= 0)
            {
                errors.Add("Age", "Invaliad age ");

            }
            if (!(patient.Gender.ToLower() == "m" || patient.Gender.ToLower() == "f"))
            {

                errors.Add("Gender", "Invaliad Gender should be M or F ");

            }

            string pattern = @"^(0\d{9}|(?:\+|00)\d{3}\d{9})$";


            // Check if the phone number matches the regex pattern
            Regex regex = new Regex(pattern);
            var isPhoneNumber= regex.IsMatch(patient.Phone);
            if(!isPhoneNumber)
            {
                errors.Add("Phone",$"Invalid phone number: {patient.Phone}");
            }

            return errors;
        }

        public async Task<Patient> GetPatientByIdAsync(int id)
        {
            return await _patientRepository.GetItemByIdAsync(id);
        }

        public async Task<IEnumerable<Patient>> GetAllPatientsAsync()
        {
            return await _patientRepository.GetItemsAsync();
        }
        //PatientListFilter
       

        public async Task UpdatePatientAsync(Patient patient)
        {
            bool phoneExists = await _patientRepository.ExistsAsync(person => person.Phone == patient.Phone && person.Id != patient.Id);
         var errors=   ValadationPatient(patient, phoneExists);
            if (errors.Count > 0) ExceptionManager.ThrowInvalidModelException(errors);
            _patientRepository.Update(patient);
          await  _patientRepository.SaveAsync();
        }

        public async Task DeletePatientAsync(int id)
        {
            var entity= await _patientRepository.GetItemByIdAsync(id);
            if(entity == null) {
                ExceptionManager.ThrowItemNotFoundException($"The Patient with id {id} not found");
            
            }
             _patientRepository.Delete(entity);
         await   _patientRepository.SaveAsync();
        }

        private static Expression<Func<Patient, bool>>[] GetFilters(
     PatientListFilter filter)
        {
            var filters = new List<Expression<Func<Patient, bool>>>();
            if (!string.IsNullOrWhiteSpace(filter?.Name))
                filters.Add(e => e.Name.ToLower().Contains(filter.Name.ToLower()));
            if (!string.IsNullOrWhiteSpace(filter?.Phone))
                filters.Add(e => e.Phone.Contains(filter.Phone));

            if (filter.Id.HasValue)
                filters.Add(e => e.Id == filter.Id);


            return filters.ToArray();
        }

       

        public async Task<IEnumerable<Patient>> Search(ListFilter filter)
        {

            var patientFilter = filter as PatientListFilter;
            var filters = GetFilters(patientFilter);

            return await _patientRepository.GetItemsAsync(filters, null, filter);
        }
    }

}
