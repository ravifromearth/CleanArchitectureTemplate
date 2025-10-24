using EntityFrameworkCore8Samples.Application.Interfaces;
using EntityFrameworkCore8Samples.Domain.Entities;
using EntityFrameworkCore8Samples.Domain.Enums;
using EntityFrameworkCore8Samples.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using Moq;

namespace EntityFrameworkCore8Samples.Application.Services;

/// <summary>
/// Service for generating large amounts of test data using Moq
/// </summary>
public class DataGeneratorService : IDataGeneratorService
{
    private readonly ILogger<DataGeneratorService> _logger;
    private readonly Random _random = new();

    // Sample data arrays for realistic generation
    private readonly string[] _firstNames = { "John", "Jane", "Michael", "Sarah", "David", "Emily", "Robert", "Jessica", "William", "Ashley", "James", "Amanda", "Christopher", "Jennifer", "Daniel", "Lisa", "Matthew", "Nancy", "Anthony", "Karen", "Mark", "Betty", "Donald", "Helen", "Steven", "Sandra", "Paul", "Donna", "Andrew", "Carol", "Joshua", "Ruth", "Kenneth", "Sharon", "Kevin", "Michelle", "Brian", "Laura", "George", "Sarah", "Timothy", "Kimberly", "Ronald", "Deborah", "Jason", "Dorothy", "Edward", "Lisa", "Jeffrey", "Nancy" };
    
    private readonly string[] _lastNames = { "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis", "Rodriguez", "Martinez", "Hernandez", "Lopez", "Gonzalez", "Wilson", "Anderson", "Thomas", "Taylor", "Moore", "Jackson", "Martin", "Lee", "Perez", "Thompson", "White", "Harris", "Sanchez", "Clark", "Ramirez", "Lewis", "Robinson", "Walker", "Young", "Allen", "King", "Wright", "Scott", "Torres", "Nguyen", "Hill", "Flores", "Green", "Adams", "Nelson", "Baker", "Hall", "Rivera", "Campbell", "Mitchell", "Carter", "Roberts" };
    
    private readonly string[] _cities = { "New York", "Los Angeles", "Chicago", "Houston", "Phoenix", "Philadelphia", "San Antonio", "San Diego", "Dallas", "San Jose", "Austin", "Jacksonville", "Fort Worth", "Columbus", "Charlotte", "San Francisco", "Indianapolis", "Seattle", "Denver", "Washington", "Boston", "El Paso", "Nashville", "Detroit", "Oklahoma City", "Portland", "Las Vegas", "Memphis", "Louisville", "Baltimore", "Milwaukee", "Albuquerque", "Tucson", "Fresno", "Sacramento", "Mesa", "Kansas City", "Atlanta", "Long Beach", "Colorado Springs", "Raleigh", "Miami", "Virginia Beach", "Omaha", "Oakland", "Minneapolis", "Tulsa", "Arlington", "Tampa" };
    
    private readonly string[] _states = { "CA", "TX", "FL", "NY", "PA", "IL", "OH", "GA", "NC", "MI", "NJ", "VA", "WA", "AZ", "MA", "TN", "IN", "MO", "MD", "WI", "CO", "MN", "SC", "AL", "LA", "KY", "OR", "OK", "CT", "UT", "IA", "NV", "AR", "MS", "KS", "NM", "NE", "WV", "ID", "HI", "NH", "ME", "RI", "MT", "DE", "SD", "ND", "AK", "VT", "WY" };
    
    private readonly string[] _countries = { "United States", "Canada", "United Kingdom", "Australia", "Germany", "France", "Japan", "Italy", "Spain", "Brazil", "India", "China", "Mexico", "Netherlands", "Sweden", "Norway", "Denmark", "Finland", "Switzerland", "Austria" };
    
