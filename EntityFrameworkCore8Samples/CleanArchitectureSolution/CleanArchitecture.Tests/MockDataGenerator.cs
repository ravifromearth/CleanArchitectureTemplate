using AutoFixture;
using Bogus;
using EntityFrameworkCore8Samples.Domain.Entities;
using EntityFrameworkCore8Samples.Domain.Enums;

namespace EntityFrameworkCore8Samples.Tests.Mocks;

public static class MockDataGenerator
{
    private static readonly Faker _faker = new();
    private static readonly Fixture _fixture = new();

    public static List<User> GenerateUsers(int count)
    {
        var userFaker = new Faker<User>()
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

        return userFaker.Generate(count);
    }

    public static List<Product> GenerateProducts(int count)
    {
        var productFaker = new Faker<Product>()
            .RuleFor(p => p.Id, f => f.Random.Guid())
            .RuleFor(p => p.Name, f => f.Commerce.ProductName())
            .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
            .RuleFor(p => p.Sku, f => f.Commerce.Ean13())
            .RuleFor(p => p.Price, f => f.Random.Decimal(10, 1000))
            .RuleFor(p => p.SalePrice, f => f.Random.Bool(0.3f) ? f.Random.Decimal(5, 800) : null)
            .RuleFor(p => p.Category, f => f.Commerce.Categories(1)[0])
            .RuleFor(p => p.Brand, f => f.Commerce.Department())
            .RuleFor(p => p.Weight, f => f.Random.Decimal(0.1m, 50))
            .RuleFor(p => p.Dimensions, f => $"{f.Random.Decimal(10, 100)}x{f.Random.Decimal(10, 100)}x{f.Random.Decimal(5, 50)}")
            .RuleFor(p => p.Color, f => f.Commerce.Color())
            .RuleFor(p => p.Material, f => f.Commerce.ProductMaterial())
            .RuleFor(p => p.Status, f => f.PickRandom<ProductStatus>())
            .RuleFor(p => p.Type, f => f.PickRandom<ProductType>())
            .RuleFor(p => p.CreatedAt, f => f.Date.Past(2))
            .RuleFor(p => p.UpdatedAt, f => f.Date.Recent())
            .RuleFor(p => p.IsActive, f => f.Random.Bool(0.9f));

        return productFaker.Generate(count);
    }

    public static List<Order> GenerateOrders(int count, List<Guid> userIds)
    {
        if (!userIds.Any()) return new List<Order>();

        var orderFaker = new Faker<Order>()
            .RuleFor(o => o.Id, f => f.Random.Guid())
            .RuleFor(o => o.UserId, f => f.PickRandom(userIds))
            .RuleFor(o => o.OrderNumber, f => f.Random.AlphaNumeric(10).ToUpper())
            .RuleFor(o => o.ShippedAt, f => f.Random.Bool(0.7f) ? f.Date.Recent(30) : null)
            .RuleFor(o => o.DeliveredAt, f => f.Random.Bool(0.5f) ? f.Date.Recent(15) : null)
            .RuleFor(o => o.SubTotal, f => f.Random.Decimal(50, 2000))
            .RuleFor(o => o.TaxAmount, f => f.Random.Decimal(5, 200))
            .RuleFor(o => o.ShippingCost, f => f.Random.Decimal(0, 50))
            .RuleFor(o => o.Total, f => f.Random.Decimal(55, 2250))
            .RuleFor(o => o.OrderData, f => f.Random.Bool(0.3f) ? f.Random.String(100) : null)
            .RuleFor(o => o.Tags, f => f.Random.Bool(0.4f) ? f.Random.StringArray(3, 5, 10) : null)
            .RuleFor(o => o.Status, f => f.PickRandom<OrderStatus>())
            .RuleFor(o => o.PaymentMethod, f => f.PickRandom<PaymentMethod>())
            .RuleFor(o => o.ShippingAddress, f => f.Address.FullAddress())
            .RuleFor(o => o.BillingAddress, f => f.Address.FullAddress())
            .RuleFor(o => o.ReceiptPdf, f => f.Random.Bool(0.2f) ? f.Random.Bytes(1024) : null)
            .RuleFor(o => o.CreatedAt, f => f.Date.Past(1))
            .RuleFor(o => o.UpdatedAt, f => f.Date.Recent());

        return orderFaker.Generate(count);
    }

    public static List<OrderItem> GenerateOrderItems(int count, List<Guid> orderIds, List<Guid> productIds)
    {
        if (!orderIds.Any() || !productIds.Any()) return new List<OrderItem>();

        var orderItemFaker = new Faker<OrderItem>()
            .RuleFor(oi => oi.Id, f => f.Random.Guid())
            .RuleFor(oi => oi.OrderId, f => f.PickRandom(orderIds))
            .RuleFor(oi => oi.ProductId, f => f.PickRandom(productIds))
            .RuleFor(oi => oi.Quantity, f => f.Random.Int(1, 10))
            .RuleFor(oi => oi.UnitPrice, f => f.Random.Decimal(10, 500))
            .RuleFor(oi => oi.TotalPrice, (f, oi) => oi.Quantity * oi.UnitPrice)
            .RuleFor(oi => oi.CreatedAt, f => f.Date.Past(1));

        return orderItemFaker.Generate(count);
    }

