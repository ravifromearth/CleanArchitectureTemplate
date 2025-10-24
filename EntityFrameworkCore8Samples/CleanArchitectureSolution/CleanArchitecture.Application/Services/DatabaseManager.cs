using EntityFrameworkCore8Samples.Application.Interfaces;
using EntityFrameworkCore8Samples.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EntityFrameworkCore8Samples.Application.Services;

/// <summary>
/// Manages database lifecycle and data seeding scenarios
/// </summary>
public class DatabaseManager : IDatabaseManager
{
    private readonly ApplicationDbContext _context;
    private readonly IDataSeederService _dataSeeder;
    private readonly IDatabaseScriptExecutor _scriptExecutor;
    private readonly ILogger<DatabaseManager> _logger;

    public DatabaseManager(
        ApplicationDbContext context,
        IDataSeederService dataSeeder,
        IDatabaseScriptExecutor scriptExecutor,
        ILogger<DatabaseManager> logger)
    {
        _context = context;
        _dataSeeder = dataSeeder;
        _scriptExecutor = scriptExecutor;
        _logger = logger;
    }

    public async Task<bool> IsDatabaseAccessibleAsync()
    {
        try
        {
            _logger.LogInformation("Checking database accessibility...");
            await _context.Database.CanConnectAsync();
            _logger.LogInformation("✓ Database is accessible");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "✗ Database is not accessible: {Message}", ex.Message);
            return false;
        }
    }

    public async Task<bool> HasExistingDataAsync()
    {
        try
        {
            _logger.LogInformation("Checking for existing data...");
            
            var userCount = await _context.Users.CountAsync();
            var productCount = await _context.Products.CountAsync();
            var orderCount = await _context.Orders.CountAsync();
            
            var hasData = userCount > 0 || productCount > 0 || orderCount > 0;
            
            if (hasData)
            {
                _logger.LogInformation("✓ Database contains existing data:");
                _logger.LogInformation("  - Users: {UserCount}", userCount);
                _logger.LogInformation("  - Products: {ProductCount}", productCount);
                _logger.LogInformation("  - Orders: {OrderCount}", orderCount);
            }
            else
            {
                _logger.LogInformation("✓ Database is empty (no existing data)");
            }
            
            return hasData;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "✗ Error checking for existing data: {Message}", ex.Message);
            return false;
        }
    }

    public async Task<bool> CreateDatabaseIfNotExistsAsync()
    {
        try
        {
            _logger.LogInformation("Ensuring database exists...");
            
            var created = await _context.Database.EnsureCreatedAsync();
            
            if (created)
            {
                _logger.LogInformation("✓ Database created successfully");
            }
            else
            {
                _logger.LogInformation("✓ Database already exists");
            }
            
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "✗ Failed to create database: {Message}", ex.Message);
            return false;
        }
    }

    public async Task<bool> ApplyMigrationsAsync()
    {
        try
        {
            _logger.LogInformation("Applying database migrations...");
            
            var pendingMigrations = await _context.Database.GetPendingMigrationsAsync();
            
            if (pendingMigrations.Any())
            {
                _logger.LogInformation("Found {Count} pending migrations:", pendingMigrations.Count());
                foreach (var migration in pendingMigrations)
                {
                    _logger.LogInformation("  - {Migration}", migration);
                }
                
                await _context.Database.MigrateAsync();
                _logger.LogInformation("✓ Migrations applied successfully");
            }
            else
            {
                _logger.LogInformation("✓ Database is up to date (no pending migrations)");
            }
            
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "✗ Failed to apply migrations: {Message}", ex.Message);
            return false;
        }
    }

    public async Task<bool> ExecuteDatabaseScriptsAsync()
    {
        try
        {
            _logger.LogInformation("Executing database scripts (Views, Functions, Stored Procedures)...");
            
            var scriptsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "Infrastructure", "Data", "Scripts");
            scriptsPath = Path.GetFullPath(scriptsPath);
            
            if (!Directory.Exists(scriptsPath))
            {
                _logger.LogWarning("Scripts directory not found at: {ScriptsPath}", scriptsPath);
                return false;
            }
            
            await _scriptExecutor.ExecuteScriptsAsync(scriptsPath);
            _logger.LogInformation("✓ Database scripts executed successfully");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "✗ Failed to execute database scripts: {Message}", ex.Message);
            return false;
        }
    }

    public async Task<bool> SeedDataAsync(bool forceSeed = false)
    {
        try
        {
            if (!forceSeed)
            {
                var hasData = await HasExistingDataAsync();
                if (hasData)
                {
                    _logger.LogInformation("Database contains existing data. Skipping seeding to preserve data.");
                    return true;
                }
            }
            
            _logger.LogInformation("Starting data seeding...");
            await _dataSeeder.SeedComplexDataAsync();
            _logger.LogInformation("✓ Data seeding completed successfully");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "✗ Failed to seed data: {Message}", ex.Message);
            return false;
        }
    }

    public async Task<DatabaseStatistics> GetDatabaseStatisticsAsync()
    {
        var stats = new DatabaseStatistics();
        
        try
        {
            stats.DatabaseExists = await _context.Database.CanConnectAsync();
            
            if (stats.DatabaseExists)
            {
                stats.UserCount = await _context.Users.CountAsync();
                stats.ProductCount = await _context.Products.CountAsync();
                stats.OrderCount = await _context.Orders.CountAsync();
                stats.TotalRecords = stats.UserCount + stats.ProductCount + stats.OrderCount;
                stats.HasData = stats.TotalRecords > 0;
                
                // Get database name from connection string
                var connectionString = _context.Database.GetConnectionString();
                if (!string.IsNullOrEmpty(connectionString))
                {
                    var parts = connectionString.Split(';');
                    foreach (var part in parts)
                    {
                        if (part.Trim().StartsWith("Initial Catalog=", StringComparison.OrdinalIgnoreCase))
                        {
                            stats.DatabaseName = part.Split('=')[1].Trim();
                        }
                        if (part.Trim().StartsWith("Data Source=", StringComparison.OrdinalIgnoreCase))
                        {
                            stats.ServerName = part.Split('=')[1].Trim();
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting database statistics: {Message}", ex.Message);
        }
        
        return stats;
    }

    public async Task<bool> PromptUserForSeedingAsync()
    {
        try
        {
            var stats = await GetDatabaseStatisticsAsync();
            
            if (!stats.DatabaseExists)
            {
                _logger.LogError("Database does not exist and cannot be accessed!");
                return false;
            }
            
            if (!stats.HasData)
            {
                _logger.LogInformation("Database is empty. Would you like to seed it with test data?");
                _logger.LogInformation("This will create 1,000 records for each entity type.");
                
                if (Environment.UserInteractive)
                {
                    Console.WriteLine("\n=== Database Seeding Options ===");
                    Console.WriteLine("1. Yes - Seed with 1,000 records per entity (Recommended for testing)");
                    Console.WriteLine("2. No - Keep database empty (Tables only)");
                    Console.WriteLine("3. Custom - Seed with custom amount");
                    Console.Write("\nEnter your choice (1-3): ");
                    
                    var choice = Console.ReadLine();
                
                    switch (choice)
                    {
                        case "1":
                            _logger.LogInformation("User chose to seed with 1,000 records per entity");
                            return await SeedDataAsync(forceSeed: true);
                        
                        case "2":
                            _logger.LogInformation("User chose to keep database empty");
                            return true;
                        
                        case "3":
                            Console.Write("Enter number of records per entity (1-1000): ");
                            if (int.TryParse(Console.ReadLine(), out int customCount) && customCount > 0 && customCount <= 1000)
                            {
                                _logger.LogInformation("User chose to seed with {Count} records per entity", customCount);
                                // Note: You would need to modify DataSeederService to accept custom count
                                return await SeedDataAsync(forceSeed: true);
                            }
                            else
                            {
                                _logger.LogWarning("Invalid input. Using default seeding.");
                                return await SeedDataAsync(forceSeed: true);
                            }
                        
                        default:
                            _logger.LogWarning("Invalid choice. Using default seeding.");
                            return await SeedDataAsync(forceSeed: true);
                    }
                }
                else
                {
                    // Non-interactive environment - use default seeding
                    _logger.LogInformation("Non-interactive environment detected. Using default seeding with 1,000 records per entity.");
                    return await SeedDataAsync(forceSeed: true);
                }
            }
            else
            {
                _logger.LogInformation("Database contains existing data:");
                _logger.LogInformation("  - Users: {UserCount}", stats.UserCount);
                _logger.LogInformation("  - Products: {ProductCount}", stats.ProductCount);
                _logger.LogInformation("  - Orders: {OrderCount}", stats.OrderCount);
                _logger.LogInformation("  - Total Records: {TotalRecords}", stats.TotalRecords);
                
                if (Environment.UserInteractive)
                {
                    Console.WriteLine("\n=== Existing Data Detected ===");
                    Console.WriteLine("Database already contains data. What would you like to do?");
                    Console.WriteLine("1. Keep existing data (Skip seeding)");
                    Console.WriteLine("2. Add more test data (Append to existing)");
                    Console.WriteLine("3. Clear and reseed (WARNING: This will delete all existing data!)");
                    Console.Write("\nEnter your choice (1-3): ");
                    
                    var choice = Console.ReadLine();
                
                    switch (choice)
                    {
                        case "1":
                            _logger.LogInformation("User chose to keep existing data");
                            return true;
                        
                        case "2":
                            _logger.LogInformation("User chose to add more test data");
                            return await SeedDataAsync(forceSeed: true);
                        
                        case "3":
                            Console.WriteLine("\n⚠️  WARNING: This will delete ALL existing data!");
                            Console.Write("Are you sure? Type 'YES' to confirm: ");
                            var confirmation = Console.ReadLine();
                            
                            if (confirmation?.ToUpper() == "YES")
                            {
                                _logger.LogInformation("User confirmed clearing and reseeding database");
                                await _context.Database.EnsureDeletedAsync();
                                await _context.Database.EnsureCreatedAsync();
                                await ExecuteDatabaseScriptsAsync();
                                return await SeedDataAsync(forceSeed: true);
                            }
                            else
                            {
                                _logger.LogInformation("User cancelled. Keeping existing data.");
                                return true;
                            }
                        
                        default:
                            _logger.LogWarning("Invalid choice. Keeping existing data.");
                            return true;
                    }
                }
                else
                {
                    // Non-interactive environment - keep existing data
                    _logger.LogInformation("Non-interactive environment detected. Keeping existing data.");
                    return true;
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in user prompt: {Message}", ex.Message);
            return false;
        }
    }
}
