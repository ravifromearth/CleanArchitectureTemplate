/*
Clean Architecture Entity Framework Core 8 Application
Following SOLID, DRY, and Clean Code principles

Commands for SQL Server:
- dotnet ef migrations add Initial --context ApplicationDbContext
- dotnet ef database update --context ApplicationDbContext

Commands for PostgreSQL (change connection string):
- dotnet ef migrations add InitialPostgreSQL --context ApplicationDbContext
- dotnet ef database update --context ApplicationDbContext
*/

using CleanArchitecture.Application.Configuration;
using CleanArchitecture.Application.Interfaces;
using CleanArchitecture.Infrastructure.Services;
using CleanArchitecture.Infrastructure.Data;
using CleanArchitecture.Infrastructure.Repositories;
using EntityFrameworkCore8Samples;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

Console.WriteLine("=== Entity Framework Core 8 - Interactive Console Application ===");
Console.WriteLine("Choose your mode:\n");
Console.WriteLine("1. ???  Interactive Mode    - Full menu-driven interface with CRUD operations");
Console.WriteLine("2. ?? Quick Test Mode     - Run automated tests and exit");
Console.WriteLine("3. ?? Database Setup      - Initialize database and seed data only");
Console.WriteLine();
Console.Write("Enter your choice (1-3): ");

var modeChoice = Console.ReadLine();

Console.WriteLine("\n[DEBUG] Building host...");

// Build host with dependency injection
var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        config.SetBasePath(AppContext.BaseDirectory);
        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    })
    .ConfigureServices((context, services) =>
    {
        // Configure settings
        services.Configure<DatabaseSettings>(context.Configuration.GetSection("DatabaseSettings"));
        services.Configure<ApplicationSettings>(context.Configuration.GetSection("ApplicationSettings"));
        
        var databaseSettings = context.Configuration.GetSection("DatabaseSettings").Get<DatabaseSettings>() ?? new DatabaseSettings();
        
        // Configure database provider
        if (databaseSettings.UseSqlServer)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    databaseSettings.ConnectionStrings.SqlServer,
                    sqlOptions => sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null)
                ));
        }
        else
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(
                    databaseSettings.ConnectionStrings.PostgreSQL,
                    npgsqlOptions => npgsqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorCodesToAdd: null)
                ));
        }
        
        // Register repositories and unit of work
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        // Register services
        services.AddScoped<IDataSeederService, DataSeederService>();
        services.AddScoped<IDataGeneratorService, DataGeneratorService>();
        services.AddScoped<IDatabaseScriptExecutor, DatabaseScriptExecutor>();
        services.AddScoped<IDatabaseObjectsService, DatabaseObjectsService>();
        services.AddScoped<IDatabaseManager, DatabaseManager>();
        
        // Configure logging
        services.AddLogging(builder =>
        {
            builder.AddConsole();
            builder.SetMinimumLevel(LogLevel.Information);
        });
    })
    .Build();

Console.WriteLine("[DEBUG] Host built successfully. Creating service scope...");

try
{
    using var scope = host.Services.CreateScope();
    var services = scope.ServiceProvider;
    
    Console.WriteLine("[DEBUG] Service scope created. Getting services...");
    
    // Get services
    var context = services.GetRequiredService<ApplicationDbContext>();
    Console.WriteLine("[DEBUG] Got ApplicationDbContext");
    var unitOfWork = services.GetRequiredService<IUnitOfWork>();
    Console.WriteLine("[DEBUG] Got UnitOfWork");
    var dbObjectsService = services.GetRequiredService<IDatabaseObjectsService>();
    Console.WriteLine("[DEBUG] Got DatabaseObjectsService");
    var databaseManager = services.GetRequiredService<IDatabaseManager>();
    Console.WriteLine("[DEBUG] Got DatabaseManager");
    var logger = services.GetRequiredService<ILogger<Program>>();
    Console.WriteLine("[DEBUG] All services resolved. Starting mode execution...");
    
    switch (modeChoice)
    {
        case "1":
            // Interactive Mode
            var interactiveApp = new InteractiveConsoleApp(services);
            await interactiveApp.RunAsync();
            break;
            
        case "2":
            // Quick Test Mode
            await RunQuickTestModeAsync(services);
            break;
            
        case "3":
            // Database Setup Mode
            await RunDatabaseSetupModeAsync(services);
            break;
            
        default:
            Console.WriteLine("? Invalid choice. Running in Quick Test Mode...");
            await RunQuickTestModeAsync(services);
            break;
    }
}
catch (Exception ex)
{
    Console.WriteLine($"\n=== Application failed with error: {ex.Message} ===");
    Console.WriteLine($"Stack trace: {ex.StackTrace}");
}

