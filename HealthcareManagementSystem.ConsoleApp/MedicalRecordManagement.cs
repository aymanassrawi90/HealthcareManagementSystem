using HealthcareManagementSystem.Application.Services;
using HealthcareManagementSystem.Domain;
using HealthcareManagementSystem.Domain.Services;
using HealthcareManagementSystem.Infrastructure.Exceptions;
using HealthcareManagementSystem.Infrastructure.ListFilters;

namespace HealthcareManagementSystem.ConsoleApp
{
    public class MedicalRecordManagement
    {
        private readonly IVisitService _visitService;
        private readonly IPatientService _patientService;
        private readonly IMedicalRecordService _medicalRecordService;
        private readonly IDiagnosisService _diagnosisService;
        private readonly IHelper _helper;       

        public MedicalRecordManagement(IMedicalRecordService medicalRecordService, IVisitService visitService, IPatientService patientService, IDiagnosisService diagnosisService,IHelper helper)
        {
            _medicalRecordService = medicalRecordService ?? throw new ArgumentNullException(nameof(medicalRecordService));
            _visitService = visitService ?? throw new ArgumentNullException(nameof(visitService));
            _patientService = patientService ?? throw new ArgumentNullException(nameof(patientService));
            _diagnosisService= diagnosisService ?? throw new Exception(nameof(diagnosisService));
            _helper= helper ?? throw new Exception(nameof(helper));
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
                        await CreateMedicalRecordAsync();
                        break;
                    case "2":
                        await ViewPatientMedicalHistory();
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
            Console.WriteLine("1. Create a new medical record");
            Console.WriteLine("2. View full medical history for a patient.");
            Console.WriteLine("3. View Total Visit for Helth Center.");
            Console.WriteLine("0. Go Back");
        }

        public async Task CreateMedicalRecordAsync()
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

                var visitId = _helper.ReadInt("Enter Visit ID: ");
                var filter = new VisitListFilter
                { 
                PatientId = patientId,
                Id = visitId
                };
                var visits = await _visitService.GetVisitsByFilterAsync(filter);
                if (visits == null || visits.Count()==0)
                {
                    
                    _helper.DisplayError($"Visist not found for patient {patient.Name}.");
                    return;
                }
                var visit = visits.FirstOrDefault();
               
                var medicalRecord = new MedicalRecord(visit);

                await _medicalRecordService.CreateMedicalRecordAsync(medicalRecord);
                // Get Medical Tests from the user
                Console.WriteLine("Enter Medical Tests (Enter 'done' when finished): ");
                while (true)
                {
                  
                    string test = _helper.ReadOptionalInput("Test name: ","done");
                    if (test.ToLower() == "done") break;  // Exit loop when user types "done"
                    if (!string.IsNullOrWhiteSpace(test))
                    {
                        medicalRecord.AddMedicalTest(test.Trim());
                    }
                    else
                    {
                       _helper.DisplayError("Please enter a valid test name.");
                    }
                }

                // Get Treatments from the user
                Console.WriteLine("Enter Treatments (Enter 'done'  when finished): ");
                while (true)
                {
                   
                    string treatment = _helper.ReadOptionalInput("Treatment: ", "done");
                    if (treatment.ToLower() == "done" ) break;  // Exit loop when user types "done"
                    if (!string.IsNullOrWhiteSpace(treatment))
                    {
                        medicalRecord.AddTreatment(treatment.Trim());
                    }
                    else
                    {
                        _helper.DisplayError("Please enter a valid treatment.");
                    }
                }




                var selectedDiagnoses = await DiagnosisHelper.GetDiagnosesFromUserInput(_diagnosisService,_helper);





                foreach (var diagnosis in selectedDiagnoses.Distinct())
                {
                    medicalRecord.AddDiagnosis(diagnosis);
                }

                await _medicalRecordService.UpdateMedicalRecordAsync(medicalRecord);

                 patient.AddMedicalRecord(medicalRecord);
                await _patientService.UpdatePatientAsync(patient);

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

        public async Task ViewPatientMedicalHistory()
        {
            try
            {
                var patientId = _helper.ReadInt("Enter patient ID: ");
                var patient = await _patientService.GetPatientByIdAsync(patientId);
                if(patient== null)
                {
                    _helper.DisplayError("patient not found");
                }

                var patientMedicalRecords = await _medicalRecordService.GetMedicalRecordByFilterAsync(new MedicalRecordListFilter { PatientId = patientId});

                _helper.DisplaySuccess($"List of Medical Record for Patient {patient.Name}:");
                foreach (var record in patientMedicalRecords)
                {
                    Console.WriteLine($"Visit ID: {record.Visit.Id}, Visit Date: {record.Visit.Date}");
                    Console.WriteLine($"Diagnosis: {string.Join(", ", record.Diagnosis.Select(_ => _.Name))}");
                    Console.WriteLine($"Medical Tests: {string.Join(" ", record.MedicalTests)}");
                    Console.WriteLine($"Treatments: {string.Join(" ",record.Treatment)}");
                    Console.WriteLine("_---------------------------------_");
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
