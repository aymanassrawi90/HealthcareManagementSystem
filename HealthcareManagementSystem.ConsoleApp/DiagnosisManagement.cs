using HealthcareManagementSystem.Domain;
using HealthcareManagementSystem.Domain.Services;
using HealthcareManagementSystem.Infrastructure.Exceptions;
using HealthcareManagementSystem.Infrastructure.ListFilters;

namespace HealthcareManagementSystem.ConsoleApp
{
    public class DiagnosisManagement
    {
        
        private readonly IDiagnosisService _diagnosisService;
        private readonly IHealthCenterService _healthCenterService;
        private readonly IPatientService _patientService;
        private readonly IVisitService _visitService;
        private readonly IMedicalRecordService _medicalRecordService;
        private readonly IHelper _helper;
       

        public DiagnosisManagement(IDiagnosisService diagnosisService, IHealthCenterService healthCenterService, IPatientService patientService, IVisitService visitService, IMedicalRecordService medicalRecordService, IHelper helper)
        {
            _diagnosisService = diagnosisService ?? throw new ArgumentNullException(nameof(diagnosisService));
            _healthCenterService = healthCenterService ?? throw new ArgumentNullException(nameof(healthCenterService));
            _patientService= patientService ?? throw new ArgumentNullException(nameof(_patientService));
            _visitService = visitService ?? throw new ArgumentNullException(nameof(visitService));
            _medicalRecordService= medicalRecordService ?? throw new ArgumentNullException(nameof(medicalRecordService));
            _helper = helper;
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
                        await CreateDiagnosis();
                        break;
                    case "2":
                        await ViewCommonDiagnoses();
                        break;
                    case "3":
                        await AssignDiagnosisToPatient();
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
            Console.WriteLine("1. Create a new diagnosis");
            Console.WriteLine("3. Assign a diagnosis to a patient’s visit");
            Console.WriteLine("2. View common diagnoses for a health center.");
            Console.WriteLine("0. Go Back");
        }

        public async Task CreateDiagnosis()
        {
            try
            {
                var name = _helper.ReadNonEmptyInput("Enter name : ");
                var servity = _helper.ReadNonEmptyInput("Enter Severity : ");

                var diagnosis = new Diagnosis(name, servity);
              await _diagnosisService.CreateDiagnosisAsync(diagnosis);
                _helper.DisplaySuccess($"Diagnosis added successfully.\n {diagnosis.ToString()}");


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

        public async Task ViewCommonDiagnoses()
        {
            try
            {
                var healthCenterId = _helper.ReadInt("Enter health center ID: ");
                var healthCenter = await _healthCenterService.GetHealthCenterByIdAsync(healthCenterId);
                if(healthCenter== null)
                {
                    _helper.DisplayError("Medical health center not found");
                }
             var commonDiagnoses= await  _diagnosisService.GetMostCommonDiagnosesAsync(healthCenterId);

               if(commonDiagnoses== null || commonDiagnoses.Count() ==0)
                {
                    _helper.DisplayError($"NO data found for medical helth center: {healthCenter.Name}");
                    return;
                }
                _helper.DisplaySuccess($"The most common diagnoses for a health center {healthCenter.Name}");
               foreach(var common in commonDiagnoses)
                {

                    Console.WriteLine($"- Diagnoses name:{common.DiagnosisName}  Count:{common.Count}");
                }
            }
            catch (Exception ex)
            {
                _helper.DisplayError(ex.Message);
            }
        }

        public async Task AssignDiagnosisToPatient()
        {
            try
            {
                // Step 1: Get Patient Details
                var patientId = _helper.ReadInt("Enter Patient Id: ");
                var patient = await _patientService.GetPatientByIdAsync(patientId);
                if (patient == null)
                {
                    _helper.DisplayError("No Patient Found");
                    return;
                }

                // Step 2: Get Visit Details
                var visitId = _helper.ReadInt("Enter Visit Id: ");
                var filterVisit = new VisitListFilter
                {
                    PatientId = patientId,
                    Id = visitId
                };
                var visits = await _visitService.GetVisitsByFilterAsync(filterVisit);
                if (visits == null || !visits.Any())
                {
                    _helper.DisplayError($"No visit found for patient: {patient.Name} (Id: {patientId}) with visit Id {visitId}");
                    return;
                }
                var visit = visits.FirstOrDefault();

               

                var selectedDiagnoses = await DiagnosisHelper.GetDiagnosesFromUserInput(_diagnosisService,_helper);

                if (selectedDiagnoses == null || !selectedDiagnoses.Any())
                {
                    _helper.DisplayError("No diagnoses selected or added.");
                    return;
                }

                var medicalRecord = await _medicalRecordService.GetMedicalRecordByVisitIdAsync(visit.Id);

                foreach (var diagnosis in selectedDiagnoses.Distinct())
                {
                    medicalRecord.AddDiagnosis(diagnosis);
                }

                await _medicalRecordService.UpdateMedicalRecordAsync(medicalRecord);
                

                patient.AddMedicalRecord(medicalRecord);
                await _patientService.UpdatePatientAsync(patient);
                _helper.DisplaySuccess("Diagnoses successfully assigned to the patient’s visit.");
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


       








    }

}