    public static List<UserProfile> GenerateUserProfiles(int count, List<Guid> userIds)
    {
        if (!userIds.Any()) return new List<UserProfile>();

        var profileFaker = new Faker<UserProfile>()
            .RuleFor(up => up.Id, f => f.Random.Guid())
            .RuleFor(up => up.UserId, f => f.PickRandom(userIds))
            .RuleFor(up => up.Bio, f => f.Lorem.Paragraph())
            .RuleFor(up => up.AvatarUrl, f => f.Internet.Avatar())
            .RuleFor(up => up.Balance, f => f.Random.Decimal(0, 10000))
            .RuleFor(up => up.TotalSpent, f => f.Random.Decimal(0, 50000))
            .RuleFor(up => up.CreatedAt, f => f.Date.Past(2))
            .RuleFor(up => up.UpdatedAt, f => f.Date.Recent());

        return profileFaker.Generate(count);
    }

    public static List<UserSession> GenerateUserSessions(int count, List<Guid> userIds)
    {
        if (!userIds.Any()) return new List<UserSession>();

        var sessionFaker = new Faker<UserSession>()
            .RuleFor(us => us.Id, f => f.Random.Guid())
            .RuleFor(us => us.UserId, f => f.PickRandom(userIds))
            .RuleFor(us => us.SessionToken, f => f.Random.AlphaNumeric(64))
            .RuleFor(us => us.IpAddress, f => f.Internet.Ip())
            .RuleFor(us => us.UserAgent, f => f.Internet.UserAgent())
            .RuleFor(us => us.CreatedAt, f => f.Date.Past(1))
            .RuleFor(us => us.ExpiresAt, f => f.Date.Future(1));

        return sessionFaker.Generate(count);
    }

    public static List<ProductReview> GenerateProductReviews(int count, List<Guid> userIds, List<Guid> productIds)
    {
        if (!userIds.Any() || !productIds.Any()) return new List<ProductReview>();

        var reviewFaker = new Faker<ProductReview>()
            .RuleFor(pr => pr.Id, f => f.Random.Guid())
            .RuleFor(pr => pr.UserId, f => f.PickRandom(userIds))
            .RuleFor(pr => pr.ProductId, f => f.PickRandom(productIds))
            .RuleFor(pr => pr.Rating, f => f.Random.Int(1, 5))
            .RuleFor(pr => pr.Title, f => f.Lorem.Sentence())
            .RuleFor(pr => pr.Comment, f => f.Lorem.Paragraph())
            .RuleFor(pr => pr.Status, f => f.PickRandom<ReviewStatus>())
            .RuleFor(pr => pr.Type, f => f.PickRandom<ReviewType>())
            .RuleFor(pr => pr.CreatedAt, f => f.Date.Past(1))
            .RuleFor(pr => pr.UpdatedAt, f => f.Date.Recent());

        return reviewFaker.Generate(count);
    }

    public static List<ProductInventory> GenerateProductInventories(int count, List<Guid> productIds)
    {
        if (!productIds.Any()) return new List<ProductInventory>();

        var inventoryFaker = new Faker<ProductInventory>()
            .RuleFor(pi => pi.Id, f => f.Random.Guid())
            .RuleFor(pi => pi.ProductId, f => f.PickRandom(productIds))
            .RuleFor(pi => pi.Quantity, f => f.Random.Int(0, 1000))
            .RuleFor(pi => pi.ReservedQuantity, f => f.Random.Int(0, 100))
            .RuleFor(pi => pi.ReorderLevel, f => f.Random.Int(10, 100))
            .RuleFor(pi => pi.MaxStock, f => f.Random.Int(500, 2000))
            .RuleFor(pi => pi.Status, f => f.PickRandom<InventoryStatus>())
            .RuleFor(pi => pi.Type, f => f.PickRandom<InventoryType>())
            .RuleFor(pi => pi.CreatedAt, f => f.Date.Past(1))
            .RuleFor(pi => pi.UpdatedAt, f => f.Date.Recent());

        return inventoryFaker.Generate(count);
    }

    public static List<OrderStatusHistory> GenerateOrderStatusHistories(int count, List<Guid> orderIds)
    {
        if (!orderIds.Any()) return new List<OrderStatusHistory>();

        var historyFaker = new Faker<OrderStatusHistory>()
            .RuleFor(osh => osh.Id, f => f.Random.Guid())
            .RuleFor(osh => osh.OrderId, f => f.PickRandom(orderIds))
            .RuleFor(osh => osh.Status, f => f.PickRandom<OrderStatus>())
            .RuleFor(osh => osh.Comment, f => f.Lorem.Sentence())
            .RuleFor(osh => osh.CreatedAt, f => f.Date.Past(1));

        return historyFaker.Generate(count);
    }

    public static T CreateEntity<T>() where T : class
    {
        return _fixture.Create<T>();
    }

    public static List<T> CreateEntities<T>(int count) where T : class
    {
        return _fixture.CreateMany<T>(count).ToList();
    }
}


