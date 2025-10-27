using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;

namespace CleanArchitecture.Infrastructure.Data.Configurations;

/// <summary>
/// Factory class to configure all entities - Follows Factory Pattern and DRY
/// </summary>
public static class EntityConfigurationFactory
{
    public static void ConfigureEntities(ModelBuilder modelBuilder, bool isPostgreSQL = false)
    {
        ConfigureUser(modelBuilder, isPostgreSQL);
        ConfigureUserProfile(modelBuilder, isPostgreSQL);
        ConfigureUserSession(modelBuilder, isPostgreSQL);
        ConfigureProduct(modelBuilder, isPostgreSQL);
        ConfigureProductReview(modelBuilder, isPostgreSQL);
        ConfigureProductInventory(modelBuilder, isPostgreSQL);
        ConfigureOrder(modelBuilder, isPostgreSQL);
        ConfigureOrderItem(modelBuilder, isPostgreSQL);
        ConfigureOrderStatusHistory(modelBuilder, isPostgreSQL);
    }
    
    private static void ConfigureUser(ModelBuilder modelBuilder, bool isPostgreSQL)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasDefaultValueSql(GetDefaultGuidSql(isPostgreSQL));
            
            ConfigureArrays(entity.Property(e => e.Tags), isPostgreSQL);
            ConfigureArrays(entity.Property(e => e.FavoriteNumbers), isPostgreSQL);
            ConfigureJson(entity.Property(e => e.Metadata), isPostgreSQL);
            
