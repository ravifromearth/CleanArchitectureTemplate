using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using EntityFrameworkCore8Samples.Application.Interfaces;
using EntityFrameworkCore8Samples.Application.Repositories;
using EntityFrameworkCore8Samples.Domain.Entities;
using EntityFrameworkCore8Samples.Infrastructure.Data;

namespace EntityFrameworkCore8Samples.Tests.Unit.Repositories;

public class RepositoryTests
{
    private readonly Fixture _fixture;
    private readonly Mock<ApplicationDbContext> _mockContext;
    private readonly Mock<DbSet<User>> _mockDbSet;
    private readonly Repository<User> _repository;

    public RepositoryTests()
    {
        _fixture = new Fixture();
        _mockContext = new Mock<ApplicationDbContext>();
        _mockDbSet = new Mock<DbSet<User>>();
        _repository = new Repository<User>(_mockContext.Object);
    }

    [Fact]
    public async Task GetByIdAsync_WhenEntityExists_ShouldReturnEntity()
    {
        // Arrange
        var userId = _fixture.Create<Guid>();
        var user = _fixture.Create<User>();
        user.Id = userId;

        _mockDbSet.Setup(x => x.FindAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        _mockContext.Setup(x => x.Set<User>()).Returns(_mockDbSet.Object);

        // Act
        var result = await _repository.GetByIdAsync(userId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(user);
        _mockDbSet.Verify(x => x.FindAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_WhenEntityDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        var userId = _fixture.Create<Guid>();

        _mockDbSet.Setup(x => x.FindAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);
        _mockContext.Setup(x => x.Set<User>()).Returns(_mockDbSet.Object);

        // Act
        var result = await _repository.GetByIdAsync(userId);

        // Assert
        result.Should().BeNull();
        _mockDbSet.Verify(x => x.FindAsync(userId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllEntities()
    {
        // Arrange
        var users = _fixture.CreateMany<User>(10).ToList();
        var mockQueryable = users.AsQueryable().BuildMockDbSet();

        _mockContext.Setup(x => x.Set<User>()).Returns(mockQueryable.Object);

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        result.Should().HaveCount(10);
        result.Should().BeEquivalentTo(users);
    }

    [Fact]
    public async Task FindAsync_WithPredicate_ShouldReturnMatchingEntities()
    {
        // Arrange
        var users = _fixture.CreateMany<User>(10).ToList();
        users[0].IsActive = true;
        users[1].IsActive = true;
        users[2].IsActive = false;

        var mockQueryable = users.AsQueryable().BuildMockDbSet();
        _mockContext.Setup(x => x.Set<User>()).Returns(mockQueryable.Object);

        // Act
        var result = await _repository.FindAsync(u => u.IsActive);

        // Assert
        result.Should().HaveCount(2);
        result.Should().OnlyContain(u => u.IsActive);
    }

    [Fact]
    public async Task AddAsync_ShouldAddEntity()
    {
        // Arrange
        var user = _fixture.Create<User>();
        _mockContext.Setup(x => x.Set<User>()).Returns(_mockDbSet.Object);
        _mockDbSet.Setup(x => x.AddAsync(user, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<User>.CreateMock(user));

        // Act
        await _repository.AddAsync(user);

        // Assert
        _mockDbSet.Verify(x => x.AddAsync(user, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task AddRangeAsync_ShouldAddMultipleEntities()
    {
        // Arrange
        var users = _fixture.CreateMany<User>(5).ToList();
        _mockContext.Setup(x => x.Set<User>()).Returns(_mockDbSet.Object);
        _mockDbSet.Setup(x => x.AddRangeAsync(users, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _repository.AddRangeAsync(users);

        // Assert
        _mockDbSet.Verify(x => x.AddRangeAsync(users, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateEntity()
    {
        // Arrange
        var user = _fixture.Create<User>();
        _mockContext.Setup(x => x.Set<User>()).Returns(_mockDbSet.Object);
        _mockDbSet.Setup(x => x.Update(user))
            .Returns(Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<User>.CreateMock(user));

        // Act
        await _repository.UpdateAsync(user);

        // Assert
        _mockDbSet.Verify(x => x.Update(user), Times.Once);
    }

    [Fact]
    public async Task UpdateRangeAsync_ShouldUpdateMultipleEntities()
    {
        // Arrange
        var users = _fixture.CreateMany<User>(5).ToList();
        _mockContext.Setup(x => x.Set<User>()).Returns(_mockDbSet.Object);
        _mockDbSet.Setup(x => x.UpdateRange(users))
            .Returns(users.Select(u => Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<User>.CreateMock(u)));

        // Act
        await _repository.UpdateRangeAsync(users);

        // Assert
        _mockDbSet.Verify(x => x.UpdateRange(users), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteEntity()
    {
        // Arrange
        var user = _fixture.Create<User>();
        _mockContext.Setup(x => x.Set<User>()).Returns(_mockDbSet.Object);
        _mockDbSet.Setup(x => x.Remove(user))
            .Returns(Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<User>.CreateMock(user));

        // Act
        await _repository.DeleteAsync(user);

        // Assert
        _mockDbSet.Verify(x => x.Remove(user), Times.Once);
    }

    [Fact]
    public async Task DeleteRangeAsync_ShouldDeleteMultipleEntities()
    {
        // Arrange
        var users = _fixture.CreateMany<User>(5).ToList();
        _mockContext.Setup(x => x.Set<User>()).Returns(_mockDbSet.Object);
        _mockDbSet.Setup(x => x.RemoveRange(users))
            .Returns(users.Select(u => Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<User>.CreateMock(u)));

        // Act
        await _repository.DeleteRangeAsync(users);

        // Assert
        _mockDbSet.Verify(x => x.RemoveRange(users), Times.Once);
    }

    [Fact]
    public async Task CountAsync_ShouldReturnCorrectCount()
    {
        // Arrange
        var users = _fixture.CreateMany<User>(15).ToList();
        var mockQueryable = users.AsQueryable().BuildMockDbSet();
        _mockContext.Setup(x => x.Set<User>()).Returns(mockQueryable.Object);

        // Act
        var result = await _repository.CountAsync();

        // Assert
        result.Should().Be(15);
    }

    [Fact]
    public async Task CountAsync_WithPredicate_ShouldReturnCorrectCount()
    {
        // Arrange
        var users = _fixture.CreateMany<User>(10).ToList();
        users[0].IsActive = true;
        users[1].IsActive = true;
        users[2].IsActive = false;

        var mockQueryable = users.AsQueryable().BuildMockDbSet();
        _mockContext.Setup(x => x.Set<User>()).Returns(mockQueryable.Object);

        // Act
        var result = await _repository.CountAsync(u => u.IsActive);

        // Assert
        result.Should().Be(2);
    }

    [Fact]
    public async Task ExistsAsync_WhenEntityExists_ShouldReturnTrue()
    {
        // Arrange
        var userId = _fixture.Create<Guid>();
        var users = _fixture.CreateMany<User>(5).ToList();
        users[0].Id = userId;

        var mockQueryable = users.AsQueryable().BuildMockDbSet();
        _mockContext.Setup(x => x.Set<User>()).Returns(mockQueryable.Object);

        // Act
        var result = await _repository.ExistsAsync(u => u.Id == userId);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task ExistsAsync_WhenEntityDoesNotExist_ShouldReturnFalse()
    {
        // Arrange
        var userId = _fixture.Create<Guid>();
        var users = _fixture.CreateMany<User>(5).ToList();

        var mockQueryable = users.AsQueryable().BuildMockDbSet();
        _mockContext.Setup(x => x.Set<User>()).Returns(mockQueryable.Object);

        // Act
        var result = await _repository.ExistsAsync(u => u.Id == userId);

        // Assert
        result.Should().BeFalse();
    }
}

public static class MockExtensions
{
    public static Mock<DbSet<T>> BuildMockDbSet<T>(this IQueryable<T> queryable) where T : class
    {
        var mockDbSet = new Mock<DbSet<T>>();
        mockDbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
        mockDbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
        mockDbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
        mockDbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());
        return mockDbSet;
    }
}



