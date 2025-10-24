using EntityFrameworkCore8Samples.Application.Interfaces;
using EntityFrameworkCore8Samples.Domain.Entities;
using EntityFrameworkCore8Samples.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace EntityFrameworkCore8Samples.Infrastructure.Repositories;

/// <summary>
/// Unit of Work implementation to manage transactions
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IDbContextTransaction? _transaction;
    
    // Lazy initialization of repositories
    private IRepository<User>? _users;
    private IRepository<UserProfile>? _userProfiles;
    private IRepository<UserSession>? _userSessions;
    private IRepository<Product>? _products;
    private IRepository<ProductReview>? _productReviews;
    private IRepository<ProductInventory>? _productInventories;
    private IRepository<Order>? _orders;
    private IRepository<OrderItem>? _orderItems;
    private IRepository<OrderStatusHistory>? _orderStatusHistories;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public IRepository<User> Users => _users ??= new Repository<User>(_context);
    public IRepository<UserProfile> UserProfiles => _userProfiles ??= new Repository<UserProfile>(_context);
    public IRepository<UserSession> UserSessions => _userSessions ??= new Repository<UserSession>(_context);
    public IRepository<Product> Products => _products ??= new Repository<Product>(_context);
    public IRepository<ProductReview> ProductReviews => _productReviews ??= new Repository<ProductReview>(_context);
    public IRepository<ProductInventory> ProductInventories => _productInventories ??= new Repository<ProductInventory>(_context);
    public IRepository<Order> Orders => _orders ??= new Repository<Order>(_context);
    public IRepository<OrderItem> OrderItems => _orderItems ??= new Repository<OrderItem>(_context);
    public IRepository<OrderStatusHistory> OrderStatusHistories => _orderStatusHistories ??= new Repository<OrderStatusHistory>(_context);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            if (_transaction != null)
            {
                await _transaction.CommitAsync(cancellationToken);
            }
        }
        catch
        {
            await RollbackTransactionAsync(cancellationToken);
            throw;
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}





