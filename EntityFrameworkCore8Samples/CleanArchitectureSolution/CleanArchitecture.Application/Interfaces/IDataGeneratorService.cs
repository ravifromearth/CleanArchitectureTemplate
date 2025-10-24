using EntityFrameworkCore8Samples.Domain.Entities;

namespace EntityFrameworkCore8Samples.Application.Interfaces;

/// <summary>
/// Service for generating large amounts of test data using Moq
/// </summary>
public interface IDataGeneratorService
{
    /// <summary>
    /// Generates 1,000 users with profiles and sessions
    /// </summary>
    Task<List<User>> GenerateUsersAsync(int count = 1000);
    
    /// <summary>
    /// Generates 1,000 products with inventory and reviews
    /// </summary>
    Task<List<Product>> GenerateProductsAsync(int count = 1000);
    
    /// <summary>
    /// Generates 1,000 orders with items and status history
    /// </summary>
    Task<List<Order>> GenerateOrdersAsync(List<User> users, List<Product> products, int count = 1000);
    
    /// <summary>
    /// Generates user profiles for existing users
    /// </summary>
    Task<List<UserProfile>> GenerateUserProfilesAsync(List<User> users);
    
    /// <summary>
    /// Generates user sessions for existing users
    /// </summary>
    Task<List<UserSession>> GenerateUserSessionsAsync(List<User> users);
    
    /// <summary>
    /// Generates product reviews for existing products and users
    /// </summary>
    Task<List<ProductReview>> GenerateProductReviewsAsync(List<Product> products, List<User> users);
    
    /// <summary>
    /// Generates product inventory for existing products
    /// </summary>
    Task<List<ProductInventory>> GenerateProductInventoriesAsync(List<Product> products);
    
    /// <summary>
    /// Generates order items for existing orders and products
    /// </summary>
    Task<List<OrderItem>> GenerateOrderItemsAsync(List<Order> orders, List<Product> products);
    
    /// <summary>
    /// Generates order status history for existing orders
    /// </summary>
    Task<List<OrderStatusHistory>> GenerateOrderStatusHistoriesAsync(List<Order> orders, List<User> users);
}

