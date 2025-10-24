using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using EntityFrameworkCore8Samples.Infrastructure.Data;
using EntityFrameworkCore8Samples.Domain.Entities;
using EntityFrameworkCore8Samples.Application.Services;
using EntityFrameworkCore8Samples.Application.Interfaces;

namespace EntityFrameworkCore8Samples.Tests.Fixtures;

public class DatabaseFixture : IDisposable
{
    public ApplicationDbContext Context { get; private set; }
    public IServiceProvider ServiceProvider { get; private set; }
    private readonly ServiceCollection _services;

    public DatabaseFixture()
    {
        _services = new ServiceCollection();
        SetupServices();
        ServiceProvider = _services.BuildServiceProvider();
        Context = ServiceProvider.GetRequiredService<ApplicationDbContext>();
        InitializeDatabase();
    }

    private void SetupServices()
    {
        // Add logging
        _services.AddLogging(builder => builder.AddConsole());

        // Add in-memory database
        _services.AddDbContext<ApplicationDbContext>(options =>
            options.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()));

        // Add repositories
        _services.AddScoped<IRepository<User>, Repository<User>>();
        _services.AddScoped<IRepository<Product>, Repository<Product>>();
        _services.AddScoped<IRepository<Order>, Repository<Order>>();
        _services.AddScoped<IRepository<OrderItem>, Repository<OrderItem>>();
        _services.AddScoped<IRepository<UserProfile>, Repository<UserProfile>>();
        _services.AddScoped<IRepository<UserSession>, Repository<UserSession>>();
        _services.AddScoped<IRepository<ProductReview>, Repository<ProductReview>>();
        _services.AddScoped<IRepository<ProductInventory>, Repository<ProductInventory>>();
        _services.AddScoped<IRepository<OrderStatusHistory>, Repository<OrderStatusHistory>>();

        // Add services
        _services.AddScoped<IDataGeneratorService, DataGeneratorService>();
        _services.AddScoped<IDatabaseManager, DatabaseManager>();
    }

    private void InitializeDatabase()
    {
        Context.Database.EnsureCreated();
    }

    public void Dispose()
    {
        Context?.Dispose();
        ServiceProvider?.Dispose();
    }
}



