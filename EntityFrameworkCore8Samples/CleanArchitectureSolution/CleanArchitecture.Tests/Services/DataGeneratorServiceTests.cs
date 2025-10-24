using AutoFixture;
using AutoFixture.Xunit2;
using FluentAssertions;
using Moq;
using EntityFrameworkCore8Samples.Application.Services;
using EntityFrameworkCore8Samples.Domain.Entities;
using EntityFrameworkCore8Samples.Domain.Enums;

namespace EntityFrameworkCore8Samples.Tests.Unit.Services;

public class DataGeneratorServiceTests
{
    private readonly Fixture _fixture;
    private readonly DataGeneratorService _service;

    public DataGeneratorServiceTests()
    {
        _fixture = new Fixture();
        _service = new DataGeneratorService();
    }

    [Fact]
    public void GenerateUsers_ShouldReturnCorrectCount()
    {
        // Arrange
        var count = 100;

        // Act
        var users = _service.GenerateUsers(count);

        // Assert
        users.Should().HaveCount(count);
        users.Should().AllSatisfy(user =>
        {
            user.Id.Should().NotBeEmpty();
            user.Username.Should().NotBeNullOrEmpty();
            user.Email.Should().NotBeNullOrEmpty();
            user.FirstName.Should().NotBeNullOrEmpty();
            user.LastName.Should().NotBeNullOrEmpty();
            user.Status.Should().BeOfType<UserStatus>();
            user.Role.Should().BeOfType<UserRole>();
            user.CreatedAt.Should().BeBefore(DateTime.UtcNow);
            user.UpdatedAt.Should().BeOnOrAfter(user.CreatedAt);
        });
    }

    [Fact]
    public void GenerateProducts_ShouldReturnCorrectCount()
    {
        // Arrange
        var count = 50;

        // Act
        var products = _service.GenerateProducts(count);

        // Assert
        products.Should().HaveCount(count);
        products.Should().AllSatisfy(product =>
        {
            product.Id.Should().NotBeEmpty();
            product.Name.Should().NotBeNullOrEmpty();
            product.Description.Should().NotBeNullOrEmpty();
            product.Sku.Should().NotBeNullOrEmpty();
            product.Price.Should().BePositive();
            product.Category.Should().NotBeNullOrEmpty();
            product.Brand.Should().NotBeNullOrEmpty();
            product.Status.Should().BeOfType<ProductStatus>();
            product.Type.Should().BeOfType<ProductType>();
            product.CreatedAt.Should().BeBefore(DateTime.UtcNow);
            product.UpdatedAt.Should().BeOnOrAfter(product.CreatedAt);
        });
    }

    [Fact]
    public void GenerateOrders_ShouldReturnCorrectCount()
    {
        // Arrange
        var count = 75;
        var userIds = _fixture.CreateMany<Guid>(10).ToList();

        // Act
        var orders = _service.GenerateOrders(count, userIds);

        // Assert
        orders.Should().HaveCount(count);
        orders.Should().AllSatisfy(order =>
        {
            order.Id.Should().NotBeEmpty();
            order.UserId.Should().BeOneOf(userIds);
            order.OrderNumber.Should().NotBeNullOrEmpty();
            order.SubTotal.Should().BePositive();
            order.TaxAmount.Should().BeGreaterOrEqualTo(0);
            order.ShippingCost.Should().BeGreaterOrEqualTo(0);
            order.Total.Should().BePositive();
            order.Status.Should().BeOfType<OrderStatus>();
            order.PaymentMethod.Should().BeOfType<PaymentMethod>();
            order.CreatedAt.Should().BeBefore(DateTime.UtcNow);
            order.UpdatedAt.Should().BeOnOrAfter(order.CreatedAt);
        });
    }

    [Fact]
    public void GenerateOrderItems_ShouldReturnCorrectCount()
    {
        // Arrange
        var count = 200;
        var orderIds = _fixture.CreateMany<Guid>(20).ToList();
        var productIds = _fixture.CreateMany<Guid>(30).ToList();

        // Act
        var orderItems = _service.GenerateOrderItems(count, orderIds, productIds);

        // Assert
        orderItems.Should().HaveCount(count);
        orderItems.Should().AllSatisfy(item =>
        {
            item.Id.Should().NotBeEmpty();
            item.OrderId.Should().BeOneOf(orderIds);
            item.ProductId.Should().BeOneOf(productIds);
            item.Quantity.Should().BePositive();
            item.UnitPrice.Should().BePositive();
            item.TotalPrice.Should().BePositive();
            item.CreatedAt.Should().BeBefore(DateTime.UtcNow);
        });
    }

    [Fact]
    public void GenerateUserProfiles_ShouldReturnCorrectCount()
    {
        // Arrange
        var count = 80;
        var userIds = _fixture.CreateMany<Guid>(15).ToList();

        // Act
        var profiles = _service.GenerateUserProfiles(count, userIds);

        // Assert
        profiles.Should().HaveCount(count);
        profiles.Should().AllSatisfy(profile =>
        {
            profile.Id.Should().NotBeEmpty();
            profile.UserId.Should().BeOneOf(userIds);
            profile.Bio.Should().NotBeNullOrEmpty();
            profile.AvatarUrl.Should().NotBeNullOrEmpty();
            profile.Balance.Should().BeGreaterOrEqualTo(0);
            profile.TotalSpent.Should().BeGreaterOrEqualTo(0);
            profile.CreatedAt.Should().BeBefore(DateTime.UtcNow);
            profile.UpdatedAt.Should().BeOnOrAfter(profile.CreatedAt);
        });
    }