            entity.Property(e => e.Status).HasConversion<string>();
            entity.Property(e => e.Role).HasConversion<string>();
            entity.Property(e => e.CreditScore).HasColumnType(GetDecimalType(18, 6, isPostgreSQL));
            entity.Property(e => e.Balance).HasColumnType(GetDecimalType(18, 2, isPostgreSQL));
        });
    }
    
    private static void ConfigureUserProfile(ModelBuilder modelBuilder, bool isPostgreSQL)
    {
        modelBuilder.Entity<UserProfile>(entity =>
        {
            entity.ToTable("user_profiles");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasDefaultValueSql(GetDefaultGuidSql(isPostgreSQL));
            
            entity.OwnsOne(e => e.HomeAddress, ConfigureAddress);
            entity.OwnsOne(e => e.WorkAddress, ConfigureAddress);
            
            ConfigureArrays(entity.Property(e => e.Skills), isPostgreSQL);
            ConfigureArrays(entity.Property(e => e.Languages), isPostgreSQL);
            ConfigureJson(entity.Property(e => e.Preferences), isPostgreSQL);
        });
    }
    
    private static void ConfigureUserSession(ModelBuilder modelBuilder, bool isPostgreSQL)
    {
        modelBuilder.Entity<UserSession>(entity =>
        {
            entity.ToTable("user_sessions");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasDefaultValueSql(GetDefaultGuidSql(isPostgreSQL));
            
            ConfigureArrays(entity.Property(e => e.Permissions), isPostgreSQL);
            ConfigureJson(entity.Property(e => e.SessionData), isPostgreSQL);
            
            entity.Property(e => e.Status).HasConversion<string>();
            entity.Property(e => e.Type).HasConversion<string>();
        });
    }
    
    private static void ConfigureProduct(ModelBuilder modelBuilder, bool isPostgreSQL)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("products");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasDefaultValueSql(GetDefaultGuidSql(isPostgreSQL));
            
            entity.OwnsOne(e => e.Dimensions, dimensions =>
            {
                dimensions.Property(d => d.Length).HasColumnType(GetDecimalType(18, 4, isPostgreSQL));
                dimensions.Property(d => d.Width).HasColumnType(GetDecimalType(18, 4, isPostgreSQL));
                dimensions.Property(d => d.Height).HasColumnType(GetDecimalType(18, 4, isPostgreSQL));
                dimensions.Property(d => d.Unit).HasMaxLength(10);
            });
            
            entity.OwnsOne(e => e.Weight, weight =>
            {
                weight.Property(w => w.Value).HasColumnType(GetDecimalType(18, 4, isPostgreSQL));
                weight.Property(w => w.Unit).HasMaxLength(10);
            });
            
            ConfigureArrays(entity.Property(e => e.Tags), isPostgreSQL);
            ConfigureArrays(entity.Property(e => e.Categories), isPostgreSQL);
            ConfigureArrays(entity.Property(e => e.Images), isPostgreSQL);
            ConfigureJson(entity.Property(e => e.Specifications), isPostgreSQL);
            
            entity.Property(e => e.Status).HasConversion<string>();
            entity.Property(e => e.Type).HasConversion<string>();
            entity.Property(e => e.Price).HasColumnType(GetDecimalType(18, 4, isPostgreSQL));
            entity.Property(e => e.SalePrice).HasColumnType(GetDecimalType(18, 4, isPostgreSQL));
            entity.Property(e => e.Cost).HasColumnType(GetDecimalType(18, 4, isPostgreSQL));
        });
    }
    
    private static void ConfigureProductReview(ModelBuilder modelBuilder, bool isPostgreSQL)
    {
        modelBuilder.Entity<ProductReview>(entity =>
        {
            entity.ToTable("product_reviews");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasDefaultValueSql(GetDefaultGuidSql(isPostgreSQL));
            
            ConfigureArrays(entity.Property(e => e.Tags), isPostgreSQL);
            ConfigureJson(entity.Property(e => e.ReviewData), isPostgreSQL);
            
            entity.Property(e => e.Status).HasConversion<string>();
            entity.Property(e => e.Type).HasConversion<string>();
            
            entity.HasOne(e => e.Product)
                .WithMany(e => e.Reviews)
                .HasForeignKey(e => e.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
    
    private static void ConfigureProductInventory(ModelBuilder modelBuilder, bool isPostgreSQL)
    {
        modelBuilder.Entity<ProductInventory>(entity =>
        {
            entity.ToTable("product_inventories");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasDefaultValueSql(GetDefaultGuidSql(isPostgreSQL));
            
            ConfigureArrays(entity.Property(e => e.BatchNumbers), isPostgreSQL);
            ConfigureArrays(entity.Property(e => e.SerialNumbers), isPostgreSQL);
            ConfigureJson(entity.Property(e => e.InventoryData), isPostgreSQL);
            
            entity.Property(e => e.Status).HasConversion<string>();
            entity.Property(e => e.Type).HasConversion<string>();
            entity.Property(e => e.UnitCost).HasColumnType(GetDecimalType(18, 4, isPostgreSQL));
            entity.Property(e => e.UnitPrice).HasColumnType(GetDecimalType(18, 4, isPostgreSQL));
            
            entity.HasOne(e => e.Product)
                .WithMany(e => e.Inventory)
                .HasForeignKey(e => e.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
    
    private static void ConfigureOrder(ModelBuilder modelBuilder, bool isPostgreSQL)
    {
        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("orders");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasDefaultValueSql(GetDefaultGuidSql(isPostgreSQL));
            
            entity.OwnsOne(e => e.ShippingAddress, ConfigureAddress);
            entity.OwnsOne(e => e.BillingAddress, ConfigureAddress);
            
            ConfigureArrays(entity.Property(e => e.Tags), isPostgreSQL);
            ConfigureJson(entity.Property(e => e.OrderData), isPostgreSQL);
            
            entity.Property(e => e.Status).HasConversion<string>();
            entity.Property(e => e.PaymentMethod).HasConversion<string>();
            entity.Property(e => e.SubTotal).HasColumnType(GetDecimalType(18, 2, isPostgreSQL));
            entity.Property(e => e.TaxAmount).HasColumnType(GetDecimalType(18, 2, isPostgreSQL));
            entity.Property(e => e.ShippingCost).HasColumnType(GetDecimalType(18, 2, isPostgreSQL));
            entity.Property(e => e.Total).HasColumnType(GetDecimalType(18, 2, isPostgreSQL));
            
            entity.HasOne(e => e.User)
                .WithMany(e => e.Orders)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
    
    private static void ConfigureOrderItem(ModelBuilder modelBuilder, bool isPostgreSQL)
    {
        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.ToTable("order_items");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasDefaultValueSql(GetDefaultGuidSql(isPostgreSQL));
            
            ConfigureArrays(entity.Property(e => e.Attributes), isPostgreSQL);
            ConfigureArrays(entity.Property(e => e.Categories), isPostgreSQL);
            ConfigureJson(entity.Property(e => e.ProductData), isPostgreSQL);
            
            entity.Property(e => e.UnitPrice).HasColumnType(GetDecimalType(18, 4, isPostgreSQL));
            entity.Property(e => e.TotalPrice).HasColumnType(GetDecimalType(18, 4, isPostgreSQL));
            
            entity.HasOne(e => e.Order)
                .WithMany(e => e.OrderItems)
                .HasForeignKey(e => e.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasOne(e => e.Product)
                .WithMany(e => e.OrderItems)
                .HasForeignKey(e => e.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
    
    private static void ConfigureOrderStatusHistory(ModelBuilder modelBuilder, bool isPostgreSQL)
    {
        modelBuilder.Entity<OrderStatusHistory>(entity =>
        {
            entity.ToTable("order_status_histories");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasDefaultValueSql(GetDefaultGuidSql(isPostgreSQL));
            
            ConfigureArrays(entity.Property(e => e.Tags), isPostgreSQL);
            ConfigureJson(entity.Property(e => e.ChangeData), isPostgreSQL);
            
            entity.Property(e => e.OldStatus).HasConversion<string>();
            entity.Property(e => e.NewStatus).HasConversion<string>();
            
            entity.HasOne(e => e.Order)
                .WithMany(e => e.StatusHistory)
                .HasForeignKey(e => e.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
    
    // Helper methods
    private static void ConfigureAddress(OwnedNavigationBuilder<UserProfile, Address> address)
    {
        address.Property(a => a.Street).HasMaxLength(200);
        address.Property(a => a.City).HasMaxLength(100);
        address.Property(a => a.State).HasMaxLength(100);
        address.Property(a => a.PostalCode).HasMaxLength(20);
        address.Property(a => a.Country).HasMaxLength(100);
    }
    
    private static void ConfigureAddress(OwnedNavigationBuilder<Order, Address> address)
    {
        address.Property(a => a.Street).HasMaxLength(200);
        address.Property(a => a.City).HasMaxLength(100);
        address.Property(a => a.State).HasMaxLength(100);
        address.Property(a => a.PostalCode).HasMaxLength(20);
        address.Property(a => a.Country).HasMaxLength(100);
    }
    
    private static void ConfigureArrays(PropertyBuilder<string[]> property, bool isPostgreSQL)
    {
        // Use JSON serialization for compatibility with existing text columns
        property.HasColumnType("text");
        property.HasConversion(
            new ValueConverter<string[], string>(
                v => v == null || v.Length == 0 ? "[]" : JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => DeserializeStringArray(v)
            )
        );
    }
    
    private static void ConfigureArrays(PropertyBuilder<int[]> property, bool isPostgreSQL)
    {
        // Use JSON serialization for compatibility with existing text columns
        property.HasColumnType("text");
        property.HasConversion(
            new ValueConverter<int[], string>(
                v => v == null || v.Length == 0 ? "[]" : JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => DeserializeIntArray(v)
            )
        );
    }
    
    private static string[] DeserializeStringArray(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value == "null") 
            return Array.Empty<string>();
        try 
        {
            return JsonSerializer.Deserialize<string[]>(value, (JsonSerializerOptions?)null) ?? Array.Empty<string>();
        }
        catch 
        {
            return Array.Empty<string>();
        }
    }
    
    private static int[] DeserializeIntArray(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value == "null") 
            return Array.Empty<int>();
        try 
        {
            return JsonSerializer.Deserialize<int[]>(value, (JsonSerializerOptions?)null) ?? Array.Empty<int>();
        }
        catch 
        {
            return Array.Empty<int>();
        }
    }
    
    private static void ConfigureJson(PropertyBuilder<string?> property, bool isPostgreSQL)
    {
        // Use text for compatibility with existing database schema
        property.HasColumnType("text");
    }
    
    private static string GetDefaultGuidSql(bool isPostgreSQL) => 
        isPostgreSQL ? "gen_random_uuid()" : "(newid())";
    
    private static string GetDecimalType(int precision, int scale, bool isPostgreSQL) =>
        isPostgreSQL ? $"numeric({precision},{scale})" : $"decimal({precision},{scale})";
}