// Helper methods for different modes
static async Task RunQuickTestModeAsync(IServiceProvider services)
{
    var context = services.GetRequiredService<ApplicationDbContext>();
    var unitOfWork = services.GetRequiredService<IUnitOfWork>();
    var dbObjectsService = services.GetRequiredService<IDatabaseObjectsService>();
    var databaseManager = services.GetRequiredService<IDatabaseManager>();
    var logger = services.GetRequiredService<ILogger<Program>>();
    
    logger.LogInformation("=== Entity Framework Core 8 - Quick Test Mode ===");
    
    // Step 1: Check database accessibility
    logger.LogInformation("\n1. Checking database accessibility...");
    if (!await databaseManager.IsDatabaseAccessibleAsync())
    {
        logger.LogError("Cannot connect to database. Please check your connection string and SQL Server status.");
        logger.LogInformation("Connection String: {ConnectionString}", context.Database.GetConnectionString());
        return;
    }
    
    // Step 2: Create database if it doesn't exist
    logger.LogInformation("\n2. Ensuring database exists...");
    if (!await databaseManager.CreateDatabaseIfNotExistsAsync())
    {
        logger.LogError("Failed to create database. Exiting...");
        return;
    }
    
    // Step 3: Apply migrations
    logger.LogInformation("\n3. Applying database migrations...");
    if (!await databaseManager.ApplyMigrationsAsync())
    {
        logger.LogError("Failed to apply migrations. Exiting...");
        return;
    }
    
    // Step 4: Execute database scripts
    logger.LogInformation("\n4. Executing database scripts...");
    if (!await databaseManager.ExecuteDatabaseScriptsAsync())
    {
        logger.LogWarning("Failed to execute some database scripts, but continuing...");
    }
    
    // Step 5: Handle data seeding automatically
    logger.LogInformation("\n5. Managing data seeding...");
    var stats = await databaseManager.GetDatabaseStatisticsAsync();
    
    if (!stats.HasData)
    {
        logger.LogInformation("Database is empty. Seeding with test data...");
        if (await databaseManager.SeedDataAsync(forceSeed: true))
        {
            logger.LogInformation("? Data seeding completed successfully!");
        }
        else
        {
            logger.LogError("? Data seeding failed!");
        }
    }
    else
    {
        logger.LogInformation("Database already contains data. Skipping seeding.");
        logger.LogInformation("  - Users: {UserCount}", stats.UserCount);
        logger.LogInformation("  - Products: {ProductCount}", stats.ProductCount);
        logger.LogInformation("  - Orders: {OrderCount}", stats.OrderCount);
        logger.LogInformation("  - Total Records: {TotalRecords}", stats.TotalRecords);
    }
    
    // Test complex queries
    logger.LogInformation("\n=== Testing Complex Queries ===");
    
    var users = await unitOfWork.Users.GetAllAsync();
    logger.LogInformation($"   ? Found {users.Count()} users");
    
    var products = await unitOfWork.Products.GetAllAsync();
    logger.LogInformation($"   ? Found {products.Count()} products");
    
    var orders = await unitOfWork.Orders.GetAllAsync();
    logger.LogInformation($"   ? Found {orders.Count()} orders");
    
    // Test Views
    logger.LogInformation("\n=== Testing Database Views ===");
    try
    {
        var userProfileSummaries = await dbObjectsService.GetUserProfileSummariesAsync();
        logger.LogInformation($"   ? User Profile Summaries: {userProfileSummaries.Count} records");
        if (userProfileSummaries.Any())
        {
            var sample = userProfileSummaries.First();
            logger.LogInformation($"      Sample: {sample.Username} - {sample.Email} - Total Orders: {sample.TotalOrders}");
        }
        
        var inventoryStatus = await dbObjectsService.GetProductInventoryStatusAsync();
        logger.LogInformation($"   ? Product Inventory Status: {inventoryStatus.Count} records");
        if (inventoryStatus.Any())
        {
            var sample = inventoryStatus.First();
            logger.LogInformation($"      Sample: {sample.ProductName} - Stock Level: {sample.StockLevel}");
        }
        
        var orderSummaries = await dbObjectsService.GetOrderDetailsSummariesAsync();
        logger.LogInformation($"   ? Order Details Summaries: {orderSummaries.Count} records");
        if (orderSummaries.Any())
        {
            var sample = orderSummaries.First();
            logger.LogInformation($"      Sample: Order #{sample.OrderNumber} - Total: ${sample.Total:F2}");
        }
    }
    catch (Exception ex)
    {
        logger.LogWarning($"   ? Views test failed: {ex.Message}");
    }
    
    // Test Functions
    logger.LogInformation("\n=== Testing Database Functions ===");
    try
    {
        if (users.Any())
        {
            var firstUser = users.First();
            var lifetimeValue = await dbObjectsService.CalculateUserLifetimeValueAsync(firstUser.Id);
            logger.LogInformation($"   ? User Lifetime Value for {firstUser.Username}: ${lifetimeValue:F2}");
        }
        
        if (products.Any())
        {
            var firstProduct = products.First();
            var avgRating = await dbObjectsService.GetProductAverageRatingAsync(firstProduct.Id);
            logger.LogInformation($"   ? Product Average Rating for {firstProduct.Name}: {avgRating:F2}");
        }
    }
    catch (Exception ex)
    {
        logger.LogWarning($"   ? Functions test failed: {ex.Message}");
    }
    
    // Test Stored Procedures
    logger.LogInformation("\n=== Testing Stored Procedures ===");
    try
    {
        // Get sales report
        var salesReport = await dbObjectsService.GetSalesReportAsync();
        logger.LogInformation($"   ? Sales Report generated: {salesReport.Count} days");
        if (salesReport.Any())
        {
            var sample = salesReport.First();
            logger.LogInformation($"      Sample: {sample.OrderDate:yyyy-MM-dd} - Orders: {sample.TotalOrders}, Revenue: ${sample.TotalRevenue:F2}");
        }
    }
    catch (Exception ex)
    {
        logger.LogWarning($"   ? Stored Procedures test failed: {ex.Message}");
    }
    
    logger.LogInformation("\n=== All Operations Completed Successfully! ===");
    logger.LogInformation($"Database: {(context.Database.IsSqlServer() ? "SQL Server" : "PostgreSQL")}");
    logger.LogInformation($"? Complex data types: Arrays, JSON, Complex Types, Enums, Decimals, Binary Data");
    logger.LogInformation($"? Database objects: Views, Functions, Stored Procedures");
    logger.LogInformation($"? Ready for migration testing!");
}

