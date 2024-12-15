using HealthcareManagementSystem.ConsoleApp;
using HealthcareManagementSystem.Domain.Services;
using HealthcareManagementSystem.Domain;
using Moq;
using Xunit;
using HealthcareManagementSystem.Infrastructure.ListFilters;

namespace HealthcareManagementSystem.Tests.HealthcareManagementSystem.ConsoleApp.Tests
{
    public class MedicalRecordManagementTests
    {
        private readonly Mock<IVisitService> _mockVisitService;
        private readonly Mock<IPatientService> _mockPatientService;
        private readonly Mock<IMedicalRecordService> _mockMedicalRecordService;
        private readonly Mock<IDiagnosisService> _mockDiagnosisService;
        private readonly Mock<IHelper> _mockHelper;
        private readonly MedicalRecordManagement _medicalRecordManagement;

        public MedicalRecordManagementTests()
        {
            _mockVisitService = new Mock<IVisitService>();
            _mockPatientService = new Mock<IPatientService>();
            _mockMedicalRecordService = new Mock<IMedicalRecordService>();
            _mockDiagnosisService = new Mock<IDiagnosisService>();
            _mockHelper = new Mock<IHelper>();

            _medicalRecordManagement = new MedicalRecordManagement(
                _mockMedicalRecordService.Object,
                _mockVisitService.Object,
                _mockPatientService.Object,
                _mockDiagnosisService.Object,
                _mockHelper.Object
            );
        }

        [Fact]
        public async Task CreateMedicalRecordAsync_Should_CreateRecord_When_ValidInput()
        {
            // Arrange
            int patientId = 1;
            int visitId = 1;
            var patient = new Patient { Id = patientId, Name = "Ali Ahmad" , Gender="M",Age=36 , Phone="0568111111" , Address="Ramallah" };
            var visit = new Visit { Reason="Check up" };
            visit.SetPatient(patient);
            _mockHelper.Setup(h => h.ReadInt(It.IsAny<string>())).Returns(patientId);
            _mockPatientService.Setup(s => s.GetPatientByIdAsync(patientId)).ReturnsAsync(patient);
            _mockVisitService.Setup(s => s.GetVisitsByFilterAsync(It.IsAny<VisitListFilter>()))
                .ReturnsAsync(new List<Visit> { visit });


            _mockHelper.SetupSequence(h => h.ReadOptionalInput(It.IsAny<string>(), It.IsAny<string>())).Returns("test").
                            Returns("done").Returns("Blod test").Returns("done").Returns("1").Returns("done");
            
            
            _mockMedicalRecordService.Setup(s => s.CreateMedicalRecordAsync(It.IsAny<MedicalRecord>())).Returns(Task.CompletedTask);

            var Dig = new List<Diagnosis> { new Diagnosis("Flue","S")};
            _mockDiagnosisService.Setup(d => d.GetAllDiagnosesAsync()).Returns(Task.FromResult((IEnumerable<Diagnosis>)Dig));
            // Act
            await _medicalRecordManagement.CreateMedicalRecordAsync();

            // Assert
            _mockMedicalRecordService.Verify(s => s.CreateMedicalRecordAsync(It.IsAny<MedicalRecord>()), Times.Once);
            _mockPatientService.Verify(s => s.UpdatePatientAsync(It.IsAny<Patient>()), Times.Once);
        }

        [Fact]
        public async Task CreateMedicalRecordAsync_Should_DisplayError_When_PatientNotFound()
        {
            // Arrange
            int patientId = 1;
            _mockHelper.Setup(h => h.ReadInt(It.IsAny<string>())).Returns(patientId);
            _mockPatientService.Setup(s => s.GetPatientByIdAsync(patientId)).ReturnsAsync((Patient)null);

            // Act
            await _medicalRecordManagement.CreateMedicalRecordAsync();

            // Assert
            _mockHelper.Verify(h => h.DisplayError(It.Is<string>(msg => msg.Contains("Patient not found."))), Times.Once);
            _mockMedicalRecordService.Verify(s => s.CreateMedicalRecordAsync(It.IsAny<MedicalRecord>()), Times.Never);
        }

        [Fact]
        public async Task ViewPatientMedicalHistory_Should_DisplayHistory_When_ValidPatient()
        {
            // Arrange
            int patientId = 1;
            var patient = new Patient { Id = patientId, Name = "Ahmad Ali" };
            var medicalRecords = new List<MedicalRecord>();
            var item1 = new MedicalRecord(new Visit(patient, new HealthCenter(), "CheckUp"));
            item1.AddTreatment("Medication");
            item1.AddMedicalTest("Blood Test");
            item1.AddDiagnosis(new Diagnosis { Name = "Flu", Severity = "s" });




            _mockHelper.Setup(h => h.ReadInt(It.IsAny<string>())).Returns(patientId);
            _mockPatientService.Setup(s => s.GetPatientByIdAsync(patientId)).ReturnsAsync(patient);
            _mockMedicalRecordService.Setup(s => s.GetMedicalRecordByFilterAsync(It.IsAny<MedicalRecordListFilter>()))
                .ReturnsAsync(medicalRecords);

            // Act
            await _medicalRecordManagement.ViewPatientMedicalHistory();

            // Assert
            _mockHelper.Verify(h => h.DisplaySuccess(It.IsAny<string>()), Times.AtLeastOnce);
        }

        [Fact]
        public async Task ViewPatientMedicalHistory_Should_DisplayError_When_PatientNotFound()
        {
            // Arrange
            int patientId = 1;
            _mockHelper.Setup(h => h.ReadInt(It.IsAny<string>())).Returns(patientId);
            _mockPatientService.Setup(s => s.GetPatientByIdAsync(patientId)).ReturnsAsync((Patient)null);

            // Act
            await _medicalRecordManagement.ViewPatientMedicalHistory();

            // Assert
            _mockHelper.Verify(h => h.DisplayError(It.Is<string>(msg => msg.Contains("patient not found"))), Times.Once);
        }

      
    }
}
