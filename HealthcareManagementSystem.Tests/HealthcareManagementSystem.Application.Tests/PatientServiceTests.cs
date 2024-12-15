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
    public class PatientServiceTests
    {
        private readonly Mock<IRepository<Patient>> _patientRepositoryMock;
        private readonly PatientService _patientService;

        public PatientServiceTests()
        {
            _patientRepositoryMock = new Mock<IRepository<Patient>>();
            _patientService = new PatientService(_patientRepositoryMock.Object);
        }

        [Fact]
        public async Task AddPatientAsync_ShouldThrowInvalidModelException_WhenPhoneExists()
        {
            // Arrange
            var patient = new Patient { Phone = "123456789", Age = 25, Gender = "M" };
            _patientRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Patient, bool>>>()))
                .ReturnsAsync(true);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidModelException>(() => _patientService.AddPatientAsync(patient));
            Assert.Contains("Phone number already exists", exception.Errors.Values);
        }

        [Fact]
        public async Task AddPatientAsync_ShouldAddPatient_WhenValid()
        {
            // Arrange
            var patient = new Patient { Phone = "0568111111", Age = 25, Gender = "M" };
            _patientRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Patient, bool>>>()))
                .ReturnsAsync(false);

            // Act
            await _patientService.AddPatientAsync(patient);

            // Assert
            _patientRepositoryMock.Verify(repo => repo.Add(patient), Times.Once);
            _patientRepositoryMock.Verify(repo => repo.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdatePatientAsync_ShouldThrowInvalidModelException_WhenPhoneExistsForDifferentPatient()
        {
            // Arrange
            var patient = new Patient { Id = 1, Phone = "123456789", Age = 30, Gender = "F" };
            _patientRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Patient, bool>>>()))
                .ReturnsAsync(true);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidModelException>(() => _patientService.UpdatePatientAsync(patient));
            Assert.Contains("Phone number already exists", exception.Errors.Values);
        }

        [Fact]
        public async Task UpdatePatientAsync_ShouldUpdatePatient_WhenValid()
        {
            // Arrange
            var patient = new Patient { Id = 1, Phone = "0568000000", Age = 25, Gender = "M" };
            _patientRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Patient, bool>>>()))
                .ReturnsAsync(false);

            // Act
            await _patientService.UpdatePatientAsync(patient);

            // Assert
            _patientRepositoryMock.Verify(repo => repo.Update(patient), Times.Once);
            _patientRepositoryMock.Verify(repo => repo.SaveAsync(), Times.Once);
        }

     
        [Fact]
        public async Task DeletePatientAsync_ShouldDeletePatient_WhenPatientExists()
        {
            // Arrange
            int patientId = 1;
            var patient = new Patient { Id = patientId };
            _patientRepositoryMock.Setup(repo => repo.GetItemByIdAsync(patientId)).ReturnsAsync(patient);

            // Act
            await _patientService.DeletePatientAsync(patientId);

            // Assert
            _patientRepositoryMock.Verify(repo => repo.Delete(patient), Times.Once);
            _patientRepositoryMock.Verify(repo => repo.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task Search_ShouldReturnFilteredPatients()
        {
            // Arrange
            var filter = new PatientListFilter { Name = "Ahmad" };
            var patients = new List<Patient> { new Patient { Name = "Ahmad Ali", Phone = "0568109595" } };
            _patientRepositoryMock.Setup(repo => repo.GetItemsAsync(It.IsAny<Expression<Func<Patient, bool>>[]>(), null, filter,null, null))
                .ReturnsAsync(patients);

            // Act
            var result = await _patientService.Search(filter);

            // Assert
            Assert.Single(result);
            Assert.Equal("Ahmad Ali", result.First().Name);
        }
    }

}