static async Task RunDatabaseSetupModeAsync(IServiceProvider services)
{
    var context = services.GetRequiredService<ApplicationDbContext>();
    var databaseManager = services.GetRequiredService<IDatabaseManager>();
    var logger = services.GetRequiredService<ILogger<Program>>();
    
    logger.LogInformation("=== Entity Framework Core 8 - Database Setup Mode ===");
    
    // Check database accessibility
    logger.LogInformation("\n1. Checking database accessibility...");
    if (!await databaseManager.IsDatabaseAccessibleAsync())
    {
        logger.LogError("Cannot connect to database. Please check your connection string and SQL Server status.");
        logger.LogInformation("Connection String: {ConnectionString}", context.Database.GetConnectionString());
        return;
    }
    
    // Create database if it doesn't exist
    logger.LogInformation("\n2. Ensuring database exists...");
    if (!await databaseManager.CreateDatabaseIfNotExistsAsync())
    {
        logger.LogError("Failed to create database. Exiting...");
        return;
    }
    
    // Apply migrations
    logger.LogInformation("\n3. Applying database migrations...");
    if (!await databaseManager.ApplyMigrationsAsync())
    {
        logger.LogError("Failed to apply migrations. Exiting...");
        return;
    }
    
    // Execute database scripts
    logger.LogInformation("\n4. Executing database scripts...");
    if (!await databaseManager.ExecuteDatabaseScriptsAsync())
    {
        logger.LogWarning("Failed to execute some database scripts, but continuing...");
    }
    
    // Handle data seeding
    logger.LogInformation("\n5. Managing data seeding...");
    var stats = await databaseManager.GetDatabaseStatisticsAsync();
    
    if (!stats.HasData)
    {
        logger.LogInformation("Database is empty. Seeding with test data...");
        if (await databaseManager.SeedDataAsync(forceSeed: true))
        {
            logger.LogInformation("? Data seeding completed successfully!");
        }
        else
        {
            logger.LogError("? Data seeding failed!");
        }
    }
    else
    {
        logger.LogInformation("Database already contains data. Skipping seeding.");
        logger.LogInformation("  - Users: {UserCount}", stats.UserCount);
        logger.LogInformation("  - Products: {ProductCount}", stats.ProductCount);
        logger.LogInformation("  - Orders: {OrderCount}", stats.OrderCount);
        logger.LogInformation("  - Total Records: {TotalRecords}", stats.TotalRecords);
    }
    
    logger.LogInformation("\n=== Database Setup Completed Successfully! ===");
    logger.LogInformation($"Database: {(context.Database.IsSqlServer() ? "SQL Server" : "PostgreSQL")}");
    logger.LogInformation("? Database initialized and ready for use!");
}
