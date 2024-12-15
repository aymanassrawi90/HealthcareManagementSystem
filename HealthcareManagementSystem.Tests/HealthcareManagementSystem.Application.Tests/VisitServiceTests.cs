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
    public class VisitServiceTests
    {
        private readonly Mock<IRepository<Visit>> _visitRepositoryMock;
        private readonly VisitService _visitService;

        public VisitServiceTests()
        {
            _visitRepositoryMock = new Mock<IRepository<Visit>>();
            _visitService = new VisitService(_visitRepositoryMock.Object);
        }

        [Fact]
        public async Task RegisterVisitAsync_ShouldAddVisit()
        {
            // Arrange
            var visit = new Visit { Id = 1, Reason = "Consultation" };

            // Act
            await _visitService.RegisterVisitAsync(visit);

            // Assert
            _visitRepositoryMock.Verify(repo => repo.Add(visit), Times.Once);
            _visitRepositoryMock.Verify(repo => repo.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task GetTotalVisitsForHealthCenterAsync_ShouldReturnCorrectCount()
        {
            // Arrange
            int healthCenterId = 1;
            _visitRepositoryMock
                .Setup(repo => repo.CountAsync(It.IsAny<Expression<Func<Visit, bool>>[]>()))
                .ReturnsAsync(3);

            // Act
            var result = await _visitService.GetTotalVisitsForHealthCenterAsync(healthCenterId);

            // Assert
            Assert.Equal(3, result);
        }

        [Fact]
        public async Task GetAllVisitsAsync_ShouldReturnAllVisits()
        {
            // Arrange
            var visits = new List<Visit>
        {
            new Visit { Id = 1 },
            new Visit { Id = 2 }
        };

            _visitRepositoryMock.Setup(repo => repo.GetItemsAsync(It.IsAny<Expression<Func<Visit, bool>>[]>(), null, null, null, null)).ReturnsAsync(visits);

            // Act
            var result = await _visitService.GetAllVisitsAsync(0);

            // Assert
            Assert.Equal(visits.Count, result.Count());
        }

        [Fact]
        public async Task GetVisitsByIdAsync_ShouldReturnVisit()
        {
            // Arrange
            int visitId = 1;
            var visit = new Visit { Id = visitId };

            _visitRepositoryMock.Setup(repo => repo.GetItemByIdAsync(visitId)).ReturnsAsync(visit);

            // Act
            var result = await _visitService.GetVisitsByIdAsync(visitId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(visitId, result.Id);
        }

        [Fact]
        public async Task GetVisitsByFilterAsync_ShouldReturnFilteredVisits()
        {
            // Arrange
            var filter = new VisitListFilter { ReasonForVisit = "Consultation" };
            var visits = new List<Visit>
        {
            new Visit { Id = 1, Reason = "Consultation" },
            new Visit { Id = 2, Reason = "Follow-up" }
        };

            var expectedResult = new List<Visit>
        {
            new Visit { Id = 1, Reason = "Consultation" },
           
        };



            _visitRepositoryMock.Setup(repo => repo.GetItemsAsync(It.IsAny<Expression<Func<Visit, bool>>[]>(), null, filter, null, null)).ReturnsAsync(expectedResult);


        
            // Act
            var result = await _visitService.GetVisitsByFilterAsync(filter);

            // Assert
            Assert.Single(result);
            Assert.Equal("Consultation", result.First().Reason);
        }
    }

}
