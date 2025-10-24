using AutoFixture;
using FluentAssertions;
using Moq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using EntityFrameworkCore8Samples.Application.Services;
using EntityFrameworkCore8Samples.Application.Interfaces;
using EntityFrameworkCore8Samples.Infrastructure.Data;

namespace EntityFrameworkCore8Samples.Tests.Unit.Services;

public class DatabaseManagerTests
{
    private readonly Fixture _fixture;
    private readonly Mock<ApplicationDbContext> _mockContext;
    private readonly Mock<ILogger<DatabaseManager>> _mockLogger;
    private readonly DatabaseManager _service;

    public DatabaseManagerTests()
    {
        _fixture = new Fixture();
        _mockContext = new Mock<ApplicationDbContext>();
        _mockLogger = new Mock<ILogger<DatabaseManager>>();
        _service = new DatabaseManager(_mockContext.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task IsDatabaseAccessibleAsync_WhenDatabaseIsAccessible_ShouldReturnTrue()
    {
        // Arrange
        _mockContext.Setup(x => x.Database.CanConnectAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _service.IsDatabaseAccessibleAsync();

        // Assert
        result.Should().BeTrue();
        _mockContext.Verify(x => x.Database.CanConnectAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task IsDatabaseAccessibleAsync_WhenDatabaseIsNotAccessible_ShouldReturnFalse()
    {
        // Arrange
        _mockContext.Setup(x => x.Database.CanConnectAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _service.IsDatabaseAccessibleAsync();

        // Assert
        result.Should().BeFalse();
        _mockContext.Verify(x => x.Database.CanConnectAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task IsDatabaseAccessibleAsync_WhenExceptionOccurs_ShouldReturnFalse()
    {
        // Arrange
        _mockContext.Setup(x => x.Database.CanConnectAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database connection failed"));

        // Act
        var result = await _service.IsDatabaseAccessibleAsync();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task CreateDatabaseIfNotExistsAsync_WhenDatabaseDoesNotExist_ShouldCreateDatabase()
    {
        // Arrange
        _mockContext.Setup(x => x.Database.EnsureCreatedAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _service.CreateDatabaseIfNotExistsAsync();

        // Assert
        result.Should().BeTrue();
        _mockContext.Verify(x => x.Database.EnsureCreatedAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateDatabaseIfNotExistsAsync_WhenDatabaseAlreadyExists_ShouldReturnFalse()
    {
        // Arrange
        _mockContext.Setup(x => x.Database.EnsureCreatedAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _service.CreateDatabaseIfNotExistsAsync();

        // Assert
        result.Should().BeFalse();
        _mockContext.Verify(x => x.Database.EnsureCreatedAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateDatabaseIfNotExistsAsync_WhenExceptionOccurs_ShouldReturnFalse()
    {
        // Arrange
        _mockContext.Setup(x => x.Database.EnsureCreatedAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database creation failed"));

        // Act
        var result = await _service.CreateDatabaseIfNotExistsAsync();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task ApplyMigrationsAsync_WhenMigrationsExist_ShouldApplyMigrations()
    {
        // Arrange
        _mockContext.Setup(x => x.Database.MigrateAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _service.ApplyMigrationsAsync();

        // Assert
        result.Should().BeTrue();
        _mockContext.Verify(x => x.Database.MigrateAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ApplyMigrationsAsync_WhenExceptionOccurs_ShouldReturnFalse()
    {
        // Arrange
        _mockContext.Setup(x => x.Database.MigrateAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Migration failed"));

        // Act
        var result = await _service.ApplyMigrationsAsync();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task ExecuteDatabaseScriptsAsync_WhenScriptsExist_ShouldExecuteScripts()
    {
        // Arrange
        _mockContext.Setup(x => x.Database.ExecuteSqlRawAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _service.ExecuteDatabaseScriptsAsync();

        // Assert
        result.Should().BeTrue();
        _mockContext.Verify(x => x.Database.ExecuteSqlRawAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce);
    }

    [Fact]
    public async Task ExecuteDatabaseScriptsAsync_WhenExceptionOccurs_ShouldReturnFalse()
    {
        // Arrange
        _mockContext.Setup(x => x.Database.ExecuteSqlRawAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Script execution failed"));

        // Act
        var result = await _service.ExecuteDatabaseScriptsAsync();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task GetDatabaseStatisticsAsync_ShouldReturnStatistics()
    {
        // Arrange
        var mockDbSet = new Mock<DbSet<object>>();
        _mockContext.Setup(x => x.Set<object>()).Returns(mockDbSet.Object);
        mockDbSet.Setup(x => x.CountAsync(It.IsAny<CancellationToken>())).ReturnsAsync(100);

        // Act
        var result = await _service.GetDatabaseStatisticsAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().ContainKey("TotalRecords");
    }

    [Fact]
    public async Task GetDatabaseStatisticsAsync_WhenExceptionOccurs_ShouldReturnEmptyDictionary()
    {
        // Arrange
        _mockContext.Setup(x => x.Set<object>()).Throws(new Exception("Statistics failed"));

        // Act
        var result = await _service.GetDatabaseStatisticsAsync();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task SeedDataAsync_WhenSeedingIsEnabled_ShouldSeedData()
    {
        // Arrange
        var mockDbSet = new Mock<DbSet<object>>();
        _mockContext.Setup(x => x.Set<object>()).Returns(mockDbSet.Object);
        mockDbSet.Setup(x => x.AddRangeAsync(It.IsAny<IEnumerable<object>>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _mockContext.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(100);

        // Act
        var result = await _service.SeedDataAsync(100, true);

        // Assert
        result.Should().BeTrue();
        _mockContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task SeedDataAsync_WhenSeedingIsDisabled_ShouldReturnTrue()
    {
        // Act
        var result = await _service.SeedDataAsync(100, false);

        // Assert
        result.Should().BeTrue();
        _mockContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task SeedDataAsync_WhenExceptionOccurs_ShouldReturnFalse()
    {
        // Arrange
        _mockContext.Setup(x => x.Set<object>()).Throws(new Exception("Seeding failed"));

        // Act
        var result = await _service.SeedDataAsync(100, true);

        // Assert
        result.Should().BeFalse();
    }
}



