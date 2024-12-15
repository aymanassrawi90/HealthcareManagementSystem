using HealthcareManagementSystem.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HealthcareManagementSystem.ConsoleApp
{
    public class MedicalSystemApp
    {
        private readonly IPatientService _patientService;
        private readonly IHealthCenterService _healthCenterService;
        private readonly IVisitService _visitService;
        private readonly IMedicalRecordService _medicalRecordService;
        private readonly IDiagnosisService _diagnosisService;

        private readonly IHelper _helper;
   
        private readonly PatientManagement _patientManagement;
        private readonly VisitManagement _visitManagement;
        private readonly MedicalRecordManagement _medicalRecordManagement;
        private readonly DiagnosisManagement _diagnosisManagement;
        private readonly MedicalCenterManagement _medicalCenterManagement;

        public MedicalSystemApp(IServiceProvider services)
        {
            _patientService = services.GetRequiredService<IPatientService>();
            _healthCenterService = services.GetRequiredService<IHealthCenterService>();
            _visitService = services.GetRequiredService<IVisitService>();
            _medicalRecordService = services.GetRequiredService<IMedicalRecordService>();
            _diagnosisService = services.GetRequiredService<IDiagnosisService>();
            _helper = services.GetRequiredService<IHelper>();
            
          
            // Initialize management services
            
            _medicalCenterManagement = new MedicalCenterManagement(_healthCenterService, _helper);
            _patientManagement = new PatientManagement(_patientService, _helper);
            _visitManagement = new VisitManagement(_visitService, _patientService, _healthCenterService, _helper);
            _medicalRecordManagement = new MedicalRecordManagement(_medicalRecordService, _visitService, _patientService, _diagnosisService, _helper);
            _diagnosisManagement = new DiagnosisManagement(_diagnosisService, _healthCenterService,_patientService,_visitService,_medicalRecordService, _helper);
        }

        public async Task InitializeAppAsync()
        {
            Console.WriteLine("Welcome to the Medical System!");

            var options = new Dictionary<string, Func<Task>>
        {
            { "p", () => ProcessPatientManagementAsync() },
            { "m", () => ProcessMedicalCenterManagementAsync() },
            { "v", () => ProcessVisitManagementAsync() },
            { "r", () => ProcessMedicalRecordManagementAsync() },
            { "d", () => ProcessDiagnosisManagementAsync() }
        };
            bool cleanScreen=true;

            while (true)
            {
               if(cleanScreen)
                {
                    Console.Clear();
                    DisplayMenu();
                }
                    

                var choice = Console.ReadLine()?.ToLower();
                if (choice == "0" && _helper.ConfirmExit()) return ;
                
                if (options.ContainsKey(choice))
                {
                    try
                    {
                        await options[choice]();
                        cleanScreen = true;
                    }
                    catch (Exception ex)
                    {
                        Console.Clear();
                        DisplayMenu();
                        cleanScreen =false;
                       _helper.DisplayError($"An error occurred: {ex.Message}");
                        // Log the error here if needed
                    }
                }
                else
                {
                    Console.Clear();
                    DisplayMenu();
                    cleanScreen = false;
                    _helper.DisplayError("Invalid option. Please try again.");
                }
            }
        }

        private static void DisplayMenu()
        {
            Console.WriteLine("Select an option:");
            Console.WriteLine("P. Patient Management");
            Console.WriteLine("M. Medical Center Management");
            Console.WriteLine("V. Visit Management");
            Console.WriteLine("R. Medical Record Management");
            Console.WriteLine("D. Diagnosis Management");
            Console.WriteLine("0. Exit");
        }

        private async Task ProcessPatientManagementAsync()
        {
            try
            {
                await _patientManagement.ProcessAsync();
            }
            catch (Exception ex)
            {
                _helper.DisplayError($"Error in Patient Management: {ex.Message}");
            }
        }

        private async Task ProcessMedicalCenterManagementAsync()
        {
            try
            {
                await _medicalCenterManagement.ProcessAsync();
            }
            catch (Exception ex)
            {
                _helper.DisplayError($"Error in Medical Center Management: {ex.Message}");
            }
        }

        private async Task ProcessVisitManagementAsync()
        {
            try
            {
                await _visitManagement.ProcessAsync();
            }
            catch (Exception ex)
            {
                _helper.DisplayError($"Error in Visit Management: {ex.Message}");
            }
        }

        private async Task ProcessMedicalRecordManagementAsync()
        {
            try
            {
                await _medicalRecordManagement.ProcessAsync();
            }
            catch (Exception ex)
            {
                _helper.DisplayError($"Error in Medical Record Management: {ex.Message}");
            }
        }

        private async Task ProcessDiagnosisManagementAsync()
        {
            try
            {
                await _diagnosisManagement.ProcessAsync();
            }
            catch (Exception ex)
            {
                _helper.DisplayError($"Error in Diagnosis Management: {ex.Message}");
            }
        }


    }
}
