using HealthcareManagementSystem.Domain.Repository;
using HealthcareManagementSystem.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthcareManagementSystem.Domain.Services;
using HealthcareManagementSystem.Domain.Filters;
using HealthcareManagementSystem.Infrastructure.ListFilters;
using System.Linq.Expressions;

namespace HealthcareManagementSystem.Application.Services
{
    public class VisitService : IVisitService
    {
        private readonly IRepository<Visit> _visitRepository;
    

        public VisitService(IRepository<Visit> visitRepository )
        {
            _visitRepository = visitRepository;
           
        }

        public async Task RegisterVisitAsync(Visit visit)
        {
             _visitRepository.Add(visit);
            await _visitRepository.SaveAsync();
        }

        public async Task<IEnumerable<Visit>> GetVisitsForPatientAsync(int patientId)
        {
            var filter= new VisitListFilter { PatientId = patientId };
            var filters = GetFilters(filter);
            return await _visitRepository.GetItemsAsync(filters, null, filter);

            //  return await _visitRepository.GetItemByIdAsync(patientId);
        }

        public async Task<int> GetTotalVisitsForHealthCenterAsync(int healthCenterId)
        {
        
          return  await _visitRepository.CountAsync(v=>v.HealthCenter.Id== healthCenterId);
        }

        public async Task<IEnumerable<Visit>> GetAllVisitsAsync(int patientId)
        {
            return await _visitRepository.GetItemsAsync();
        }

        

        private static Expression<Func<Visit, bool>>[] GetFilters(
VisitListFilter filter)
        {
            var filters = new List<Expression<Func<Visit, bool>>>();
            if (!string.IsNullOrWhiteSpace(filter?.ReasonForVisit))
                filters.Add(e => e.Reason.ToLower().Contains(filter.ReasonForVisit.ToLower()));
           ;

            if (filter.Id.HasValue)
                filters.Add(e => e.Id == filter.Id);

            if (filter.PatientId.HasValue)
                filters.Add(e => e.Patient.Id == filter.PatientId);

            if (filter.HealthCenterId.HasValue)
                filters.Add(e => e.HealthCenter.Id == filter.HealthCenterId);

            //if (filter.DiagnosisId.HasValue)
            //    filters.Add(e => e.Diagnosis.DiagnosisName == filter.Id);


            return filters.ToArray();
        }

        public async Task<IEnumerable<Visit>> GetVisitsByFilterAsync(ListFilter filter)
        {
            var visitFilter = filter as VisitListFilter;
            var filters= GetFilters(visitFilter);
            return await _visitRepository.GetItemsAsync(filters,null, visitFilter);
        }

        public async Task<Visit> GetVisitsByIdAsync(int id)
        {
            return await _visitRepository.GetItemByIdAsync(id);
        }
    }


}
