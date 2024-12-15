using HealthcareManagementSystem.ConsoleApp;
using HealthcareManagementSystem.Domain.Services;
using HealthcareManagementSystem.Domain;
using HealthcareManagementSystem.Infrastructure.Exceptions;
using Moq;
using Xunit;

namespace HealthcareManagementSystem.Tests.HealthcareManagementSystem.ConsoleApp.Tests
{

    public class MedicalCenterManagementTests
    {
        private readonly Mock<IHealthCenterService> _mockHealthCenterService;
        private readonly Mock<IHelper> _mockHelper;
        private readonly MedicalCenterManagement _medicalCenterManagement;

        public MedicalCenterManagementTests()
        {
            _mockHealthCenterService = new Mock<IHealthCenterService>();
            _mockHelper = new Mock<IHelper>();

            _medicalCenterManagement = new MedicalCenterManagement(
                _mockHealthCenterService.Object,
                _mockHelper.Object
            );
        }

        [Fact]
        public async Task AddHealthCenterAsync_ShouldAddHealthCenter_WithValidInput()
        {
            // Arrange
            _mockHelper.Setup(h => h.ReadNonEmptyInput(It.IsAny<string>())).Returns("Test Center");
            _mockHelper.Setup(h => h.ReadNonEmptyInput(It.IsAny<string>())).Returns("Ramallah");
            _mockHelper.Setup(h => h.ReadNonEmptyInput(It.IsAny<string>())).Returns("Hospital");

            // Act
            await _medicalCenterManagement.AddHealthCenterAsync();

            // Assert
            _mockHelper.Verify(h => h.DisplaySuccess(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task AddHealthCenterAsync_ShouldDisplayError_WhenValidationFails()
        {
            // Arrange
            _mockHelper.Setup(h => h.ReadNonEmptyInput(It.IsAny<string>())).Returns("Test Center");
            _mockHelper.Setup(h => h.ReadNonEmptyInput(It.IsAny<string>())).Returns("Ramallah");
            _mockHelper.Setup(h => h.ReadNonEmptyInput(It.IsAny<string>())).Returns("Hospital");

            _mockHealthCenterService.Setup(service => service.AddHealthCenterAsync(It.IsAny<HealthCenter>()))
                .ThrowsAsync(new ValidateModelException("Validation failed"));

            // Act
            await _medicalCenterManagement.AddHealthCenterAsync();

            // Assert
            _mockHelper.Verify(h => h.DisplayValidationErrors(It.IsAny<ValidateModelException>()), Times.Once);
        }

        [Fact]
        public async Task UpdateHealthCenterAsync_ShouldUpdateHealthCenter_WithValidInput()
        {
            // Arrange
            var healthCenter = new HealthCenter { Id = 1, Name = "Old Center", Location = "Old Location", Type = "Old Type" };

            _mockHelper.Setup(h => h.ReadInt(It.IsAny<string>())).Returns(1);
            _mockHelper.Setup(h => h.ReadOptionalInput(It.IsAny<string>(), It.IsAny<string>())).Returns("Updated Center");

            _mockHealthCenterService.Setup(service => service.GetHealthCenterByIdAsync(1)).ReturnsAsync(healthCenter);

            // Act
            await _medicalCenterManagement.UpdateHealthCenterAsync();

            // Assert
            
            _mockHelper.Verify(h => h.DisplaySuccess(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task UpdateHealthCenterAsync_ShouldDisplayError_WhenHealthCenterNotFound()
        {
            // Arrange
            _mockHelper.Setup(h => h.ReadInt(It.IsAny<string>())).Returns(99);
            _mockHealthCenterService.Setup(service => service.GetHealthCenterByIdAsync(99)).ReturnsAsync((HealthCenter)null);

            // Act
            await _medicalCenterManagement.UpdateHealthCenterAsync();

            // Assert
            _mockHelper.Verify(h => h.DisplayError("medical health center not found."), Times.Once);
        }

        [Fact]
        public async Task DisplayAllHealthCentersAsync_ShouldDisplayHealthCenters_WhenHealthCentersExist()
        {
            // Arrange
            var healthCenters = new List<HealthCenter>
            {
                new HealthCenter { Name = "Health Center 1", Location = "Ramallah", Type = "Clinic" },
                new HealthCenter { Name = "Health Center 2", Location = "Nablus", Type = "Hospital" }
            };

            _mockHealthCenterService.Setup(service => service.GetAllHealthCentersAsync()).ReturnsAsync(healthCenters);

            // Act
            await _medicalCenterManagement.DisplayAllHealthCentersAsync();

            // Assert
            _mockHelper.Verify(h => h.DisplaySuccess(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task DisplayAllHealthCentersAsync_ShouldDisplayError_WhenNoHealthCentersExist()
        {
            // Arrange
            _mockHealthCenterService.Setup(service => service.GetAllHealthCentersAsync()).ReturnsAsync(new List<HealthCenter>());

            // Act
            await _medicalCenterManagement.DisplayAllHealthCentersAsync();

            // Assert
            _mockHelper.Verify(h => h.DisplayError("No health centers found."), Times.Once);
        }




    }
}
