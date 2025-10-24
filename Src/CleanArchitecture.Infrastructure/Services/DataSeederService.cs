using CleanArchitecture.Application.Interfaces;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Enums;
using CleanArchitecture.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace CleanArchitecture.Infrastructure.Services;

/// <summary>
/// Service for seeding complex test data
/// Follows Single Responsibility Principle
/// </summary>
public class DataSeederService : IDataSeederService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDataGeneratorService _dataGenerator;
    private readonly ILogger<DataSeederService> _logger;

    public DataSeederService(
        IUnitOfWork unitOfWork, 
        IDataGeneratorService dataGenerator,
        ILogger<DataSeederService> logger)
    {
        _unitOfWork = unitOfWork;
        _dataGenerator = dataGenerator;
        _logger = logger;
    }

    public async Task SeedComplexDataAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting complex data seeding with 1,000 records per entity...");
        
        try
        {
            // Generate 1,000 users
            _logger.LogInformation("Generating 1,000 users...");
            var users = await _dataGenerator.GenerateUsersAsync(1000);
            await _unitOfWork.Users.AddRangeAsync(users, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("✓ Created {Count} users", users.Count);

            // Generate 1,000 products
            _logger.LogInformation("Generating 1,000 products...");
            var products = await _dataGenerator.GenerateProductsAsync(1000);
            await _unitOfWork.Products.AddRangeAsync(products, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("✓ Created {Count} products", products.Count);

            // Generate user profiles
            _logger.LogInformation("Generating user profiles...");
            var userProfiles = await _dataGenerator.GenerateUserProfilesAsync(users);
            await _unitOfWork.UserProfiles.AddRangeAsync(userProfiles, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("✓ Created {Count} user profiles", userProfiles.Count);

            // Generate user sessions
            _logger.LogInformation("Generating user sessions...");
            var userSessions = await _dataGenerator.GenerateUserSessionsAsync(users);
            await _unitOfWork.UserSessions.AddRangeAsync(userSessions, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("✓ Created {Count} user sessions", userSessions.Count);

            // Generate product inventories
            _logger.LogInformation("Generating product inventories...");
            var productInventories = await _dataGenerator.GenerateProductInventoriesAsync(products);
            await _unitOfWork.ProductInventories.AddRangeAsync(productInventories, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("✓ Created {Count} product inventories", productInventories.Count);

            // Generate product reviews
            _logger.LogInformation("Generating product reviews...");
            var productReviews = await _dataGenerator.GenerateProductReviewsAsync(products, users);
            await _unitOfWork.ProductReviews.AddRangeAsync(productReviews, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("✓ Created {Count} product reviews", productReviews.Count);

            // Generate 1,000 orders
            _logger.LogInformation("Generating 1,000 orders...");
            var orders = await _dataGenerator.GenerateOrdersAsync(users, products, 1000);
            await _unitOfWork.Orders.AddRangeAsync(orders, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("✓ Created {Count} orders", orders.Count);

            // Generate order items
            _logger.LogInformation("Generating order items...");
            var orderItems = await _dataGenerator.GenerateOrderItemsAsync(orders, products);
            await _unitOfWork.OrderItems.AddRangeAsync(orderItems, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("✓ Created {Count} order items", orderItems.Count);

            // Generate order status histories
            _logger.LogInformation("Generating order status histories...");
            var orderStatusHistories = await _dataGenerator.GenerateOrderStatusHistoriesAsync(orders, users);
            await _unitOfWork.OrderStatusHistories.AddRangeAsync(orderStatusHistories, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("✓ Created {Count} order status histories", orderStatusHistories.Count);

            _logger.LogInformation("Complex data seeding completed successfully!");
            _logger.LogInformation("Total records created:");
            _logger.LogInformation("  - Users: {UserCount}", users.Count);
            _logger.LogInformation("  - Products: {ProductCount}", products.Count);
            _logger.LogInformation("  - User Profiles: {ProfileCount}", userProfiles.Count);
            _logger.LogInformation("  - User Sessions: {SessionCount}", userSessions.Count);
            _logger.LogInformation("  - Product Inventories: {InventoryCount}", productInventories.Count);
            _logger.LogInformation("  - Product Reviews: {ReviewCount}", productReviews.Count);
            _logger.LogInformation("  - Orders: {OrderCount}", orders.Count);
            _logger.LogInformation("  - Order Items: {OrderItemCount}", orderItems.Count);
            _logger.LogInformation("  - Order Status Histories: {StatusHistoryCount}", orderStatusHistories.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during data seeding");
            throw;
        }
    }

    public async Task<(Guid UserId, Guid ProductId)> SeedUserAndProductAsync(CancellationToken cancellationToken = default)
    {
        // Create User
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = "testuser123",
            Email = "test@example.com",
            Bio = "Test user with complex data types",
            BirthDate = new DateOnly(1990, 5, 15),
            PreferredLoginTime = new TimeOnly(9, 30),
            Metadata = JsonSerializer.Serialize(new { Theme = "dark", Notifications = true }),
            Tags = new[] { "premium", "verified" },
            FavoriteNumbers = new[] { 7, 13, 42 },
            CreditScore = 750.123456m,
            Balance = 1250.50m,
            ProfilePicture = System.Text.Encoding.UTF8.GetBytes("fake-image-data"),
            Status = UserStatus.Active,
            Role = UserRole.User
        };

        await _unitOfWork.Users.AddAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        Console.WriteLine($"   ✓ Created user: {user.Username}");

        // Create User Profile
        var profile = new UserProfile
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            FirstName = "John",
            LastName = "Doe",
            PhoneNumber = "+1-555-0123",
            HomeAddress = new Address
            {
                Street = "123 Main St",
                City = "New York",
                State = "NY",
                PostalCode = "10001",
                Country = "USA",
                Latitude = 40.7128,
                Longitude = -74.0060
            },
            Preferences = JsonSerializer.Serialize(new { EmailNotifications = true }),
            Skills = new[] { "C#", "Entity Framework", "PostgreSQL" },
            Languages = new[] { "English", "Spanish" }
        };

        await _unitOfWork.UserProfiles.AddAsync(profile, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        Console.WriteLine($"   ✓ Created user profile");

        // Create Product
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = "Advanced Laptop Pro",
            Description = "High-performance laptop",
            SKU = "LAPTOP-PRO-001",
            Price = 1299.99m,
            SalePrice = 1199.99m,
            Specifications = JsonSerializer.Serialize(new { CPU = "Intel i7", RAM = "32GB" }),
            Tags = new[] { "laptop", "gaming", "professional" },
            Categories = new[] { "Electronics", "Computers" },
            Dimensions = new ProductDimensions { Length = 35.5m, Width = 24.5m, Height = 1.8m },
            Weight = new ProductWeight { Value = 2.1m, Unit = "kg" },
            Status = ProductStatus.Active,
            Type = ProductType.Physical
        };

        await _unitOfWork.Products.AddAsync(product, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        Console.WriteLine($"   ✓ Created product: {product.Name}");

        return (user.Id, product.Id);
    }

    private async Task SeedOrdersAsync(Guid userId, Guid productId, CancellationToken cancellationToken)
    {
        var order = new Order
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            OrderNumber = $"ORD-{DateTime.UtcNow:yyyyMMdd-HHmmss}",
            SubTotal = 1199.99m,
            TaxAmount = 96.00m,
            ShippingCost = 25.00m,
            Total = 1320.99m,
            Status = OrderStatus.Processing,
            PaymentMethod = PaymentMethod.CreditCard,
            ShippingAddress = new Address
            {
                Street = "789 Delivery St",
                City = "Los Angeles",
                State = "CA",
                PostalCode = "90210",
                Country = "USA"
            }
        };

        await _unitOfWork.Orders.AddAsync(order, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        Console.WriteLine($"   ✓ Created order: {order.OrderNumber}");
    }
}

