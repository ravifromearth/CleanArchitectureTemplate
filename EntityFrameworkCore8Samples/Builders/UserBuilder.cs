using EntityFrameworkCore8Samples.Domain.Entities;
using EntityFrameworkCore8Samples.Domain.Enums;
using Bogus;

namespace EntityFrameworkCore8Samples.Tests.Builders;

public class UserBuilder
{
    private readonly Faker<User> _faker;
    private User _user;

    public UserBuilder()
    {
        _faker = new Faker<User>()
            .RuleFor(u => u.Id, f => f.Random.Guid())
            .RuleFor(u => u.Username, f => f.Internet.UserName())
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .RuleFor(u => u.FirstName, f => f.Name.FirstName())
            .RuleFor(u => u.LastName, f => f.Name.LastName())
            .RuleFor(u => u.PhoneNumber, f => f.Phone.PhoneNumber())
            .RuleFor(u => u.DateOfBirth, f => f.Date.Past(50, DateTime.Now.AddYears(-18)))
            .RuleFor(u => u.Status, f => f.PickRandom<UserStatus>())
            .RuleFor(u => u.Role, f => f.PickRandom<UserRole>())
            .RuleFor(u => u.CreatedAt, f => f.Date.Past(2))
            .RuleFor(u => u.UpdatedAt, f => f.Date.Recent())
            .RuleFor(u => u.IsActive, f => f.Random.Bool(0.8f));

        _user = _faker.Generate();
    }

    public UserBuilder WithId(Guid id)
    {
        _user.Id = id;
        return this;
    }

    public UserBuilder WithUsername(string username)
    {
        _user.Username = username;
        return this;
    }

    public UserBuilder WithEmail(string email)
    {
        _user.Email = email;
        return this;
    }

    public UserBuilder WithName(string firstName, string lastName)
    {
        _user.FirstName = firstName;
        _user.LastName = lastName;
        return this;
    }

    public UserBuilder WithStatus(UserStatus status)
    {
        _user.Status = status;
        return this;
    }

    public UserBuilder WithRole(UserRole role)
    {
        _user.Role = role;
        return this;
    }

    public UserBuilder WithDateOfBirth(DateTime dateOfBirth)
    {
        _user.DateOfBirth = dateOfBirth;
        return this;
    }

    public UserBuilder AsActive()
    {
        _user.IsActive = true;
        return this;
    }

    public UserBuilder AsInactive()
    {
        _user.IsActive = false;
        return this;
    }

    public UserBuilder WithPhoneNumber(string phoneNumber)
    {
        _user.PhoneNumber = phoneNumber;
        return this;
    }

    public User Build()
    {
        return _user;
    }

    public static UserBuilder Create() => new();
    public static UserBuilder CreateRandom() => new();
}



