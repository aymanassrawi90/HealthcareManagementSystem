using Moq;
using Xunit;
using HealthcareManagementSystem.Application.Services;
using HealthcareManagementSystem.Domain;
using HealthcareManagementSystem.Domain.Repository;
using Assert = Xunit.Assert;
using System.Linq.Expressions;

namespace HealthcareManagementSystem.Tests.HealthcareManagementSystem.Application.Tests
{
    public class HealthCenterServiceTests
    {
        private readonly Mock<IRepository<HealthCenter>> _healthCenterRepositoryMock;
        private readonly HealthCenterService _healthCenterService;

        public HealthCenterServiceTests()
        {
            _healthCenterRepositoryMock = new Mock<IRepository<HealthCenter>>();
            _healthCenterService = new HealthCenterService(_healthCenterRepositoryMock.Object);
        }

        [Fact]
        public async Task AddHealthCenterAsync_ShouldAddHealthCenter()
        {
            // Arrange
            var healthCenter = new HealthCenter { Id = 1, Name = "Health Center 1" };

            // Act
            await _healthCenterService.AddHealthCenterAsync(healthCenter);

            // Assert
            _healthCenterRepositoryMock.Verify(repo => repo.Add(It.Is<HealthCenter>(hc => hc == healthCenter)), Times.Once);
            _healthCenterRepositoryMock.Verify(repo => repo.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllHealthCentersAsync_ShouldReturnAllHealthCenters()
        {
            // Arrange
            var healthCenters = new List<HealthCenter>
        {
            new HealthCenter { Id = 1, Name = "Health Center 1" },
            new HealthCenter { Id = 2, Name = "Health Center 2" }
        };

            _healthCenterRepositoryMock
                .Setup(repo => repo.GetItemsAsync(It.IsAny<Expression<Func<HealthCenter, bool>>[]>(), null, null, null, null))
                .ReturnsAsync(healthCenters);

            // Act
            var result = await _healthCenterService.GetAllHealthCentersAsync();

            // Assert
            Assert.Equal(healthCenters.Count, result.Count());
            Assert.Contains(healthCenters[0], result);
            Assert.Contains(healthCenters[1], result);
        }

        [Fact]
        public async Task GetHealthCenterByIdAsync_ShouldReturnHealthCenterById()
        {
            // Arrange
            var healthCenter = new HealthCenter { Id = 1, Name = "Health Center 1" };

            _healthCenterRepositoryMock
                .Setup(repo => repo.GetItemByIdAsync(1))
                .ReturnsAsync(healthCenter);

            // Act
            var result = await _healthCenterService.GetHealthCenterByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(healthCenter.Id, result.Id);
            Assert.Equal(healthCenter.Name, result.Name);
        }

        [Fact]
        public async Task GetHealthCenterByIdAsync_ShouldReturnNull_WhenHealthCenterDoesNotExist()
        {
            // Arrange
            _healthCenterRepositoryMock
                .Setup(repo => repo.GetItemByIdAsync(1))
                .ReturnsAsync((HealthCenter)null);

            // Act
            var result = await _healthCenterService.GetHealthCenterByIdAsync(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateHealthCenterAsync_ShouldUpdateHealthCenter()
        {
            // Arrange
            var healthCenter = new HealthCenter { Id = 1, Name = "Updated Health Center" };

            // Act
            await _healthCenterService.UpdateHealthCenterAsync(healthCenter);

            // Assert
            _healthCenterRepositoryMock.Verify(repo => repo.Update(It.Is<HealthCenter>(hc => hc == healthCenter)), Times.Once);
            _healthCenterRepositoryMock.Verify(repo => repo.SaveAsync(), Times.Once);
        }
    }
}
