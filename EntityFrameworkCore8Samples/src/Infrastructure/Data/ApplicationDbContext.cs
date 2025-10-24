using EntityFrameworkCore8Samples.Application.Interfaces;
using EntityFrameworkCore8Samples.Domain.Entities;
using EntityFrameworkCore8Samples.Domain.Entities.ViewModels;
using EntityFrameworkCore8Samples.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace EntityFrameworkCore8Samples.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // DbSets for Tables
    public DbSet<User> Users => Set<User>();
    public DbSet<UserProfile> UserProfiles => Set<UserProfile>();
    public DbSet<UserSession> UserSessions => Set<UserSession>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductReview> ProductReviews => Set<ProductReview>();
    public DbSet<ProductInventory> ProductInventories => Set<ProductInventory>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<OrderStatusHistory> OrderStatusHistories => Set<OrderStatusHistory>();

    // DbSets for Views (Read-only)
    public DbSet<UserProfileSummaryView> UserProfileSummaries => Set<UserProfileSummaryView>();
    public DbSet<ProductInventoryStatusView> ProductInventoryStatuses => Set<ProductInventoryStatusView>();
    public DbSet<OrderDetailsSummaryView> OrderDetailsSummaries => Set<OrderDetailsSummaryView>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Determine if using PostgreSQL
        bool isPostgreSQL = Database.IsNpgsql();
        
        // Apply configurations using factory pattern
        EntityConfigurationFactory.ConfigureEntities(modelBuilder, isPostgreSQL);

        // Configure Views as read-only entities
        modelBuilder.Entity<UserProfileSummaryView>(entity =>
        {
            entity.HasNoKey();
            entity.ToView("vw_UserProfileSummary");
            
            // Configure decimal precision for SQL Server
            entity.Property(e => e.Balance)
                .HasPrecision(18, 2);
            entity.Property(e => e.TotalSpent)
                .HasPrecision(18, 2);
            // TotalOrders is now INT, no precision needed
        });

        modelBuilder.Entity<ProductInventoryStatusView>(entity =>
        {
            entity.HasNoKey();
            entity.ToView("vw_ProductInventoryStatus");
            
            // Configure decimal precision
            entity.Property(e => e.Price)
                .HasPrecision(18, 2);
            entity.Property(e => e.SalePrice)
                .HasPrecision(18, 2);
        });

        modelBuilder.Entity<OrderDetailsSummaryView>(entity =>
        {
            entity.HasNoKey();
            entity.ToView("vw_OrderDetailsSummary");
            
            // Configure decimal precision
            entity.Property(e => e.SubTotal)
                .HasPrecision(18, 2);
            entity.Property(e => e.TaxAmount)
                .HasPrecision(18, 2);
            entity.Property(e => e.ShippingCost)
                .HasPrecision(18, 2);
            entity.Property(e => e.Total)
                .HasPrecision(18, 2);
        });
        
        // Configure SalesReportDto for stored procedure results
        modelBuilder.Entity<SalesReportDto>(entity =>
        {
            entity.HasNoKey();
            
            // Configure decimal precision
            entity.Property(e => e.TotalRevenue)
                .HasPrecision(18, 2);
            entity.Property(e => e.AverageOrderValue)
                .HasPrecision(18, 2);
            entity.Property(e => e.TotalTax)
                .HasPrecision(18, 2);
            entity.Property(e => e.TotalShipping)
                .HasPrecision(18, 2);
        });
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        
        // Log SQL commands to console
        optionsBuilder.LogTo(Console.WriteLine, [RelationalEventId.CommandExecuted]);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Automatically set CreatedAt and UpdatedAt timestamps
        var entries = ChangeTracker.Entries<BaseEntity>();
        
        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.UtcNow;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }
        }
        
        return base.SaveChangesAsync(cancellationToken);
    }
}

