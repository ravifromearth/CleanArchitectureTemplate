using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using EntityFrameworkCore8Samples.Tests.Fixtures;
using EntityFrameworkCore8Samples.Application.Services;
using EntityFrameworkCore8Samples.Domain.Entities;

namespace EntityFrameworkCore8Samples.Tests.Integration.Database;

public class DatabaseIntegrationTests : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _fixture;
    private readonly DataGeneratorService _dataGenerator;

    public DatabaseIntegrationTests(DatabaseFixture fixture)
    {
        _fixture = fixture;
        _dataGenerator = _fixture.ServiceProvider.GetRequiredService<DataGeneratorService>();
    }

    [Fact]
    public async Task Database_ShouldBeAccessible()
    {
        // Act
        var isAccessible = await _fixture.Context.Database.CanConnectAsync();

        // Assert
        isAccessible.Should().BeTrue();
    }

    [Fact]
    public async Task Database_ShouldHaveCorrectSchema()
    {
        // Act
        var tables = await _fixture.Context.Database.GetDbConnection().GetSchemaAsync("Tables");

        // Assert
        tables.Should().NotBeNull();
        tables.Rows.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Users_ShouldBeCreatableAndRetrievable()
    {
        // Arrange
        var users = _dataGenerator.GenerateUsers(10);
        var repository = _fixture.ServiceProvider.GetRequiredService<IRepository<User>>();

        // Act
        await repository.AddRangeAsync(users);
        await _fixture.Context.SaveChangesAsync();
        var savedUsers = await repository.GetAllAsync();

        // Assert
        savedUsers.Should().HaveCount(10);
        savedUsers.Should().AllSatisfy(user =>
        {
            user.Id.Should().NotBeEmpty();
            user.Username.Should().NotBeNullOrEmpty();
            user.Email.Should().NotBeNullOrEmpty();
        });
    }

    [Fact]
    public async Task Products_ShouldBeCreatableAndRetrievable()
    {
        // Arrange
        var products = _dataGenerator.GenerateProducts(15);
        var repository = _fixture.ServiceProvider.GetRequiredService<IRepository<Product>>();

        // Act
        await repository.AddRangeAsync(products);
        await _fixture.Context.SaveChangesAsync();
        var savedProducts = await repository.GetAllAsync();

        // Assert
        savedProducts.Should().HaveCount(15);
        savedProducts.Should().AllSatisfy(product =>
        {
            product.Id.Should().NotBeEmpty();
            product.Name.Should().NotBeNullOrEmpty();
            product.Price.Should().BePositive();
        });
    }

    [Fact]
    public async Task Orders_ShouldBeCreatableAndRetrievable()
    {
        // Arrange
        var users = _dataGenerator.GenerateUsers(5);
        var userRepository = _fixture.ServiceProvider.GetRequiredService<IRepository<User>>();
        await userRepository.AddRangeAsync(users);
        await _fixture.Context.SaveChangesAsync();

        var orders = _dataGenerator.GenerateOrders(20, users.Select(u => u.Id).ToList());
        var orderRepository = _fixture.ServiceProvider.GetRequiredService<IRepository<Order>>();

        // Act
        await orderRepository.AddRangeAsync(orders);
        await _fixture.Context.SaveChangesAsync();
        var savedOrders = await orderRepository.GetAllAsync();

        // Assert
        savedOrders.Should().HaveCount(20);
        savedOrders.Should().AllSatisfy(order =>
        {
            order.Id.Should().NotBeEmpty();
            order.UserId.Should().BeOneOf(users.Select(u => u.Id));
            order.Total.Should().BePositive();
        });
    }

    [Fact]
    public async Task OrderItems_ShouldBeCreatableAndRetrievable()
    {
        // Arrange
        var users = _dataGenerator.GenerateUsers(3);
        var products = _dataGenerator.GenerateProducts(10);
        var orders = _dataGenerator.GenerateOrders(5, users.Select(u => u.Id).ToList());

        var userRepository = _fixture.ServiceProvider.GetRequiredService<IRepository<User>>();
        var productRepository = _fixture.ServiceProvider.GetRequiredService<IRepository<Product>>();
        var orderRepository = _fixture.ServiceProvider.GetRequiredService<IRepository<Order>>();

        await userRepository.AddRangeAsync(users);
        await productRepository.AddRangeAsync(products);
        await orderRepository.AddRangeAsync(orders);
        await _fixture.Context.SaveChangesAsync();

        var orderItems = _dataGenerator.GenerateOrderItems(50, orders.Select(o => o.Id).ToList(), products.Select(p => p.Id).ToList());
        var orderItemRepository = _fixture.ServiceProvider.GetRequiredService<IRepository<OrderItem>>();

        // Act
        await orderItemRepository.AddRangeAsync(orderItems);
        await _fixture.Context.SaveChangesAsync();
        var savedOrderItems = await orderItemRepository.GetAllAsync();

        // Assert
        savedOrderItems.Should().HaveCount(50);
        savedOrderItems.Should().AllSatisfy(item =>
        {
            item.Id.Should().NotBeEmpty();
            item.Quantity.Should().BePositive();
            item.UnitPrice.Should().BePositive();
            item.TotalPrice.Should().BePositive();
        });
    }

    [Fact]
    public async Task UserProfiles_ShouldBeCreatableAndRetrievable()
    {
        // Arrange
        var users = _dataGenerator.GenerateUsers(8);
        var userRepository = _fixture.ServiceProvider.GetRequiredService<IRepository<User>>();
        await userRepository.AddRangeAsync(users);
        await _fixture.Context.SaveChangesAsync();

        var profiles = _dataGenerator.GenerateUserProfiles(8, users.Select(u => u.Id).ToList());
        var profileRepository = _fixture.ServiceProvider.GetRequiredService<IRepository<UserProfile>>();

        // Act
        await profileRepository.AddRangeAsync(profiles);
        await _fixture.Context.SaveChangesAsync();
        var savedProfiles = await profileRepository.GetAllAsync();

        // Assert
        savedProfiles.Should().HaveCount(8);
        savedProfiles.Should().AllSatisfy(profile =>
        {
            profile.Id.Should().NotBeEmpty();
            profile.UserId.Should().BeOneOf(users.Select(u => u.Id));
            profile.Bio.Should().NotBeNullOrEmpty();
        });
    }

    [Fact]
    public async Task UserSessions_ShouldBeCreatableAndRetrievable()
    {
        // Arrange
        var users = _dataGenerator.GenerateUsers(6);
        var userRepository = _fixture.ServiceProvider.GetRequiredService<IRepository<User>>();
        await userRepository.AddRangeAsync(users);
        await _fixture.Context.SaveChangesAsync();

        var sessions = _dataGenerator.GenerateUserSessions(30, users.Select(u => u.Id).ToList());
        var sessionRepository = _fixture.ServiceProvider.GetRequiredService<IRepository<UserSession>>();

        // Act
        await sessionRepository.AddRangeAsync(sessions);
        await _fixture.Context.SaveChangesAsync();
        var savedSessions = await sessionRepository.GetAllAsync();

        // Assert
        savedSessions.Should().HaveCount(30);
        savedSessions.Should().AllSatisfy(session =>
        {
            session.Id.Should().NotBeEmpty();
            session.UserId.Should().BeOneOf(users.Select(u => u.Id));
            session.SessionToken.Should().NotBeNullOrEmpty();
        });
    }

    [Fact]
    public async Task ProductReviews_ShouldBeCreatableAndRetrievable()
    {
        // Arrange
        var users = _dataGenerator.GenerateUsers(10);
        var products = _dataGenerator.GenerateProducts(15);

        var userRepository = _fixture.ServiceProvider.GetRequiredService<IRepository<User>>();
        var productRepository = _fixture.ServiceProvider.GetRequiredService<IRepository<Product>>();
        await userRepository.AddRangeAsync(users);
        await productRepository.AddRangeAsync(products);
        await _fixture.Context.SaveChangesAsync();

        var reviews = _dataGenerator.GenerateProductReviews(40, users.Select(u => u.Id).ToList(), products.Select(p => p.Id).ToList());
        var reviewRepository = _fixture.ServiceProvider.GetRequiredService<IRepository<ProductReview>>();

        // Act
        await reviewRepository.AddRangeAsync(reviews);
        await _fixture.Context.SaveChangesAsync();
        var savedReviews = await reviewRepository.GetAllAsync();

        // Assert
        savedReviews.Should().HaveCount(40);
        savedReviews.Should().AllSatisfy(review =>
        {
            review.Id.Should().NotBeEmpty();
            review.Rating.Should().BeInRange(1, 5);
            review.Title.Should().NotBeNullOrEmpty();
        });
    }

    [Fact]
    public async Task ProductInventories_ShouldBeCreatableAndRetrievable()
    {
        // Arrange
        var products = _dataGenerator.GenerateProducts(12);
        var productRepository = _fixture.ServiceProvider.GetRequiredService<IRepository<Product>>();
        await productRepository.AddRangeAsync(products);
        await _fixture.Context.SaveChangesAsync();

        var inventories = _dataGenerator.GenerateProductInventories(12, products.Select(p => p.Id).ToList());
        var inventoryRepository = _fixture.ServiceProvider.GetRequiredService<IRepository<ProductInventory>>();

        // Act
        await inventoryRepository.AddRangeAsync(inventories);
        await _fixture.Context.SaveChangesAsync();
        var savedInventories = await inventoryRepository.GetAllAsync();

        // Assert
        savedInventories.Should().HaveCount(12);
        savedInventories.Should().AllSatisfy(inventory =>
        {
            inventory.Id.Should().NotBeEmpty();
            inventory.Quantity.Should().BeGreaterOrEqualTo(0);
            inventory.MaxStock.Should().BeGreaterOrEqualTo(inventory.Quantity);
        });
    }

    [Fact]
    public async Task OrderStatusHistories_ShouldBeCreatableAndRetrievable()
    {
        // Arrange
        var users = _dataGenerator.GenerateUsers(5);
        var orders = _dataGenerator.GenerateOrders(10, users.Select(u => u.Id).ToList());

        var userRepository = _fixture.ServiceProvider.GetRequiredService<IRepository<User>>();
        var orderRepository = _fixture.ServiceProvider.GetRequiredService<IRepository<Order>>();
        await userRepository.AddRangeAsync(users);
        await orderRepository.AddRangeAsync(orders);
        await _fixture.Context.SaveChangesAsync();

        var histories = _dataGenerator.GenerateOrderStatusHistories(50, orders.Select(o => o.Id).ToList());
        var historyRepository = _fixture.ServiceProvider.GetRequiredService<IRepository<OrderStatusHistory>>();

        // Act
        await historyRepository.AddRangeAsync(histories);
        await _fixture.Context.SaveChangesAsync();
        var savedHistories = await historyRepository.GetAllAsync();

        // Assert
        savedHistories.Should().HaveCount(50);
        savedHistories.Should().AllSatisfy(history =>
        {
            history.Id.Should().NotBeEmpty();
            history.Comment.Should().NotBeNullOrEmpty();
        });
    }

    [Fact]
    public async Task Database_ShouldSupportComplexQueries()
    {
        // Arrange
        var users = _dataGenerator.GenerateUsers(20);
        var products = _dataGenerator.GenerateProducts(30);
        var orders = _dataGenerator.GenerateOrders(50, users.Select(u => u.Id).ToList());

        var userRepository = _fixture.ServiceProvider.GetRequiredService<IRepository<User>>();
        var productRepository = _fixture.ServiceProvider.GetRequiredService<IRepository<Product>>();
        var orderRepository = _fixture.ServiceProvider.GetRequiredService<IRepository<Order>>();

        await userRepository.AddRangeAsync(users);
        await productRepository.AddRangeAsync(products);
        await orderRepository.AddRangeAsync(orders);
        await _fixture.Context.SaveChangesAsync();

        // Act
        var activeUsers = await userRepository.FindAsync(u => u.IsActive);
        var expensiveProducts = await productRepository.FindAsync(p => p.Price > 100);
        var recentOrders = await orderRepository.FindAsync(o => o.CreatedAt > DateTime.UtcNow.AddDays(-30));

        // Assert
        activeUsers.Should().NotBeEmpty();
        expensiveProducts.Should().NotBeEmpty();
        recentOrders.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Database_ShouldSupportAggregateQueries()
    {
        // Arrange
        var users = _dataGenerator.GenerateUsers(15);
        var products = _dataGenerator.GenerateProducts(25);
        var orders = _dataGenerator.GenerateOrders(40, users.Select(u => u.Id).ToList());

        var userRepository = _fixture.ServiceProvider.GetRequiredService<IRepository<User>>();
        var productRepository = _fixture.ServiceProvider.GetRequiredService<IRepository<Product>>();
        var orderRepository = _fixture.ServiceProvider.GetRequiredService<IRepository<Order>>();

        await userRepository.AddRangeAsync(users);
        await productRepository.AddRangeAsync(products);
        await orderRepository.AddRangeAsync(orders);
        await _fixture.Context.SaveChangesAsync();

        // Act
        var userCount = await userRepository.CountAsync();
        var productCount = await productRepository.CountAsync();
        var orderCount = await orderRepository.CountAsync();

        // Assert
        userCount.Should().Be(15);
        productCount.Should().Be(25);
        orderCount.Should().Be(40);
    }
}



