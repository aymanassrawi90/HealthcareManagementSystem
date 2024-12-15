using Moq;
using Xunit;
using HealthcareManagementSystem.Application.Services;
using HealthcareManagementSystem.Domain;
using HealthcareManagementSystem.Domain.Repository;
using HealthcareManagementSystem.Domain.Services;
using Assert = Xunit.Assert;
using HealthcareManagementSystem.Infrastructure.ListFilters;
using HealthcareManagementSystem.Infrastructure.Exceptions;
using System.Linq.Expressions;
using FluentAssertions;

namespace HealthcareManagementSystem.Tests.HealthcareManagementSystem.Application.Tests
{
    public class DiagnosisServiceTests
    {
        private readonly Mock<IRepository<Diagnosis>> _diagnosisRepositoryMock;
        private readonly Mock<IMedicalRecordService> _medicalRecordServiceMock;
        private readonly DiagnosisService _diagnosisService;

        public DiagnosisServiceTests()
        {
            _diagnosisRepositoryMock = new Mock<IRepository<Diagnosis>>();
            _medicalRecordServiceMock = new Mock<IMedicalRecordService>();
            _diagnosisService = new DiagnosisService(_diagnosisRepositoryMock.Object, _medicalRecordServiceMock.Object);
        }

        [Fact]
        public async Task CreateDiagnosisAsync_ShouldThrowConflictException_WhenDiagnosisNameExists()
        {
            // Arrange
            var diagnosis = new Diagnosis { Name = "Flu" };


            _diagnosisRepositoryMock.Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<Diagnosis, bool>>[]>())).
              Returns(Task.FromResult(true));

           

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ConflictException>(() => _diagnosisService.CreateDiagnosisAsync(diagnosis));
        

            exception.GetErrorString("Name")
                  .Should()
                  .Be("the Name Flu already exsist");
        }

        [Fact]
        public async Task CreateDiagnosisAsync_ShouldAddDiagnosis_WhenNotExists()
        {
            // Arrange
            var diagnosis = new Diagnosis { Name = "Cold" };
            _diagnosisRepositoryMock.Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<Diagnosis, bool>>[]>())).
             Returns(Task.FromResult(false));

            // Act
            await _diagnosisService.CreateDiagnosisAsync(diagnosis);

            // Assert

            _diagnosisRepositoryMock.Verify(m => m.Add(It.IsAny<Diagnosis>()));
            _diagnosisRepositoryMock.Verify(m => m.SaveAsync());
         
        }

        [Fact]
        public async Task CreateDiagnosesAsync_ShouldReturnSkippedDiagnoses_WhenAlreadyExists()
        {
            // Arrange
            var diagnoses = new List<Diagnosis>
            {
                new Diagnosis { Name = "Flu" },
                new Diagnosis { Name = "Cold" }
            };

        


            _diagnosisRepositoryMock
       .Setup(r => r.ExistsAsync(It.Is<Expression<Func<Diagnosis, bool>>>(expr =>
           ExpressionMatchesName(expr, "Flu"))))
       .ReturnsAsync(true);

            _diagnosisRepositoryMock
                .Setup(r => r.ExistsAsync(It.Is<Expression<Func<Diagnosis, bool>>>(expr =>
                    ExpressionMatchesName(expr, "Cold"))))
                .ReturnsAsync(false);

            // Act
            var skippedDiagnoses = await _diagnosisService.CreateDiagnosesAsync(diagnoses);

            // Assert
            Assert.Single(skippedDiagnoses); // Only "Flu" should be skipped
            Assert.Equal("Flu", skippedDiagnoses.First().Name);
            _diagnosisRepositoryMock.Verify(repo => repo.Add(It.IsAny<Diagnosis>()), Times.Once); // "Cold" should be added
            _diagnosisRepositoryMock.Verify(repo => repo.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllDiagnosesAsync_ShouldReturnDiagnoses()
        {
            // Arrange
            var diagnoses = new List<Diagnosis>
            {
                new Diagnosis { Name = "Flu" },
                new Diagnosis { Name = "Cold" }
            };
           
                 _diagnosisRepositoryMock
                .Setup(r => r.GetItemsAsync(It.IsAny<Expression<Func<Diagnosis, bool>>[]>(), null, null, null, null))
                .Returns(Task.FromResult(diagnoses));


                

            // Act
            var result = await _diagnosisService.GetAllDiagnosesAsync();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(result, d => d.Name == "Flu");
            Assert.Contains(result, d => d.Name == "Cold");
        }

        [Fact]
        public async Task GetMostCommonDiagnosesAsync_ShouldReturnMostCommonDiagnoses()
        {
            // Arrange
            var medicalRecords = new List<MedicalRecord>();
            

            var item1 = new MedicalRecord();
            item1.AddDiagnosis(new Diagnosis { Name = "Cold" });
            var item2 = new MedicalRecord();
            item2.AddDiagnosis(new Diagnosis { Name = "Cold" });
            var item3 = new MedicalRecord();
            item3.AddDiagnosis(new Diagnosis { Name = "Flu" });
            item3.AddDiagnosis(new Diagnosis { Name = "Cold" });
            medicalRecords.Add(item1);
            medicalRecords.Add(item2);
            medicalRecords.Add(item3);

            _medicalRecordServiceMock.Setup(service => service.GetMedicalRecordByFilterAsync(It.IsAny<MedicalRecordListFilter>()))
                .ReturnsAsync(medicalRecords); // Mock return value

            // Act
            var result =( await _diagnosisService.GetMostCommonDiagnosesAsync(1)).ToList();

            // Assert
            
            Assert.Equal(2, result.Count()); // Should return 2 distinct diagnoses
            Assert.Equal(result[0].Count, 3);
            Assert.Equal(result[1].Count, 1);
        }

        // Helper method to match expressions
        private bool ExpressionMatchesName(Expression<Func<Diagnosis, bool>> expr, string name)
        {
            // Compile the expression into a function
            var func = expr.Compile();

            // Create a diagnosis with the target name and test the function
            return func(new Diagnosis { Name = name });
        }
    }
}
