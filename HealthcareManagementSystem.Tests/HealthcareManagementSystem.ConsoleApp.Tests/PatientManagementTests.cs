using HealthcareManagementSystem.ConsoleApp;
using HealthcareManagementSystem.Domain.Services;
using HealthcareManagementSystem.Domain;
using HealthcareManagementSystem.Infrastructure.ListFilters;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HealthcareManagementSystem.Tests.HealthcareManagementSystem.ConsoleApp.Tests
{
    public class PatientManagementTests
    {
        private readonly Mock<IPatientService> _mockPatientService;
        private readonly Mock<IHelper> _mockHelper;
        private readonly PatientManagement _patientManagement;

        public PatientManagementTests()
        {
            _mockPatientService = new Mock<IPatientService>();
            _mockHelper = new Mock<IHelper>();
            _patientManagement = new PatientManagement(_mockPatientService.Object, _mockHelper.Object);
        }

        [Fact]
        public async Task AddPatientAsync_ShouldCallAddPatient_WithValidInput()
        {
            // Arrange
            _mockHelper.SetupSequence(h => h.ReadNonEmptyInput(It.IsAny<string>()))
                       .Returns("Ahmed")
                   
                       .Returns("Ramallah");

            _mockHelper.Setup(_ => _.ReadPhone(It.IsAny<string>(), "")).Returns("0568111111");
            _mockHelper.Setup(h => h.ReadInt(It.IsAny<string>())).Returns(30);
            _mockHelper.Setup(h => h.ReadGender(It.IsAny<string>())).Returns("M");

            var newPatient = new Patient("Ahmed", 30, "M", "0568111111", "Ramallah");

            // Act
            await _patientManagement.AddPatientAsync();

            // Assert
            _mockPatientService.Verify(service => service.AddPatientAsync(It.Is<Patient>(p =>
                p.Name == newPatient.Name &&
                p.Age == newPatient.Age &&
                p.Gender == newPatient.Gender &&
                p.Phone == newPatient.Phone &&
                p.Address == newPatient.Address)), Times.Once);
        }

        [Fact]
        public async Task ViewAllPatientsAsync_ShouldDisplayPatients_WhenPatientsExist()
        {
            // Arrange
            var patients = new List<Patient>
        {
            new Patient("Ahmed", 30, "M", "0599111111", "Ramallah")
        };

            _mockPatientService.Setup(service => service.GetAllPatientsAsync()).ReturnsAsync(patients);

            // Act
            await _patientManagement.ViewAllPatientsAsync();

            // Assert
            _mockHelper.Verify(h => h.DisplaySuccess(It.IsAny<string>()), Times.AtLeastOnce);
        }

        [Fact]
        public async Task ViewAllPatientsAsync_ShouldDisplayError_WhenNoPatientsExist()
        {
            // Arrange
            _mockPatientService.Setup(service => service.GetAllPatientsAsync()).ReturnsAsync(new List<Patient>());

            // Act
            await _patientManagement.ViewAllPatientsAsync();

            // Assert
            _mockHelper.Verify(h => h.DisplayError("No patients found."), Times.Once);
        }

        [Fact]
        public async Task SearchPatientsAsync_ShouldCallSearch_WithCorrectFilter()
        {
            // Arrange
            _mockHelper.SetupSequence(h => h.ReadOptionalInput(It.IsAny<string>(),null))
                       .Returns("Ahmed")
                       .Returns("0568111111")
                      ;

            _mockHelper.Setup(h => h.ReadInt(It.IsAny<string>())).Returns(1);

            var filter = new PatientListFilter
            {
                Name = "Ahmed",
                Phone = "0568111111"

            };

            // Act
            await _patientManagement.SearchPatientsAsync();

            // Assert
            _mockPatientService.Verify(service => service.Search(It.Is<PatientListFilter>(f =>
                f.Name == filter.Name &&
                f.Phone == filter.Phone 
               )), Times.Once);
        }

        [Fact]
        public async Task UpdatePatientAsync_ShouldUpdateExistingPatient_WithValidData()
        {
            // Arrange
            var patient = new Patient("Ahmed", 30, "M", "0599111111", "Ramallah") { Id = 1 };

            _mockHelper.Setup(h => h.ReadInt(It.IsAny<string>())).Returns(1);
            _mockHelper.Setup(h => h.ReadOptionalInput(It.IsAny<string>(), It.IsAny<string>()))
                       .Returns((string prompt, string existingValue) => existingValue);

            _mockHelper.Setup(_ => _.ReadPhone(It.IsAny<string>(), It.IsAny<string>())).Returns("0568111111");
            _mockHelper.Setup(h => h.ReadIntOptional(It.IsAny<string>(),30))
                       .Returns(30);

            _mockPatientService.Setup(service => service.GetPatientByIdAsync(1)).ReturnsAsync(patient);

            // Act
            await _patientManagement.UpdatePatientAsync();

            // Assert
            _mockPatientService.Verify(service => service.UpdatePatientAsync(It.Is<Patient>(p =>
                p.Id == patient.Id &&
                p.Name == patient.Name &&
                p.Age == patient.Age &&
                p.Gender == patient.Gender &&
                p.Phone == "0568111111" &&
                p.Address == patient.Address)), Times.Once);
        }

        [Fact]
        public async Task UpdatePatientAsync_ShouldDisplayError_WhenPatientNotFound()
        {
            // Arrange
            _mockHelper.Setup(h => h.ReadInt(It.IsAny<string>())).Returns(1);
            _mockPatientService.Setup(service => service.GetPatientByIdAsync(1)).ReturnsAsync((Patient)null);

            // Act
            await _patientManagement.UpdatePatientAsync();

            // Assert
            _mockHelper.Verify(h => h.DisplayError("Patient not found."), Times.Once);
        }
    }



}