    [Fact]
    public void GenerateUserSessions_ShouldReturnCorrectCount()
    {
        // Arrange
        var count = 120;
        var userIds = _fixture.CreateMany<Guid>(25).ToList();

        // Act
        var sessions = _service.GenerateUserSessions(count, userIds);

        // Assert
        sessions.Should().HaveCount(count);
        sessions.Should().AllSatisfy(session =>
        {
            session.Id.Should().NotBeEmpty();
            session.UserId.Should().BeOneOf(userIds);
            session.SessionToken.Should().NotBeNullOrEmpty();
            session.IpAddress.Should().NotBeNullOrEmpty();
            session.UserAgent.Should().NotBeNullOrEmpty();
            session.CreatedAt.Should().BeBefore(DateTime.UtcNow);
            session.ExpiresAt.Should().BeAfter(session.CreatedAt);
        });
    }

    [Fact]
    public void GenerateProductReviews_ShouldReturnCorrectCount()
    {
        // Arrange
        var count = 150;
        var userIds = _fixture.CreateMany<Guid>(20).ToList();
        var productIds = _fixture.CreateMany<Guid>(40).ToList();

        // Act
        var reviews = _service.GenerateProductReviews(count, userIds, productIds);

        // Assert
        reviews.Should().HaveCount(count);
        reviews.Should().AllSatisfy(review =>
        {
            review.Id.Should().NotBeEmpty();
            review.UserId.Should().BeOneOf(userIds);
            review.ProductId.Should().BeOneOf(productIds);
            review.Rating.Should().BeInRange(1, 5);
            review.Title.Should().NotBeNullOrEmpty();
            review.Comment.Should().NotBeNullOrEmpty();
            review.Status.Should().BeOfType<ReviewStatus>();
            review.Type.Should().BeOfType<ReviewType>();
            review.CreatedAt.Should().BeBefore(DateTime.UtcNow);
            review.UpdatedAt.Should().BeOnOrAfter(review.CreatedAt);
        });
    }

    [Fact]
    public void GenerateProductInventories_ShouldReturnCorrectCount()
    {
        // Arrange
        var count = 100;
        var productIds = _fixture.CreateMany<Guid>(50).ToList();

        // Act
        var inventories = _service.GenerateProductInventories(count, productIds);

        // Assert
        inventories.Should().HaveCount(count);
        inventories.Should().AllSatisfy(inventory =>
        {
            inventory.Id.Should().NotBeEmpty();
            inventory.ProductId.Should().BeOneOf(productIds);
            inventory.Quantity.Should().BeGreaterOrEqualTo(0);
            inventory.ReservedQuantity.Should().BeGreaterOrEqualTo(0);
            inventory.ReorderLevel.Should().BeGreaterOrEqualTo(0);
            inventory.MaxStock.Should().BeGreaterOrEqualTo(inventory.Quantity);
            inventory.Status.Should().BeOfType<InventoryStatus>();
            inventory.Type.Should().BeOfType<InventoryType>();
            inventory.CreatedAt.Should().BeBefore(DateTime.UtcNow);
            inventory.UpdatedAt.Should().BeOnOrAfter(inventory.CreatedAt);
        });
    }

    [Fact]
    public void GenerateOrderStatusHistories_ShouldReturnCorrectCount()
    {
        // Arrange
        var count = 300;
        var orderIds = _fixture.CreateMany<Guid>(50).ToList();

        // Act
        var histories = _service.GenerateOrderStatusHistories(count, orderIds);

        // Assert
        histories.Should().HaveCount(count);
        histories.Should().AllSatisfy(history =>
        {
            history.Id.Should().NotBeEmpty();
            history.OrderId.Should().BeOneOf(orderIds);
            history.Status.Should().BeOfType<OrderStatus>();
            history.Comment.Should().NotBeNullOrEmpty();
            history.CreatedAt.Should().BeBefore(DateTime.UtcNow);
        });
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void GenerateUsers_WithInvalidCount_ShouldReturnEmptyCollection(int count)
    {
        // Act
        var users = _service.GenerateUsers(count);

        // Assert
        users.Should().BeEmpty();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-50)]
    public void GenerateProducts_WithInvalidCount_ShouldReturnEmptyCollection(int count)
    {
        // Act
        var products = _service.GenerateProducts(count);

        // Assert
        products.Should().BeEmpty();
    }

    [Fact]
    public void GenerateOrders_WithEmptyUserIds_ShouldReturnEmptyCollection()
    {
        // Arrange
        var count = 10;
        var userIds = new List<Guid>();

        // Act
        var orders = _service.GenerateOrders(count, userIds);

        // Assert
        orders.Should().BeEmpty();
    }

    [Fact]
    public void GenerateOrderItems_WithEmptyOrderIds_ShouldReturnEmptyCollection()
    {
        // Arrange
        var count = 10;
        var orderIds = new List<Guid>();
        var productIds = _fixture.CreateMany<Guid>(5).ToList();

        // Act
        var orderItems = _service.GenerateOrderItems(count, orderIds, productIds);

        // Assert
        orderItems.Should().BeEmpty();
    }

    [Fact]
    public void GenerateOrderItems_WithEmptyProductIds_ShouldReturnEmptyCollection()
    {
        // Arrange
        var count = 10;
        var orderIds = _fixture.CreateMany<Guid>(5).ToList();
        var productIds = new List<Guid>();

        // Act
        var orderItems = _service.GenerateOrderItems(count, orderIds, productIds);

        // Assert
        orderItems.Should().BeEmpty();
    }
}