    private readonly string[] _productNames = { "Laptop", "Smartphone", "Tablet", "Headphones", "Camera", "Watch", "Speaker", "Monitor", "Keyboard", "Mouse", "Charger", "Cable", "Case", "Stand", "Desk", "Chair", "Lamp", "Book", "Pen", "Notebook", "Bag", "Shoes", "Shirt", "Pants", "Jacket", "Hat", "Gloves", "Sunglasses", "Wallet", "Belt", "Socks", "Underwear", "Dress", "Skirt", "Blouse", "Sweater", "Coat", "Boots", "Sandals", "Sneakers", "Backpack", "Purse", "Jewelry", "Ring", "Necklace", "Bracelet", "Earrings", "Watch", "Belt" };
    
    private readonly string[] _productDescriptions = { "High quality", "Premium", "Professional", "Durable", "Lightweight", "Compact", "Ergonomic", "Modern", "Classic", "Stylish", "Comfortable", "Versatile", "Innovative", "Advanced", "Reliable", "Efficient", "Eco-friendly", "Sustainable", "Affordable", "Luxury" };
    
    private readonly string[] _warehouseCodes = { "WH-001", "WH-002", "WH-003", "WH-004", "WH-005", "WH-006", "WH-007", "WH-008", "WH-009", "WH-010" };
    
    private readonly string[] _reviewTitles = { "Great product!", "Excellent quality", "Highly recommended", "Perfect!", "Amazing!", "Love it!", "Best purchase", "Outstanding", "Fantastic", "Wonderful", "Superb", "Terrific", "Brilliant", "Outstanding", "Exceptional", "Remarkable", "Impressive", "Satisfied", "Happy with purchase", "Would buy again" };
    
    private readonly string[] _reviewComments = { "Works perfectly", "Great value for money", "Fast shipping", "Exactly as described", "High quality", "Very satisfied", "Would recommend", "Excellent service", "Great customer support", "Perfect fit", "Good quality", "Nice design", "Easy to use", "Good price", "Fast delivery", "Well made", "Good product", "Happy customer", "Great experience", "Top quality" };

    public DataGeneratorService(ILogger<DataGeneratorService> logger)
    {
        _logger = logger;
    }

    public async Task<List<User>> GenerateUsersAsync(int count = 1000)
    {
        _logger.LogInformation("Generating {Count} users...", count);
        
        var users = new List<User>();
        var roles = Enum.GetValues<UserRole>();
        var statuses = Enum.GetValues<UserStatus>();

        for (int i = 0; i < count; i++)
        {
            var firstName = _firstNames[_random.Next(_firstNames.Length)];
            var lastName = _lastNames[_random.Next(_lastNames.Length)];
            var username = $"{firstName.ToLower()}.{lastName.ToLower()}{i + 1}";
            var email = $"{username}@example.com";

            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = username,
                Email = email,
                Bio = _random.NextDouble() < 0.7 ? GenerateRandomString(50, 200) : null, // 70% chance of bio
                Status = statuses[_random.Next(statuses.Length)],
                Role = roles[_random.Next(roles.Length)],
                Balance = Math.Round((decimal)(_random.NextDouble() * 10000), 2), // $0 - $10,000
                CreditScore = _random.Next(300, 850),
                CreatedAt = DateTime.UtcNow.AddDays(-_random.Next(1, 365)), // Random date within last year
                LastLoginAt = DateTime.UtcNow.AddDays(-_random.Next(0, 30)), // Random date within last month
                BirthDate = new DateOnly(1990 + _random.Next(0, 30), _random.Next(1, 13), _random.Next(1, 29)), // Random birth date
                PreferredLoginTime = new TimeOnly(_random.Next(6, 23), _random.Next(0, 60)), // Random preferred time
                Tags = GenerateRandomStringArray(3, 8), // 3-8 random tags
                FavoriteNumbers = GenerateRandomIntArray(3, 8), // 3-8 random favorite numbers
                Metadata = GenerateRandomJson(), // Random JSON metadata
                ProfilePicture = GenerateRandomBytes(1024) // 1KB random image data
            };

            users.Add(user);
        }

