using AutoFixture;
using FluentAssertions;
using EntityFrameworkCore8Samples.Domain.Entities;
using EntityFrameworkCore8Samples.Domain.Enums;

namespace EntityFrameworkCore8Samples.Tests.Unit.Entities;

public class UserTests
{
    private readonly Fixture _fixture;

    public UserTests()
    {
        _fixture = new Fixture();
    }

    [Fact]
    public void User_ShouldHaveRequiredProperties()
    {
        // Arrange & Act
        var user = _fixture.Create<User>();

        // Assert
        user.Should().NotBeNull();
        user.Id.Should().NotBeEmpty();
        user.Username.Should().NotBeNullOrEmpty();
        user.Email.Should().NotBeNullOrEmpty();
        user.FirstName.Should().NotBeNullOrEmpty();
        user.LastName.Should().NotBeNullOrEmpty();
        user.Status.Should().BeOfType<UserStatus>();
        user.Role.Should().BeOfType<UserRole>();
        user.CreatedAt.Should().BeBefore(DateTime.UtcNow);
        user.UpdatedAt.Should().BeOnOrAfter(user.CreatedAt);
    }

    [Fact]
    public void User_ShouldHaveValidEmailFormat()
    {
        // Arrange
        var user = _fixture.Create<User>();

        // Act & Assert
        user.Email.Should().MatchRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
    }

    [Fact]
    public void User_ShouldHaveValidPhoneNumberFormat()
    {
        // Arrange
        var user = _fixture.Create<User>();

        // Act & Assert
        if (!string.IsNullOrEmpty(user.PhoneNumber))
        {
            user.PhoneNumber.Should().MatchRegex(@"^[\+]?[1-9][\d]{0,15}$");
        }
    }

    [Fact]
    public void User_ShouldHaveValidDateOfBirth()
    {
        // Arrange
        var user = _fixture.Create<User>();

        // Act & Assert
        user.DateOfBirth.Should().BeBefore(DateTime.Now);
        user.DateOfBirth.Should().BeAfter(DateTime.Now.AddYears(-120));
    }

    [Fact]
    public void User_ShouldHaveValidCreatedAtAndUpdatedAt()
    {
        // Arrange
        var user = _fixture.Create<User>();

        // Act & Assert
        user.CreatedAt.Should().BeBefore(DateTime.UtcNow);
        user.UpdatedAt.Should().BeOnOrAfter(user.CreatedAt);
    }

    [Theory]
    [InlineData(UserStatus.Active)]
    [InlineData(UserStatus.Inactive)]
    [InlineData(UserStatus.Suspended)]
    [InlineData(UserStatus.Pending)]
    public void User_ShouldAcceptValidStatus(UserStatus status)
    {
        // Arrange
        var user = _fixture.Create<User>();

        // Act
        user.Status = status;

        // Assert
        user.Status.Should().Be(status);
    }

    [Theory]
    [InlineData(UserRole.Admin)]
    [InlineData(UserRole.User)]
    [InlineData(UserRole.Moderator)]
    [InlineData(UserRole.Guest)]
    public void User_ShouldAcceptValidRole(UserRole role)
    {
        // Arrange
        var user = _fixture.Create<User>();

        // Act
        user.Role = role;

        // Assert
        user.Role.Should().Be(role);
    }

    [Fact]
    public void User_ShouldHaveValidUsernameLength()
    {
        // Arrange
        var user = _fixture.Create<User>();

        // Act & Assert
        user.Username.Should().HaveLengthGreaterThan(0);
        user.Username.Should().HaveLengthLessOrEqualTo(50);
    }

    [Fact]
    public void User_ShouldHaveValidNameLength()
    {
        // Arrange
        var user = _fixture.Create<User>();

        // Act & Assert
        user.FirstName.Should().HaveLengthGreaterThan(0);
        user.FirstName.Should().HaveLengthLessOrEqualTo(50);
        user.LastName.Should().HaveLengthGreaterThan(0);
        user.LastName.Should().HaveLengthLessOrEqualTo(50);
    }

    [Fact]
    public void User_ShouldHaveValidEmailLength()
    {
        // Arrange
        var user = _fixture.Create<User>();

        // Act & Assert
        user.Email.Should().HaveLengthGreaterThan(0);
        user.Email.Should().HaveLengthLessOrEqualTo(100);
    }
}


