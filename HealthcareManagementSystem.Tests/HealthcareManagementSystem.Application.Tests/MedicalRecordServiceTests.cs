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
    public class MedicalRecordServiceTests
    {
        private readonly Mock<IRepository<MedicalRecord>> _medicalRecordRepositoryMock;
        private readonly MedicalRecordService _medicalRecordService;

        public MedicalRecordServiceTests()
        {
            _medicalRecordRepositoryMock = new Mock<IRepository<MedicalRecord>>();
            _medicalRecordService = new MedicalRecordService(_medicalRecordRepositoryMock.Object);
        }

        [Fact]
        public async Task CreateMedicalRecordAsync_ShouldAddMedicalRecord()
        {
            // Arrange
            var medicalRecord = new MedicalRecord ();
            medicalRecord.AddTreatment("Treatment 1");

            // Act
            await _medicalRecordService.CreateMedicalRecordAsync(medicalRecord);

            // Assert
            _medicalRecordRepositoryMock.Verify(repo => repo.Add(It.Is<MedicalRecord>(mr => mr == medicalRecord)), Times.Once);
            _medicalRecordRepositoryMock.Verify(repo => repo.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task GetMedicalRecordByFilterAsync_ShouldReturnFilteredMedicalRecords()
        {
            // Arrange
            var filter = new MedicalRecordListFilter { Treatment = "Test Treatment" };
            var medicalRecords = new List<MedicalRecord>();
    ;
            var item1 = new MedicalRecord();
            item1.AddTreatment("Test Treatment");
            var item2 = new MedicalRecord();
            item2.AddTreatment("Other Treatment");
            medicalRecords.Add(item1 );
            medicalRecords.Add(item2);

            _medicalRecordRepositoryMock
                .Setup(repo => repo.GetItemsAsync(It.IsAny<Expression<Func<MedicalRecord, bool>>[]>(), null, filter, null, null))
                .ReturnsAsync(medicalRecords.Where(mr => mr.Treatment.Contains(filter.Treatment)).ToList());

            // Act
            var result = (await _medicalRecordService.GetMedicalRecordByFilterAsync(filter)).ToList();

            // Assert
            Assert.Single(result);
            Assert.Equal("Test Treatment", result.First().Treatment.First());
        }

        [Fact]
        public async Task GetMedicalRecordByVisitIdAsync_ShouldReturnCorrectMedicalRecord()
        {
            // Arrange
            var visitId = 10;
           
            var medicalRecords = new List<MedicalRecord>
        {
            new MedicalRecord (new Visit { Id = 5 }),
            new MedicalRecord ( new Visit { Id = 10 } )
        };

            _medicalRecordRepositoryMock
                .Setup(repo => repo.GetItemsAsync(It.IsAny<Expression<Func<MedicalRecord, bool>>[]>(), null, null, null, null))
                .ReturnsAsync(medicalRecords);

            // Act
            var result = await _medicalRecordService.GetMedicalRecordByVisitIdAsync(visitId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(visitId, result.Visit.Id);
        }

        [Fact]
        public async Task GetMedicalRecordByVisitIdAsync_ShouldReturnNull_WhenVisitIdNotFound()
        {
            // Arrange
            var visitId = 20;
            var medicalRecords = new List<MedicalRecord>
        {
            new MedicalRecord (new Visit { Id = 5 }),
            new MedicalRecord ( new Visit { Id = 10 } )
        };

            _medicalRecordRepositoryMock
                .Setup(repo => repo.GetItemsAsync(It.IsAny<Expression<Func<MedicalRecord, bool>>[]>(), null, null, null, null))
                .ReturnsAsync(medicalRecords);

            // Act
            var result = await _medicalRecordService.GetMedicalRecordByVisitIdAsync(visitId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateMedicalRecordAsync_ShouldUpdateMedicalRecord()
        {
            // Arrange
            var medicalRecord = new MedicalRecord ();
            medicalRecord.AddTreatment("Updated Treatment");

            // Act
            await _medicalRecordService.UpdateMedicalRecordAsync(medicalRecord);

            // Assert
            _medicalRecordRepositoryMock.Verify(repo => repo.Update(It.Is<MedicalRecord>(mr => mr == medicalRecord)), Times.Once);
            _medicalRecordRepositoryMock.Verify(repo => repo.SaveAsync(), Times.Once);
        }
    }
}
