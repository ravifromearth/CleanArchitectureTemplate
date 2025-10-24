using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Interfaces;

/// <summary>
/// Unit of Work pattern to manage transactions across repositories
/// </summary>
public interface IUnitOfWork : IDisposable
{
    IRepository<User> Users { get; }
    IRepository<UserProfile> UserProfiles { get; }
    IRepository<UserSession> UserSessions { get; }
    IRepository<Product> Products { get; }
    IRepository<ProductReview> ProductReviews { get; }
    IRepository<ProductInventory> ProductInventories { get; }
    IRepository<Order> Orders { get; }
    IRepository<OrderItem> OrderItems { get; }
    IRepository<OrderStatusHistory> OrderStatusHistories { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}