        _logger.LogInformation("Generated {Count} users successfully", users.Count);
        return users;
    }

    public async Task<List<Product>> GenerateProductsAsync(int count = 1000)
    {
        _logger.LogInformation("Generating {Count} products...", count);
        
        var products = new List<Product>();
        var statuses = Enum.GetValues<ProductStatus>();
        var types = Enum.GetValues<ProductType>();

        for (int i = 0; i < count; i++)
        {
            var productName = _productNames[_random.Next(_productNames.Length)];
            var description = _productDescriptions[_random.Next(_productDescriptions.Length)];
            var sku = $"SKU-{_random.Next(100000, 999999)}-{i + 1}";
            var basePrice = (decimal)(_random.NextDouble() * 1000 + 10); // $10 - $1010
            var salePrice = _random.NextDouble() < 0.3 ? (decimal?)(basePrice * 0.8m) : null; // 30% chance of sale

            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = $"{description} {productName}",
                Description = $"A {description.ToLower()} {productName.ToLower()} perfect for your needs.",
                SKU = sku,
                Barcode = $"BC{_random.Next(100000000, 999999999)}", // Random barcode
                Price = basePrice,
                SalePrice = salePrice,
                Cost = basePrice * (decimal)(_random.NextDouble() * 0.6 + 0.3), // 30-90% of price
                Status = statuses[_random.Next(statuses.Length)],
                Type = types[_random.Next(types.Length)],
                CreatedAt = DateTime.UtcNow.AddDays(-_random.Next(1, 365)),
                UpdatedAt = DateTime.UtcNow.AddDays(-_random.Next(0, 30)),
                Tags = GenerateRandomStringArray(2, 6), // 2-6 random tags
                Categories = GenerateRandomStringArray(1, 3), // 1-3 random categories
                Images = GenerateRandomStringArray(1, 5), // 1-5 image URLs
                Specifications = GenerateRandomJson(), // Random JSON specifications
                Thumbnail = GenerateRandomBytes(2048), // 2KB random thumbnail data
                ManualPdf = GenerateRandomBytes(10240), // 10KB random manual data
                DiscontinuedAt = _random.NextDouble() < 0.1 ? DateTime.UtcNow.AddDays(-_random.Next(1, 30)) : null, // 10% chance discontinued
                Dimensions = new ProductDimensions
                {
                    Length = (decimal)(_random.NextDouble() * 50 + 1), // 1-51 inches
                    Width = (decimal)(_random.NextDouble() * 30 + 1),  // 1-31 inches
                    Height = (decimal)(_random.NextDouble() * 20 + 1), // 1-21 inches
                    Unit = "inches"
                },
                Weight = new ProductWeight
                {
                    Value = (decimal)(_random.NextDouble() * 50 + 0.1), // 0.1-50.1 lbs
                    Unit = "lbs"
                }
            };

            products.Add(product);
        }

        _logger.LogInformation("Generated {Count} products successfully", products.Count);
        return products;
    }

    public async Task<List<Order>> GenerateOrdersAsync(List<User> users, List<Product> products, int count = 1000)
    {
        _logger.LogInformation("Generating {Count} orders...", count);
        
        var orders = new List<Order>();
        var statuses = Enum.GetValues<OrderStatus>();
        var paymentMethods = Enum.GetValues<PaymentMethod>();

        for (int i = 0; i < count; i++)
        {
            var user = users[_random.Next(users.Count)];
            var orderNumber = $"ORD-{DateTime.UtcNow:yyyyMMdd}-{i + 1:D6}";
            var subTotal = (decimal)(_random.NextDouble() * 2000 + 50); // $50 - $2050
            var taxAmount = subTotal * 0.08m; // 8% tax
            var shippingCost = (decimal)(_random.NextDouble() * 50 + 5); // $5 - $55
            var total = subTotal + taxAmount + shippingCost;

            var order = new Order
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                OrderNumber = orderNumber,
                Status = statuses[_random.Next(statuses.Length)],
                PaymentMethod = paymentMethods[_random.Next(paymentMethods.Length)],
                SubTotal = subTotal,
                TaxAmount = taxAmount,
                ShippingCost = shippingCost,
                Total = total,
                CreatedAt = DateTime.UtcNow.AddDays(-_random.Next(1, 180)), // Random date within last 6 months
                UpdatedAt = DateTime.UtcNow.AddDays(-_random.Next(0, 30)),
                ShippedAt = _random.NextDouble() < 0.7 ? DateTime.UtcNow.AddDays(-_random.Next(1, 30)) : null, // 70% chance shipped
                DeliveredAt = _random.NextDouble() < 0.5 ? DateTime.UtcNow.AddDays(-_random.Next(1, 15)) : null, // 50% chance delivered
                OrderData = GenerateRandomJson(), // Random JSON order data
                Tags = GenerateRandomStringArray(1, 3), // 1-3 random tags
                ShippingAddress = new Address
                {
                    Street = $"{_random.Next(1, 9999)} {GenerateRandomString(5, 15)} St",
                    City = _cities[_random.Next(_cities.Length)],
                    State = _states[_random.Next(_states.Length)],
                    PostalCode = _random.Next(10000, 99999).ToString(),
                    Country = _countries[_random.Next(_countries.Length)],
                    Latitude = (double)(_random.NextDouble() * 180 - 90), // -90 to 90
                    Longitude = (double)(_random.NextDouble() * 360 - 180) // -180 to 180
                },
                BillingAddress = new Address
                {
                    Street = $"{_random.Next(1, 9999)} {GenerateRandomString(5, 15)} Ave",
                    City = _cities[_random.Next(_cities.Length)],
                    State = _states[_random.Next(_states.Length)],
                    PostalCode = _random.Next(10000, 99999).ToString(),
                    Country = _countries[_random.Next(_countries.Length)],
                    Latitude = (double)(_random.NextDouble() * 180 - 90),
                    Longitude = (double)(_random.NextDouble() * 360 - 180)
                },
                ReceiptPdf = GenerateRandomBytes(4096)  // 4KB random receipt data
            };

            orders.Add(order);
        }

        _logger.LogInformation("Generated {Count} orders successfully", orders.Count);
        return orders;
    }

    public async Task<List<UserProfile>> GenerateUserProfilesAsync(List<User> users)
    {
        _logger.LogInformation("Generating user profiles for {Count} users...", users.Count);
        
        var profiles = new List<UserProfile>();

        foreach (var user in users)
        {
            // 80% chance of having a profile
            if (_random.NextDouble() < 0.8)
            {
                var firstName = _firstNames[_random.Next(_firstNames.Length)];
                var lastName = _lastNames[_random.Next(_lastNames.Length)];

                var profile = new UserProfile
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    FirstName = firstName,
                    LastName = lastName,
                    PhoneNumber = $"+1-{_random.Next(200, 999)}-{_random.Next(100, 999)}-{_random.Next(1000, 9999)}",
                    Anniversary = _random.NextDouble() < 0.3 ? new DateOnly(2020 + _random.Next(0, 4), _random.Next(1, 13), _random.Next(1, 29)) : null, // 30% chance of anniversary
                    Preferences = GenerateRandomJson(), // Random JSON preferences
                    Skills = GenerateRandomStringArray(2, 8), // 2-8 skills
                    Languages = GenerateRandomStringArray(1, 4), // 1-4 languages
                    Avatar = GenerateRandomBytes(1024), // 1KB random avatar data
                    HomeAddress = new Address
                    {
                        Street = $"{_random.Next(1, 9999)} {GenerateRandomString(5, 15)} St",
                        City = _cities[_random.Next(_cities.Length)],
                        State = _states[_random.Next(_states.Length)],
                        PostalCode = _random.Next(10000, 99999).ToString(),
                        Country = _countries[_random.Next(_countries.Length)],
                        Latitude = (double)(_random.NextDouble() * 180 - 90),
                        Longitude = (double)(_random.NextDouble() * 360 - 180)
                    },
                    WorkAddress = new Address
                    {
                        Street = $"{_random.Next(1, 9999)} {GenerateRandomString(5, 15)} Ave",
                        City = _cities[_random.Next(_cities.Length)],
                        State = _states[_random.Next(_states.Length)],
                        PostalCode = _random.Next(10000, 99999).ToString(),
                        Country = _countries[_random.Next(_countries.Length)],
                        Latitude = (double)(_random.NextDouble() * 180 - 90),
                        Longitude = (double)(_random.NextDouble() * 360 - 180)
                    },
                    CreatedAt = user.CreatedAt.AddDays(_random.Next(0, 7)) // Within a week of user creation
                };

                profiles.Add(profile);
            }
        }

        _logger.LogInformation("Generated {Count} user profiles successfully", profiles.Count);
        return profiles;
    }

    public async Task<List<UserSession>> GenerateUserSessionsAsync(List<User> users)
    {
        _logger.LogInformation("Generating user sessions for {Count} users...", users.Count);
        
        var sessions = new List<UserSession>();

        foreach (var user in users)
        {
            // Generate 1-5 sessions per user
            var sessionCount = _random.Next(1, 6);
            
            for (int i = 0; i < sessionCount; i++)
            {
                var session = new UserSession
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    SessionToken = GenerateRandomString(32, 32), // 32 character token
                    IpAddress = GenerateRandomIpAddress(),
                    UserAgent = GenerateRandomUserAgent(),
                    Type = _random.NextDouble() < 0.7 ? SessionType.Web : SessionType.Mobile, // 70% web, 30% mobile
                    Status = _random.NextDouble() < 0.8 ? SessionStatus.Active : SessionStatus.Expired, // 80% active, 20% expired
                    CreatedAt = DateTime.UtcNow.AddDays(-_random.Next(0, 30)),
                    LastActivityAt = DateTime.UtcNow.AddMinutes(-_random.Next(0, 1440)), // Within last 24 hours
                    ExpiresAt = DateTime.UtcNow.AddDays(_random.Next(1, 30)), // 1-30 days from now
                    SessionData = GenerateRandomJson(), // Random session data
                    Permissions = GenerateRandomStringArray(1, 5), // 1-5 permissions
                    EncryptionKey = GenerateRandomBytes(32) // 32-byte encryption key
                };

                sessions.Add(session);
            }
        }

        _logger.LogInformation("Generated {Count} user sessions successfully", sessions.Count);
        return sessions;
    }

    public async Task<List<ProductReview>> GenerateProductReviewsAsync(List<Product> products, List<User> users)
    {
        _logger.LogInformation("Generating product reviews for {Count} products...", products.Count);
        
        var reviews = new List<ProductReview>();

        foreach (var product in products)
        {
            // Generate 0-10 reviews per product
            var reviewCount = _random.Next(0, 11);
            
            for (int i = 0; i < reviewCount; i++)
            {
                var user = users[_random.Next(users.Count)];
                var title = _reviewTitles[_random.Next(_reviewTitles.Length)];
                var comment = _reviewComments[_random.Next(_reviewComments.Length)];

                var review = new ProductReview
                {
                    Id = Guid.NewGuid(),
                    ProductId = product.Id,
                    UserId = user.Id,
                    Rating = _random.Next(1, 6), // 1-5 stars
                    Title = title,
                    Comment = $"{comment}. {GenerateRandomString(20, 100)}",
                    Status = _random.NextDouble() < 0.9 ? ReviewStatus.Approved : ReviewStatus.Pending, // 90% approved
                    Type = _random.NextDouble() < 0.8 ? ReviewType.Product : ReviewType.Service, // 80% product, 20% service
                    CreatedAt = DateTime.UtcNow.AddDays(-_random.Next(1, 180)),
                    UpdatedAt = DateTime.UtcNow.AddDays(-_random.Next(0, 30)),
                    ReviewData = GenerateRandomJson(), // Random review data
                    Tags = GenerateRandomStringArray(1, 4), // 1-4 tags
                    ReviewImage = GenerateRandomBytes(2048) // 2KB random image data
                };

                reviews.Add(review);
            }
        }

        _logger.LogInformation("Generated {Count} product reviews successfully", reviews.Count);
        return reviews;
    }

    public async Task<List<ProductInventory>> GenerateProductInventoriesAsync(List<Product> products)
    {
        _logger.LogInformation("Generating product inventories for {Count} products...", products.Count);
        
        var inventories = new List<ProductInventory>();

        foreach (var product in products)
        {
            // Generate 1-3 inventory records per product (different warehouses)
            var warehouseCount = _random.Next(1, 4);
            
            for (int i = 0; i < warehouseCount; i++)
            {
                var warehouseCode = _warehouseCodes[_random.Next(_warehouseCodes.Length)];
                var quantity = _random.Next(0, 1000);
                var availableQuantity = (int)(quantity * _random.NextDouble() * 0.9); // 0-90% of total
                var reservedQuantity = quantity - availableQuantity;

                var inventory = new ProductInventory
                {
                    Id = Guid.NewGuid(),
                    ProductId = product.Id,
                    WarehouseCode = warehouseCode,
                    Location = $"Aisle {_random.Next(1, 20)}-{_random.Next(1, 10)}-{_random.Next(1, 5)}", // Random warehouse location
                    Quantity = quantity,
                    AvailableQuantity = availableQuantity,
                    ReservedQuantity = reservedQuantity,
                    UnitCost = (decimal)(_random.NextDouble() * 100 + 10), // $10-$110
                    UnitPrice = (decimal)(_random.NextDouble() * 200 + 20), // $20-$220
                    Status = availableQuantity > 0 ? InventoryStatus.InStock : InventoryStatus.OutOfStock,
                    Type = _random.NextDouble() < 0.8 ? InventoryType.Physical : InventoryType.Digital, // 80% physical, 20% digital
                    LastRestocked = DateTime.UtcNow.AddDays(-_random.Next(0, 90)),
                    LastUpdated = DateTime.UtcNow.AddDays(-_random.Next(0, 7)),
                    ExpiryDate = _random.NextDouble() < 0.3 ? DateTime.UtcNow.AddDays(_random.Next(30, 365)) : null, // 30% chance of expiry
                    InventoryData = GenerateRandomJson(), // Random inventory data
                    BatchNumbers = GenerateRandomStringArray(1, 3), // 1-3 batch numbers
                    SerialNumbers = GenerateRandomStringArray(1, 5), // 1-5 serial numbers
                    BarcodeImage = GenerateRandomBytes(1024) // 1KB barcode image
                };

                inventories.Add(inventory);
            }
        }

        _logger.LogInformation("Generated {Count} product inventories successfully", inventories.Count);
        return inventories;
    }

    public async Task<List<OrderItem>> GenerateOrderItemsAsync(List<Order> orders, List<Product> products)
    {
        _logger.LogInformation("Generating order items for {Count} orders...", orders.Count);
        
        var orderItems = new List<OrderItem>();

        foreach (var order in orders)
        {
            // Generate 1-5 items per order
            var itemCount = _random.Next(1, 6);
            var usedProducts = new HashSet<Guid>();
            
            for (int i = 0; i < itemCount; i++)
            {
                Product product;
                do
                {
                    product = products[_random.Next(products.Count)];
                } while (usedProducts.Contains(product.Id));
                
                usedProducts.Add(product.Id);

                var quantity = _random.Next(1, 6);
                var unitPrice = product.SalePrice ?? product.Price;
                var totalPrice = unitPrice * quantity;

                var orderItem = new OrderItem
                {
                    Id = Guid.NewGuid(),
                    OrderId = order.Id,
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Description = product.Description,
                    Quantity = quantity,
                    UnitPrice = unitPrice,
                    TotalPrice = totalPrice,
                    CreatedAt = order.CreatedAt,
                    AddedAt = order.CreatedAt,
                    ModifiedAt = _random.NextDouble() < 0.3 ? DateTime.UtcNow.AddDays(-_random.Next(0, 30)) : null, // 30% chance modified
                    ProductData = GenerateRandomJson(), // Random product data
                    Attributes = GenerateRandomStringArray(0, 3), // 0-3 attributes
                    Categories = product.Categories, // Use product categories
                    ProductImage = GenerateRandomBytes(1024) // 1KB random image data
                };

                orderItems.Add(orderItem);
            }
        }

        _logger.LogInformation("Generated {Count} order items successfully", orderItems.Count);
        return orderItems;
    }

    public async Task<List<OrderStatusHistory>> GenerateOrderStatusHistoriesAsync(List<Order> orders, List<User> users)
    {
        _logger.LogInformation("Generating order status histories for {Count} orders...", orders.Count);
        
        var statusHistories = new List<OrderStatusHistory>();

        foreach (var order in orders)
        {
            // Generate 1-4 status changes per order
            var statusCount = _random.Next(1, 5);
            var statuses = new[] { "Pending", "Processing", "Shipped", "Delivered", "Cancelled" };
            var currentStatus = order.Status.ToString();
            
            for (int i = 0; i < statusCount; i++)
            {
                var status = statuses[_random.Next(statuses.Length)];
                var changedBy = users[_random.Next(users.Count)];

                var statusHistory = new OrderStatusHistory
                {
                    Id = Guid.NewGuid(),
                    OrderId = order.Id,
                    OldStatus = order.Status, // Use current order status as old status
                    NewStatus = Enum.Parse<OrderStatus>(status), // Parse string to enum
                    ChangedBy = changedBy.Username, // Use username instead of ID
                    ChangedAt = order.CreatedAt.AddDays(_random.Next(0, 30)),
                    Reason = GenerateRandomString(20, 100),
                    Notes = _random.NextDouble() < 0.5 ? GenerateRandomString(20, 100) : null, // 50% chance of notes
                    ChangeData = GenerateRandomJson(), // Random change data
                    Tags = GenerateRandomStringArray(1, 3), // 1-3 tags
                    Document = GenerateRandomBytes(1024) // 1KB random document data
                };

                statusHistories.Add(statusHistory);
            }
        }

        _logger.LogInformation("Generated {Count} order status histories successfully", statusHistories.Count);
        return statusHistories;
    }

    #region Helper Methods

    private string GenerateRandomString(int minLength, int maxLength)
    {
        var length = _random.Next(minLength, maxLength + 1);
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[_random.Next(s.Length)]).ToArray());
    }

    private string[] GenerateRandomStringArray(int minCount, int maxCount)
    {
        var count = _random.Next(minCount, maxCount + 1);
        var array = new string[count];
        
        for (int i = 0; i < count; i++)
        {
            array[i] = GenerateRandomString(5, 15);
        }
        
        return array;
    }

    private int[] GenerateRandomIntArray(int minCount, int maxCount)
    {
        var count = _random.Next(minCount, maxCount + 1);
        var array = new int[count];
        
        for (int i = 0; i < count; i++)
        {
            array[i] = _random.Next(1, 100);
        }
        
        return array;
    }

    private byte[] GenerateRandomBytes(int length)
    {
        var bytes = new byte[length];
        _random.NextBytes(bytes);
        return bytes;
    }

    private string GenerateRandomJson()
    {
        var jsonObject = new
        {
            id = _random.Next(1000, 9999),
            name = GenerateRandomString(5, 15),
            value = _random.NextDouble() * 100,
            active = _random.NextDouble() < 0.5,
            tags = GenerateRandomStringArray(2, 5),
            created = DateTime.UtcNow.AddDays(-_random.Next(1, 365)).ToString("O")
        };
        
        return System.Text.Json.JsonSerializer.Serialize(jsonObject);
    }

    private string GenerateRandomIpAddress()
    {
        return $"{_random.Next(1, 255)}.{_random.Next(0, 255)}.{_random.Next(0, 255)}.{_random.Next(1, 255)}";
    }

    private string GenerateRandomUserAgent()
    {
        var userAgents = new[]
        {
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36",
            "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36",
            "Mozilla/5.0 (iPhone; CPU iPhone OS 14_0 like Mac OS X) AppleWebKit/605.1.15",
            "Mozilla/5.0 (Android 10; Mobile; rv:68.0) Gecko/68.0 Firefox/68.0"
        };
        
        return userAgents[_random.Next(userAgents.Length)];
    }

    #endregion
}
