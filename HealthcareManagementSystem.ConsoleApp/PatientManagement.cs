using HealthcareManagementSystem.Domain;
using HealthcareManagementSystem.Domain.Services;
using HealthcareManagementSystem.Infrastructure.Exceptions;
using HealthcareManagementSystem.Infrastructure.ListFilters;
using System.Linq;

namespace HealthcareManagementSystem.ConsoleApp
{
    public class PatientManagement
    {
        private readonly IPatientService _patientService;
        private readonly IHelper _helper;

        public PatientManagement(IPatientService patientService, IHelper helper)
        {
            _patientService = patientService ?? throw new ArgumentNullException(nameof(patientService));

            _helper = helper;
        }

        public async Task ProcessAsync()
        {
            var isRunning = true;
            while (isRunning)
            {
                Console.WriteLine("\nSelect an option:");
                Console.WriteLine("1. Add Patient");
                Console.WriteLine("2. View All Patients");
                Console.WriteLine("3. Search for Patients");
                Console.WriteLine("4. Update Patient Information");
                Console.WriteLine("0. Go Back");

                var choice = Console.ReadLine();
                try
                {
                    switch (choice)
                    {
                        case "1":
                            await AddPatientAsync();
                            break;
                        case "2":
                            await ViewAllPatientsAsync();
                            break;
                        case "3":
                            await SearchPatientsAsync();
                            break;
                        case "4":
                            await UpdatePatientAsync();
                            break;
                        case "0":
                            isRunning = _helper.ConfirmExit();
                            break;
                        default:
                            _helper.DisplayError("Invalid option. Please try again.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    Console.ResetColor();
                }
            }
        }

        public async Task AddPatientAsync()
        {
            Console.WriteLine("\nAdding a new patient...");

            var name =_helper.ReadNonEmptyInput("Enter patient name: ");
            var age = _helper.ReadInt("Enter age: ");
            var gender = _helper.ReadGender("Enter gender (M/F): ");
            var phone = _helper.ReadPhone("Enter phone: ");
            var address = _helper.ReadNonEmptyInput("Enter address: ");

            try
            {
                var patient = new Patient(name: name, age: age, gender: gender, phone: phone, address: address);
                await _patientService.AddPatientAsync(patient);

           
                _helper.DisplaySuccess($"Patient added successfully.\n ID: {patient.Id}, Name: {patient.Name}, Age: {patient.Age}, Gender: {patient.Gender}");
            }
            catch (ValidateModelException ex)
            {
                _helper.DisplayValidationErrors(ex);
            }
            catch (Exception ex)
            {
                _helper.DisplayError("An error occurred: {ex.Message}");
            }
        }

        public async Task ViewAllPatientsAsync()
        {
            Console.WriteLine("\nFetching all patients...");
            var patients = await _patientService.GetAllPatientsAsync();

            if (!patients.Any())
            {
                _helper.DisplayError("No patients found.");
                return;
            }

            _helper.DisplaySuccess("\nList of Patients:");
            
            foreach (var p in patients)
            {
                Console.WriteLine(p.ToString());
            }
        }

        public async Task SearchPatientsAsync()
        {
            Console.WriteLine("\nSearch for patients:");
            var name = _helper.ReadOptionalInput("Enter patient name: ");
            var phone = _helper. ReadOptionalInput("Enter phone: ");
            var idInput = _helper. ReadOptionalInput("Enter patient ID: ");

            var filter = new PatientListFilter
            {
                Name = name,
                Phone = phone,
            };

            if (int.TryParse(idInput, out var id))
            {
                filter.Id = id;
            }

            var searchResults = await _patientService.Search(filter);

            if (!searchResults.Any())
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("No patients found matching the search criteria.");
                Console.ResetColor();
                return;
            }

            _helper.DisplaySuccess("\nList of Patients:");
            foreach (var p in searchResults)
            {
                Console.WriteLine($"ID: {p.Id}, Name: {p.Name}, Phone: {p.Phone}, Age: {p.Age}, Gender: {p.Gender}");
            }
        }

        public async Task UpdatePatientAsync()
        {
            Console.WriteLine("\nUpdating patient information...");
            var id = _helper.ReadInt("Enter patient ID to update: ");
            var patient = await _patientService.GetPatientByIdAsync(id);

            if (patient == null)
            {
                _helper.DisplayError("Patient not found.");
                return;
            }

            Console.WriteLine("Leave fields blank to keep the existing value.");

            patient.Name =_helper. ReadOptionalInput($"Enter new name (current: {patient.Name}): ", patient.Name);
            patient.Age = _helper.ReadIntOptional($"Enter new age (current: {patient.Age}): ", patient.Age);
            patient.Gender = _helper.ReadOptionalInput($"Enter new gender (current: {patient.Gender}): ", patient.Gender);
            patient.Phone = _helper.ReadPhone($"Enter new phone (current: {patient.Phone}): ", patient.Phone);
            patient.Address = _helper.ReadOptionalInput($"Enter new address (current: {patient.Address}): ", patient.Address);

            try
            {
                await _patientService.UpdatePatientAsync(patient);
                _helper.DisplaySuccess("Patient information updated successfully.");
            }
            catch (ValidateModelException ex)
            {
                _helper.DisplayValidationErrors(ex);
            }
            catch (Exception ex)
            {
                _helper.DisplayError($"An error occurred: {ex.Message}");
            }
        }

      

       

   
    }
}
