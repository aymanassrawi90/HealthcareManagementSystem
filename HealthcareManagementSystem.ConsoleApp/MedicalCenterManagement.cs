using HealthcareManagementSystem.Domain;
using HealthcareManagementSystem.Domain.Services;
using HealthcareManagementSystem.Infrastructure.Exceptions;

namespace HealthcareManagementSystem.ConsoleApp
{
    public class MedicalCenterManagement
    {
        private readonly IHealthCenterService _healthCenterService;
        private readonly IHelper _helper;
        public MedicalCenterManagement(IHealthCenterService healthCenterService, IHelper helper)
        {
            _healthCenterService = healthCenterService ?? throw new ArgumentNullException(nameof(healthCenterService));
            _helper=helper ?? throw new ArgumentNullException(nameof(helper));
        }

        public async Task ProcessAsync()
        {
            bool isManagingHealthCenters = true;
            while (isManagingHealthCenters)
            {
                Console.WriteLine("\nSelect an option:");
                Console.WriteLine("1. Add a new health center ");
                Console.WriteLine("2. Update existing health center details.");
                Console.WriteLine("3. View a list of all health centers.");
                Console.WriteLine("0. Go Back");

                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        await AddHealthCenterAsync();
                        break;
                    case "2":
                        await UpdateHealthCenterAsync();
                        break;
                    case "3":
                        await DisplayAllHealthCentersAsync();
                        break;
                    case "0":
                        isManagingHealthCenters = _helper.ConfirmExit();
                        break;
                    default:
                        _helper.DisplayError("Invalid option. Please try again.");
                        break;
                }
            }
        }

        public async Task AddHealthCenterAsync()
        {
           
            var name = _helper.ReadNonEmptyInput("Enter name: "); ;
            var location = _helper.ReadNonEmptyInput("Enter location: ");
            var type = _helper.ReadNonEmptyInput("Enter type: "); ;

          

            try
            {
                var healthCenter = new HealthCenter { Name = name, Location = location, Type = type };
                await _healthCenterService.AddHealthCenterAsync(healthCenter);

                _helper.DisplaySuccess($"Medical health center added successfully: {healthCenter.ToString()}");
            }
            catch (ValidateModelException ex)
            {
               _helper. DisplayValidationErrors(ex);
                
            }
            catch (Exception ex)
            {
                _helper.DisplayError($"An error occurred: {ex.Message}");
            }
        }

        public async Task UpdateHealthCenterAsync()
        {
            try
            {
                var updateHealthId = _helper.ReadInt("Enter medical health center ID to update: ");
                var healthCenterToUpdate = await _healthCenterService.GetHealthCenterByIdAsync(updateHealthId);

                if (healthCenterToUpdate == null)
                {
                  
                    _helper.DisplayError("medical health center not found.");
                   
                    return;
                }

                Console.WriteLine("Leave fields blank to keep the existing value.");

                healthCenterToUpdate.Name = _helper.ReadOptionalInput($"Enter new name (current: {healthCenterToUpdate.Name}): ", healthCenterToUpdate.Name);


                healthCenterToUpdate.Location= _helper.ReadOptionalInput($"Enter new Location (current: {healthCenterToUpdate.Location}): ", healthCenterToUpdate.Location);


                healthCenterToUpdate.Type= _helper.ReadOptionalInput($"Enter new Type (current: {healthCenterToUpdate.Type}): ", healthCenterToUpdate.Type);
               





                await _healthCenterService.UpdateHealthCenterAsync(healthCenterToUpdate);
                _helper.DisplaySuccess($"Medical health center information updated successfully: {healthCenterToUpdate.ToString()}");
            }
            catch (Exception ex)
            {
                if (ex is ValidateModelException validationException)
                {
                   _helper.DisplayValidationErrors(validationException);
                }
                else
                {
                    _helper.DisplayError(ex.Message);
                }
            }
        }

        public async Task DisplayAllHealthCentersAsync()
        {
            var healthCenters = await _healthCenterService.GetAllHealthCentersAsync();

            if(healthCenters==null || healthCenters.Count()==0)
            {
                 _helper.DisplayError("No health centers found.");
                return;
            }
            _helper.DisplaySuccess("List of medical health centers:");
            foreach (var hc in healthCenters)
            {
                Console.WriteLine($"{hc.ToString()}");
            }
        }

      
    }

}
