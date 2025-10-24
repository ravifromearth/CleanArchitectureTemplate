using FluentAssertions;
using EntityFrameworkCore8Samples.Tests.Fixtures;
using EntityFrameworkCore8Samples.Application.Services;
using EntityFrameworkCore8Samples.Domain.Entities;

namespace EntityFrameworkCore8Samples.Tests.Integration.Services;

public class DataGeneratorServiceIntegrationTests : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _fixture;
    private readonly DataGeneratorService _service;

    public DataGeneratorServiceIntegrationTests(DatabaseFixture fixture)
    {
        _fixture = fixture;
        _service = _fixture.ServiceProvider.GetRequiredService<DataGeneratorService>();
    }

    [Fact]
    public async Task GenerateAndSaveUsers_ShouldCreateValidUsers()
    {
        // Arrange
        var count = 50;
        var users = _service.GenerateUsers(count);

        // Act
        var repository = _fixture.ServiceProvider.GetRequiredService<IRepository<User>>();
        await repository.AddRangeAsync(users);
        await _fixture.Context.SaveChangesAsync();

        // Assert
        var savedUsers = await repository.GetAllAsync();
        savedUsers.Should().HaveCount(count);
        savedUsers.Should().AllSatisfy(user =>
        {
            user.Id.Should().NotBeEmpty();
            user.Username.Should().NotBeNullOrEmpty();
            user.Email.Should().NotBeNullOrEmpty();
            user.FirstName.Should().NotBeNullOrEmpty();
            user.LastName.Should().NotBeNullOrEmpty();
        });
    }

    [Fact]
    public async Task GenerateAndSaveProducts_ShouldCreateValidProducts()
    {
        // Arrange
        var count = 30;
        var products = _service.GenerateProducts(count);

        // Act
        var repository = _fixture.ServiceProvider.GetRequiredService<IRepository<Product>>();
        await repository.AddRangeAsync(products);
        await _fixture.Context.SaveChangesAsync();

        // Assert
        var savedProducts = await repository.GetAllAsync();
        savedProducts.Should().HaveCount(count);
        savedProducts.Should().AllSatisfy(product =>
        {
            product.Id.Should().NotBeEmpty();
            product.Name.Should().NotBeNullOrEmpty();
            product.Description.Should().NotBeNullOrEmpty();
            product.Sku.Should().NotBeNullOrEmpty();
            product.Price.Should().BePositive();
        });
    }

    [Fact]
    public async Task GenerateAndSaveOrders_ShouldCreateValidOrders()
    {
        // Arrange
        var count = 25;
        var users = _service.GenerateUsers(10);
        var userRepository = _fixture.ServiceProvider.GetRequiredService<IRepository<User>>();
        await userRepository.AddRangeAsync(users);
        await _fixture.Context.SaveChangesAsync();

        var userIds = users.Select(u => u.Id).ToList();
        var orders = _service.GenerateOrders(count, userIds);

        // Act
        var orderRepository = _fixture.ServiceProvider.GetRequiredService<IRepository<Order>>();
        await orderRepository.AddRangeAsync(orders);
        await _fixture.Context.SaveChangesAsync();

        // Assert
        var savedOrders = await orderRepository.GetAllAsync();
        savedOrders.Should().HaveCount(count);
        savedOrders.Should().AllSatisfy(order =>
        {
            order.Id.Should().NotBeEmpty();
            order.UserId.Should().BeOneOf(userIds);
            order.OrderNumber.Should().NotBeNullOrEmpty();
            order.Total.Should().BePositive();
        });
    }

    [Fact]
    public async Task GenerateAndSaveOrderItems_ShouldCreateValidOrderItems()
    {
        // Arrange
        var count = 100;
        var users = _service.GenerateUsers(5);
        var products = _service.GenerateProducts(20);
        var orders = _service.GenerateOrders(10, users.Select(u => u.Id).ToList());

        var userRepository = _fixture.ServiceProvider.GetRequiredService<IRepository<User>>();
        var productRepository = _fixture.ServiceProvider.GetRequiredService<IRepository<Product>>();
        var orderRepository = _fixture.ServiceProvider.GetRequiredService<IRepository<Order>>();

        await userRepository.AddRangeAsync(users);
        await productRepository.AddRangeAsync(products);
        await orderRepository.AddRangeAsync(orders);
        await _fixture.Context.SaveChangesAsync();

        var orderIds = orders.Select(o => o.Id).ToList();
        var productIds = products.Select(p => p.Id).ToList();
        var orderItems = _service.GenerateOrderItems(count, orderIds, productIds);

        // Act
        var orderItemRepository = _fixture.ServiceProvider.GetRequiredService<IRepository<OrderItem>>();
        await orderItemRepository.AddRangeAsync(orderItems);
        await _fixture.Context.SaveChangesAsync();

        // Assert
        var savedOrderItems = await orderItemRepository.GetAllAsync();
        savedOrderItems.Should().HaveCount(count);
        savedOrderItems.Should().AllSatisfy(item =>
        {
            item.Id.Should().NotBeEmpty();
            item.OrderId.Should().BeOneOf(orderIds);
            item.ProductId.Should().BeOneOf(productIds);
            item.Quantity.Should().BePositive();
            item.UnitPrice.Should().BePositive();
            item.TotalPrice.Should().BePositive();
        });
    }

    [Fact]
    public async Task GenerateAndSaveUserProfiles_ShouldCreateValidUserProfiles()
    {
        // Arrange
        var count = 40;
        var users = _service.GenerateUsers(15);
        var userRepository = _fixture.ServiceProvider.GetRequiredService<IRepository<User>>();
        await userRepository.AddRangeAsync(users);
        await _fixture.Context.SaveChangesAsync();

        var userIds = users.Select(u => u.Id).ToList();
        var profiles = _service.GenerateUserProfiles(count, userIds);

        // Act
        var profileRepository = _fixture.ServiceProvider.GetRequiredService<IRepository<UserProfile>>();
        await profileRepository.AddRangeAsync(profiles);
        await _fixture.Context.SaveChangesAsync();

        // Assert
        var savedProfiles = await profileRepository.GetAllAsync();
        savedProfiles.Should().HaveCount(count);
        savedProfiles.Should().AllSatisfy(profile =>
        {
            profile.Id.Should().NotBeEmpty();
            profile.UserId.Should().BeOneOf(userIds);
            profile.Bio.Should().NotBeNullOrEmpty();
            profile.AvatarUrl.Should().NotBeNullOrEmpty();
            profile.Balance.Should().BeGreaterOrEqualTo(0);
        });
    }

    [Fact]
    public async Task GenerateAndSaveUserSessions_ShouldCreateValidUserSessions()
    {
        // Arrange
        var count = 60;
        var users = _service.GenerateUsers(20);
        var userRepository = _fixture.ServiceProvider.GetRequiredService<IRepository<User>>();
        await userRepository.AddRangeAsync(users);
        await _fixture.Context.SaveChangesAsync();

        var userIds = users.Select(u => u.Id).ToList();
        var sessions = _service.GenerateUserSessions(count, userIds);

        // Act
        var sessionRepository = _fixture.ServiceProvider.GetRequiredService<IRepository<UserSession>>();
        await sessionRepository.AddRangeAsync(sessions);
        await _fixture.Context.SaveChangesAsync();

        // Assert
        var savedSessions = await sessionRepository.GetAllAsync();
        savedSessions.Should().HaveCount(count);
        savedSessions.Should().AllSatisfy(session =>
        {
            session.Id.Should().NotBeEmpty();
            session.UserId.Should().BeOneOf(userIds);
            session.SessionToken.Should().NotBeNullOrEmpty();
            session.IpAddress.Should().NotBeNullOrEmpty();
            session.UserAgent.Should().NotBeNullOrEmpty();
            session.ExpiresAt.Should().BeAfter(session.CreatedAt);
        });
    }

    [Fact]
    public async Task GenerateAndSaveProductReviews_ShouldCreateValidProductReviews()
    {
        // Arrange
        var count = 80;
        var users = _service.GenerateUsers(25);
        var products = _service.GenerateProducts(30);

        var userRepository = _fixture.ServiceProvider.GetRequiredService<IRepository<User>>();
        var productRepository = _fixture.ServiceProvider.GetRequiredService<IRepository<Product>>();
        await userRepository.AddRangeAsync(users);
        await productRepository.AddRangeAsync(products);
        await _fixture.Context.SaveChangesAsync();

        var userIds = users.Select(u => u.Id).ToList();
        var productIds = products.Select(p => p.Id).ToList();
        var reviews = _service.GenerateProductReviews(count, userIds, productIds);

        // Act
        var reviewRepository = _fixture.ServiceProvider.GetRequiredService<IRepository<ProductReview>>();
        await reviewRepository.AddRangeAsync(reviews);
        await _fixture.Context.SaveChangesAsync();

        // Assert
        var savedReviews = await reviewRepository.GetAllAsync();
        savedReviews.Should().HaveCount(count);
        savedReviews.Should().AllSatisfy(review =>
        {
            review.Id.Should().NotBeEmpty();
            review.UserId.Should().BeOneOf(userIds);
            review.ProductId.Should().BeOneOf(productIds);
            review.Rating.Should().BeInRange(1, 5);
            review.Title.Should().NotBeNullOrEmpty();
            review.Comment.Should().NotBeNullOrEmpty();
        });
    }

    [Fact]
    public async Task GenerateAndSaveProductInventories_ShouldCreateValidProductInventories()
    {
        // Arrange
        var count = 50;
        var products = _service.GenerateProducts(25);
        var productRepository = _fixture.ServiceProvider.GetRequiredService<IRepository<Product>>();
        await productRepository.AddRangeAsync(products);
        await _fixture.Context.SaveChangesAsync();

        var productIds = products.Select(p => p.Id).ToList();
        var inventories = _service.GenerateProductInventories(count, productIds);

        // Act
        var inventoryRepository = _fixture.ServiceProvider.GetRequiredService<IRepository<ProductInventory>>();
        await inventoryRepository.AddRangeAsync(inventories);
        await _fixture.Context.SaveChangesAsync();

        // Assert
        var savedInventories = await inventoryRepository.GetAllAsync();
        savedInventories.Should().HaveCount(count);
        savedInventories.Should().AllSatisfy(inventory =>
        {
            inventory.Id.Should().NotBeEmpty();
            inventory.ProductId.Should().BeOneOf(productIds);
            inventory.Quantity.Should().BeGreaterOrEqualTo(0);
            inventory.ReservedQuantity.Should().BeGreaterOrEqualTo(0);
            inventory.ReorderLevel.Should().BeGreaterOrEqualTo(0);
            inventory.MaxStock.Should().BeGreaterOrEqualTo(inventory.Quantity);
        });
    }

    [Fact]
    public async Task GenerateAndSaveOrderStatusHistories_ShouldCreateValidOrderStatusHistories()
    {
        // Arrange
        var count = 150;
        var users = _service.GenerateUsers(10);
        var orders = _service.GenerateOrders(20, users.Select(u => u.Id).ToList());

        var userRepository = _fixture.ServiceProvider.GetRequiredService<IRepository<User>>();
        var orderRepository = _fixture.ServiceProvider.GetRequiredService<IRepository<Order>>();
        await userRepository.AddRangeAsync(users);
        await orderRepository.AddRangeAsync(orders);
        await _fixture.Context.SaveChangesAsync();

        var orderIds = orders.Select(o => o.Id).ToList();
        var histories = _service.GenerateOrderStatusHistories(count, orderIds);

        // Act
        var historyRepository = _fixture.ServiceProvider.GetRequiredService<IRepository<OrderStatusHistory>>();
        await historyRepository.AddRangeAsync(histories);
        await _fixture.Context.SaveChangesAsync();

        // Assert
        var savedHistories = await historyRepository.GetAllAsync();
        savedHistories.Should().HaveCount(count);
        savedHistories.Should().AllSatisfy(history =>
        {
            history.Id.Should().NotBeEmpty();
            history.OrderId.Should().BeOneOf(orderIds);
            history.Comment.Should().NotBeNullOrEmpty();
            history.CreatedAt.Should().BeBefore(DateTime.UtcNow);
        });
    }
}


