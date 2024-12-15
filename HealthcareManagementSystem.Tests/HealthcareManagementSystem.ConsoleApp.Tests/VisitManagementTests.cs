using HealthcareManagementSystem.ConsoleApp;
using HealthcareManagementSystem.Domain;
using HealthcareManagementSystem.Domain.Services;
using Moq;
using Xunit;

namespace HealthcareManagementSystem.Tests.HealthcareManagementSystem.ConsoleApp.Tests
{
    public class VisitManagementTests
    {
        private readonly Mock<IVisitService> _mockVisitService;
        private readonly Mock<IPatientService> _mockPatientService;
        private readonly Mock<IHealthCenterService> _mockHealthCenterService;
        private readonly Mock<IHelper> _mockHelper;
        private readonly VisitManagement _visitManagement;

        public VisitManagementTests()
        {
            _mockVisitService = new Mock<IVisitService>();
            _mockPatientService = new Mock<IPatientService>();
            _mockHealthCenterService = new Mock<IHealthCenterService>();
            _mockHelper = new Mock<IHelper>();

            _visitManagement = new VisitManagement(
                _mockVisitService.Object,
                _mockPatientService.Object,
                _mockHealthCenterService.Object,
                _mockHelper.Object
            );
            
        }

        [Fact]
        public async Task RegisterNewVisitAsync_ShouldAddVisit_WithValidInput()
        {
            // Arrange
            var patient = new Patient("Ahmed", 30, "M", "123456789", "Ramallah") { Id = 1 };
            var healthCenter = new HealthCenter("City Clinic", "Ramallah", "Clinic") { Id = 1 };

            _mockHelper.SetupSequence(h => h.ReadInt(It.IsAny<string>())).Returns(1).Returns(1);
            _mockHelper.Setup(h => h.ReadNonEmptyInput(It.IsAny<string>())).Returns("Checkup");

            _mockPatientService.Setup(service => service.GetPatientByIdAsync(1)).ReturnsAsync(patient);
            _mockHealthCenterService.Setup(service => service.GetHealthCenterByIdAsync(1)).ReturnsAsync(healthCenter);

            // Act
            await _visitManagement.RegisterNewVisitAsync();

            // Assert
            _mockVisitService.Verify(service => service.RegisterVisitAsync(It.Is<Visit>(v =>
                v.Patient.Id == patient.Id &&
                v.HealthCenter.Id == healthCenter.Id &&
                v.Reason == "Checkup"
            )), Times.Once);

            _mockHelper.Verify(h => h.DisplaySuccess(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task RegisterNewVisitAsync_ShouldDisplayError_WhenPatientNotFound()
        {
            // Arrange
            _mockHelper.Setup(h => h.ReadInt(It.IsAny<string>())).Returns(1);
            _mockPatientService.Setup(service => service.GetPatientByIdAsync(1)).ReturnsAsync((Patient)null);

            // Act
            await _visitManagement.RegisterNewVisitAsync();

            // Assert
            _mockHelper.Verify(h => h.DisplayError("Patient not found."), Times.Once);
        }

        [Fact]
        public async Task RegisterNewVisitAsync_ShouldDisplayError_WhenHealthCenterNotFound()
        {
            // Arrange
            var patient = new Patient("Ahmed", 30, "M", "123456789", "Ramallah") { Id = 1 };

            _mockHelper.Setup(h => h.ReadInt(It.IsAny<string>())).Returns(1);
            _mockPatientService.Setup(service => service.GetPatientByIdAsync(1)).ReturnsAsync(patient);
            _mockHealthCenterService.Setup(service => service.GetHealthCenterByIdAsync(1)).ReturnsAsync((HealthCenter)null);

            // Act
            await _visitManagement.RegisterNewVisitAsync();

            // Assert
            _mockHelper.Verify(h => h.DisplayError("Medical health center not found."), Times.Once);
        }

        [Fact]
        public async Task ViewVisitsForPatientAsync_ShouldDisplayVisits_WhenVisitsExist()
        {
            // Arrange
            var visits = new List<Visit>
        {
            new Visit(new Patient("Ahmed", 30, "M", "056988888", "Ramallah"), new HealthCenter("City Clinic", "Ramallah","Clinic"), "Checkup")
            {
                Id = 1
            }
        };

            _mockHelper.Setup(h => h.ReadInt(It.IsAny<string>())).Returns(1);
            _mockVisitService.Setup(service => service.GetVisitsForPatientAsync(1)).ReturnsAsync(visits);

            // Act
            await _visitManagement.ViewVisitsForPatientAsync();

            // Assert
            _mockHelper.Verify(h => h.DisplaySuccess(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task ViewVisitsForPatientAsync_ShouldDisplayError_WhenNoVisitsExist()
        {
            // Arrange
            _mockHelper.Setup(h => h.ReadInt(It.IsAny<string>())).Returns(1);
            _mockVisitService.Setup(service => service.GetVisitsForPatientAsync(1)).ReturnsAsync(new List<Visit>());

            // Act
            await _visitManagement.ViewVisitsForPatientAsync();

            // Assert
            _mockHelper.Verify(h => h.DisplayError("No visits found."), Times.Once);
        }

        [Fact]
        public async Task ViewTotalVisitsForHealthCenterAsync_ShouldDisplayTotalVisits()
        {
            // Arrange
            _mockHelper.Setup(h => h.ReadInt(It.IsAny<string>())).Returns(1);
            _mockVisitService.Setup(service => service.GetTotalVisitsForHealthCenterAsync(1)).ReturnsAsync(10);

            // Act
            await _visitManagement.ViewTotalVisitsForHealthCenterAsync();

            // Assert
            _mockHelper.Verify(h => h.DisplaySuccess("Total Visits: 10"), Times.Once);
        }

     
    }
}
