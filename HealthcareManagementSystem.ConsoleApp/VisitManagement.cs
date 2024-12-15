using HealthcareManagementSystem.Domain;
using HealthcareManagementSystem.Domain.Services;
using HealthcareManagementSystem.Infrastructure.Exceptions;

namespace HealthcareManagementSystem.ConsoleApp
{
    public class VisitManagement
    {
        private readonly IHealthCenterService _healthCenterService;
        private readonly IVisitService _visitService;
        private readonly IPatientService _patientService;
        private readonly IHelper _helper;

        public VisitManagement(IVisitService visitService, IPatientService patientService, IHealthCenterService healthCenterService, IHelper helper)
        {
            _visitService = visitService ?? throw new ArgumentNullException(nameof(visitService));
            _patientService = patientService ?? throw new ArgumentNullException(nameof(patientService));
            _healthCenterService = healthCenterService ?? throw new ArgumentNullException(nameof(healthCenterService));
            _helper = helper ?? throw new ArgumentNullException(nameof(_helper));
        }

        public async Task ProcessAsync()
        {
            bool isRunning = true;
            while (isRunning)
            {
                ShowMenu();
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await RegisterNewVisitAsync();
                        break;
                    case "2":
                        await ViewVisitsForPatientAsync();
                        break;
                    case "3":
                        await ViewTotalVisitsForHealthCenterAsync();
                        break;
                    case "0":
                        isRunning = _helper.ConfirmExit();
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid option. Please try again.");
                        Console.ResetColor();
                        break;
                }
            }
        }

        private void ShowMenu()
        {
            Console.WriteLine(" Select an option:");
            Console.WriteLine("1. Register a new visit.");
            Console.WriteLine("2. View a list of visits for a specific patient.");
            Console.WriteLine("3. View the total number of visits for a health center.");
            Console.WriteLine("0. Go Back");
        }

        public async Task RegisterNewVisitAsync()
        {
            try
            {
                var patientId = _helper.ReadInt("Enter patient ID: ");
                var patient = await _patientService.GetPatientByIdAsync(patientId);
                if (patient == null)
                {
                    _helper.DisplayError("Patient not found.");
                    return;
                }

                var healthCenterId = _helper.ReadInt("Enter health center ID: ");
                var healthCenter = await _healthCenterService.GetHealthCenterByIdAsync(healthCenterId);
                if (healthCenter == null)
                {
                    _helper.DisplayError("Medical health center not found.");
                    return;
                }

               
                var reason = _helper.ReadNonEmptyInput("Reason for visit: ");

                var visit = new Visit(patient, healthCenter, reason);
                await _visitService.RegisterVisitAsync(visit);

                _helper.DisplaySuccess($"Visit added successfully. ID: {visit.Id}, Patient Name: {visit.Patient.Name}, Reason: {visit.Reason}");
            }
            catch (Exception ex) when (ex is ValidateModelException validationException)
            {
               _helper.DisplayValidationErrors(validationException);
            }
            catch (Exception ex)
            {
                _helper.DisplayError(ex.Message);
            }
        }

        public async Task ViewVisitsForPatientAsync()
        {
            try
            {
                var patientId = _helper.ReadInt("Enter patient ID: ");
                var visits = await _visitService.GetVisitsForPatientAsync(patientId);
                if (visits == null || !visits.Any())
                {
                    _helper.DisplayError("No visits found.");
                         return;
                }
                _helper.DisplaySuccess("List of Visits:");
                foreach (var visit in visits)
                {
                    Console.WriteLine($"ID: {visit.Id}, Date: {visit.Date}, Reason: {visit.Reason}");
                }
            }
            catch (Exception ex)
            {
                _helper.DisplayError(ex.Message);
            }
        }

        public async Task ViewTotalVisitsForHealthCenterAsync()
        {
            try
            {
                var healthCenterId = _helper.ReadInt("Enter health center ID: ");
                var totalVisits = await _visitService.GetTotalVisitsForHealthCenterAsync(healthCenterId);

                _helper.DisplaySuccess($"Total Visits: {totalVisits}");
            }
            catch (Exception ex)
            {
                _helper.DisplayError(ex.Message);
            }
        }

       

       

       
    }

}
