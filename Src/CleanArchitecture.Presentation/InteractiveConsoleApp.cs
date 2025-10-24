using CleanArchitecture.Application.Configuration;
using CleanArchitecture.Application.Interfaces;
using CleanArchitecture.Infrastructure.Services;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Enums;
using CleanArchitecture.Infrastructure.Data;
using CleanArchitecture.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EntityFrameworkCore8Samples;

/// <summary>
/// Interactive Console Application for Entity Framework Core 8 Testing
/// Provides CRUD operations, database management, and migration testing features
/// </summary>
public class InteractiveConsoleApp
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<InteractiveConsoleApp> _logger;
    private readonly ApplicationDbContext _context;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDatabaseManager _databaseManager;
    private readonly IDatabaseObjectsService _dbObjectsService;

    public InteractiveConsoleApp(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _logger = serviceProvider.GetRequiredService<ILogger<InteractiveConsoleApp>>();
        _context = serviceProvider.GetRequiredService<ApplicationDbContext>();
        _unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();
        _databaseManager = serviceProvider.GetRequiredService<IDatabaseManager>();
        _dbObjectsService = serviceProvider.GetRequiredService<IDatabaseObjectsService>();
    }

    public async Task RunAsync()
    {
        Console.Clear();
        Console.WriteLine("╔══════════════════════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║                    Entity Framework Core 8 - Interactive Console             ║");
        Console.WriteLine("║                    SQL Server ↔ PostgreSQL Migration Testing                 ║");
        Console.WriteLine("╚══════════════════════════════════════════════════════════════════════════════╝");
        Console.WriteLine();

        // Initialize database
        await InitializeDatabaseAsync();

        // Main menu loop
        while (true)
        {
            try
            {
                await ShowMainMenuAsync();
                var choice = GetUserChoice();
                
                switch (choice)
                {
                    case "1":
                        await ShowCrudMenuAsync();
                        break;
                    case "2":
                        await ShowDatabaseManagementMenuAsync();
                        break;
                    case "3":
                        await ShowQueryMenuAsync();
                        break;
                    case "4":
                        await ShowDatabaseObjectsMenuAsync();
                        break;
                    case "5":
                        await ShowMigrationTestingMenuAsync();
                        break;
                    case "6":
                        await ShowStatisticsMenuAsync();
                        break;
                    case "7":
                        await ShowSettingsMenuAsync();
                        break;
                    case "0":
                        Console.WriteLine("\n👋 Goodbye! Thank you for testing Entity Framework Core 8!");
                        return;
                    default:
                        Console.WriteLine("\n❌ Invalid choice. Please try again.");
                        break;
                }

                if (choice != "0")
                {
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in main menu: {Message}", ex.Message);
                Console.WriteLine($"\n❌ Error: {ex.Message}");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }
    }

    private async Task InitializeDatabaseAsync()
    {
        Console.WriteLine("🔧 Initializing database...");
        
        try
        {
            // Check database accessibility
            if (!await _databaseManager.IsDatabaseAccessibleAsync())
            {
                Console.WriteLine("❌ Cannot connect to database. Please check your connection string.");
                return;
            }

            // Create database if needed
            await _databaseManager.CreateDatabaseIfNotExistsAsync();

            // Apply migrations
            await _databaseManager.ApplyMigrationsAsync();

            // Execute scripts
            await _databaseManager.ExecuteDatabaseScriptsAsync();

            // Check if we need to seed data
            var stats = await _databaseManager.GetDatabaseStatisticsAsync();
            if (!stats.HasData)
            {
                Console.WriteLine("📊 Database is empty. Would you like to seed with test data? (y/n): ");
                var seedChoice = Console.ReadLine();
                if (seedChoice?.ToLower() == "y" || seedChoice?.ToLower() == "yes")
                {
                    await _databaseManager.SeedDataAsync(forceSeed: true);
                    Console.WriteLine("✅ Database seeded successfully!");
                }
            }
            else
            {
                Console.WriteLine($"✅ Database ready! Found {stats.TotalRecords} existing records.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error initializing database: {Message}", ex.Message);
            Console.WriteLine($"❌ Error initializing database: {ex.Message}");
        }
    }

    private async Task ShowMainMenuAsync()
    {
        Console.WriteLine("╔══════════════════════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║                                MAIN MENU                                    ║");
        Console.WriteLine("╠══════════════════════════════════════════════════════════════════════════════╣");
        Console.WriteLine("║ 1. 📝 CRUD Operations        - Create, Read, Update, Delete entities        ║");
        Console.WriteLine("║ 2. 🗄️  Database Management    - Manage database, scripts, migrations        ║");
        Console.WriteLine("║ 3. 🔍 Query Operations       - Complex queries and data analysis            ║");
        Console.WriteLine("║ 4. 📊 Database Objects       - Views, Functions, Stored Procedures          ║");
        Console.WriteLine("║ 5. 🔄 Migration Testing      - Test SQL Server ↔ PostgreSQL scenarios      ║");
        Console.WriteLine("║ 6. 📈 Statistics & Reports   - Database statistics and performance          ║");
        Console.WriteLine("║ 7. ⚙️  Settings              - Configuration and connection settings         ║");
        Console.WriteLine("║ 0. 🚪 Exit                   - Close the application                        ║");
        Console.WriteLine("╚══════════════════════════════════════════════════════════════════════════════╝");
        Console.WriteLine();
    }

    private async Task ShowCrudMenuAsync()
    {
        while (true)
        {
            Console.WriteLine("╔══════════════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                            CRUD OPERATIONS                                 ║");
            Console.WriteLine("╠══════════════════════════════════════════════════════════════════════════════╣");
            Console.WriteLine("║ 1. 👤 Users                - Manage user accounts                          ║");
            Console.WriteLine("║ 2. 📦 Products             - Manage product catalog                        ║");
            Console.WriteLine("║ 3. 🛒 Orders               - Manage customer orders                        ║");
            Console.WriteLine("║ 4. 📝 Reviews              - Manage product reviews                        ║");
            Console.WriteLine("║ 5. 📊 Inventory            - Manage product inventory                      ║");
            Console.WriteLine("║ 6. 🔄 Bulk Operations      - Bulk create, update, delete                  ║");
            Console.WriteLine("║ 0. ⬅️  Back to Main Menu    - Return to main menu                          ║");
            Console.WriteLine(" ╚ ══════════════════════════════════════════════════════════════════════════════╝");
            Console.WriteLine();

            var choice = GetUserChoice();
            
            switch (choice)
            {
                case "1":
                    await ManageUsersAsync();
                    break;
                case "2":
                    await ManageProductsAsync();
                    break;
                case "3":
                    await ManageOrdersAsync();
                    break;
                case "4":
                    await ManageReviewsAsync();
                    break;
                case "5":
                    await ManageInventoryAsync();
                    break;
                case "6":
                    await BulkOperationsAsync();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("❌ Invalid choice. Please try again.");
                    break;
            }
        }
    }

    private async Task ManageUsersAsync()
    {
        while (true)
        {
            Console.WriteLine("\n╔══════════════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                              USER MANAGEMENT                                ║");
            Console.WriteLine("╠══════════════════════════════════════════════════════════════════════════════╣");
            Console.WriteLine("║ 1. 📋 List Users           - View all users with pagination                ║");
            Console.WriteLine("║ 2. 🔍 Search Users         - Search users by criteria                      ║");
            Console.WriteLine("║ 3. ➕ Create User          - Add new user account                          ║");
            Console.WriteLine("║ 4. ✏️  Update User         - Modify existing user                           ║");
            Console.WriteLine("║ 5. 🗑️  Delete User         - Remove user account                           ║");
            Console.WriteLine("║ 6. 📊 User Statistics      - View user-related statistics                  ║");
            Console.WriteLine("║ 0. ⬅️  Back                - Return to CRUD menu                           ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════════════════════════╝");
            Console.WriteLine();

            var choice = GetUserChoice();
            
            switch (choice)
            {
                case "1":
                    await ListUsersAsync();
                    break;
                case "2":
                    await SearchUsersAsync();
                    break;
                case "3":
                    await CreateUserAsync();
                    break;
                case "4":
                    await UpdateUserAsync();
                    break;
                case "5":
                    await DeleteUserAsync();
                    break;
                case "6":
                    await ShowUserStatisticsAsync();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("❌ Invalid choice. Please try again.");
                    break;
            }
        }
    }

    private async Task ListUsersAsync()
    {
        try
        {
            Console.WriteLine("\n📋 Listing Users...");
            Console.WriteLine("Enter page size (default 10): ");
            var pageSizeInput = Console.ReadLine();
            int pageSize = int.TryParse(pageSizeInput, out int size) ? size : 10;

            Console.WriteLine("Enter page number (default 1): ");
            var pageInput = Console.ReadLine();
            int page = int.TryParse(pageInput, out int p) ? p : 1;

            var users = await _unitOfWork.Users.GetAllAsync();
            var totalCount = users.Count();
            var pagedUsers = users.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            Console.WriteLine($"\n📊 Showing {pagedUsers.Count} of {totalCount} users (Page {page}):");
            Console.WriteLine("┌─────────────────────────────────────────────────────────────────────────────┐");
            Console.WriteLine("│ Username                    │ Email                           │ Status    │");
            Console.WriteLine("├─────────────────────────────────────────────────────────────────────────────┤");

            foreach (var user in pagedUsers)
            {
                Console.WriteLine($"│ {user.Username.PadRight(28)} │ {user.Email.PadRight(29)} │ {user.Status.ToString().PadRight(9)} │");
            }
            Console.WriteLine("└─────────────────────────────────────────────────────────────────────────────┘");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error listing users: {ex.Message}");
        }
    }

    private async Task SearchUsersAsync()
    {
        try
        {
            Console.WriteLine("\n🔍 Search Users");
            Console.WriteLine("Enter search term (username, email, or status): ");
            var searchTerm = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                Console.WriteLine("❌ Search term cannot be empty.");
                return;
            }

            var users = await _unitOfWork.Users.GetAllAsync();
            var filteredUsers = users.Where(u => 
                u.Username.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                u.Email.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                u.Status.ToString().Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
            ).ToList();

            Console.WriteLine($"\n📊 Found {filteredUsers.Count} users matching '{searchTerm}':");
            Console.WriteLine("┌─────────────────────────────────────────────────────────────────────────────┐");
            Console.WriteLine("│ Username                    │ Email                           │ Status    │");
            Console.WriteLine("├─────────────────────────────────────────────────────────────────────────────┤");

            foreach (var user in filteredUsers.Take(20)) // Limit to 20 results
            {
                Console.WriteLine($"│ {user.Username.PadRight(28)} │ {user.Email.PadRight(29)} │ {user.Status.ToString().PadRight(9)} │");
            }
            Console.WriteLine("└─────────────────────────────────────────────────────────────────────────────┘");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error searching users: {ex.Message}");
        }
    }

    private async Task CreateUserAsync()
    {
        try
        {
            Console.WriteLine("\n➕ Create New User");
            
            Console.Write("Username: ");
            var username = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(username))
            {
                Console.WriteLine("❌ Username is required.");
                return;
            }

            Console.Write("Email: ");
            var email = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(email))
            {
                Console.WriteLine("❌ Email is required.");
                return;
            }

            Console.Write("Status (Active/Inactive/Suspended): ");
            var statusInput = Console.ReadLine();
            if (!Enum.TryParse<UserStatus>(statusInput, true, out var status))
            {
                status = UserStatus.Active;
            }

            Console.Write("Role (User/Admin/SuperAdmin/Moderator): ");
            var roleInput = Console.ReadLine();
            if (!Enum.TryParse<UserRole>(roleInput, true, out var role))
            {
                role = UserRole.User;
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = username,
                Email = email,
                Status = status,
                Role = role,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            Console.WriteLine($"✅ User '{username}' created successfully with ID: {user.Id}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error creating user: {ex.Message}");
        }
    }

    private async Task UpdateUserAsync()
    {
        try
        {
            Console.WriteLine("\n✏️ Update User");
            Console.Write("Enter user ID or username: ");
            var identifier = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(identifier))
            {
                Console.WriteLine("❌ User identifier is required.");
                return;
            }

            var users = await _unitOfWork.Users.GetAllAsync();
            var user = users.FirstOrDefault(u => 
                u.Id.ToString() == identifier || 
                u.Username.Equals(identifier, StringComparison.OrdinalIgnoreCase));

            if (user == null)
            {
                Console.WriteLine("❌ User not found.");
                return;
            }

            Console.WriteLine($"\nCurrent user details:");
            Console.WriteLine($"Username: {user.Username}");
            Console.WriteLine($"Email: {user.Email}");
            Console.WriteLine($"Status: {user.Status}");
            Console.WriteLine($"Role: {user.Role}");

            Console.WriteLine("\nEnter new values (press Enter to keep current):");
            
            Console.Write($"New username [{user.Username}]: ");
            var newUsername = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newUsername))
                user.Username = newUsername;

            Console.Write($"New email [{user.Email}]: ");
            var newEmail = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newEmail))
                user.Email = newEmail;

            Console.Write($"New status [{user.Status}] (Active/Inactive/Suspended): ");
            var newStatusInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newStatusInput) && Enum.TryParse<UserStatus>(newStatusInput, true, out var newStatus))
                user.Status = newStatus;

            Console.Write($"New role [{user.Role}] (User/Admin/SuperAdmin/Moderator): ");
            var newRoleInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newRoleInput) && Enum.TryParse<UserRole>(newRoleInput, true, out var newRole))
                user.Role = newRole;

            user.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            Console.WriteLine($"✅ User '{user.Username}' updated successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error updating user: {ex.Message}");
        }
    }

    private async Task DeleteUserAsync()
    {
        try
        {
            Console.WriteLine("\n🗑️ Delete User");
            Console.Write("Enter user ID or username: ");
            var identifier = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(identifier))
            {
                Console.WriteLine("❌ User identifier is required.");
                return;
            }

            var users = await _unitOfWork.Users.GetAllAsync();
            var user = users.FirstOrDefault(u => 
                u.Id.ToString() == identifier || 
                u.Username.Equals(identifier, StringComparison.OrdinalIgnoreCase));

            if (user == null)
            {
                Console.WriteLine("❌ User not found.");
                return;
            }

            Console.WriteLine($"\n⚠️  WARNING: You are about to delete user '{user.Username}' (ID: {user.Id})");
            Console.WriteLine("This action cannot be undone!");
            Console.Write("Type 'DELETE' to confirm: ");
            var confirmation = Console.ReadLine();

            if (confirmation?.ToUpper() == "DELETE")
            {
                await _unitOfWork.Users.DeleteAsync(user);
                await _unitOfWork.SaveChangesAsync();
                Console.WriteLine($"✅ User '{user.Username}' deleted successfully!");
            }
            else
            {
                Console.WriteLine("❌ Deletion cancelled.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error deleting user: {ex.Message}");
        }
    }

    private async Task ShowUserStatisticsAsync()
    {
        try
        {
            Console.WriteLine("\n📊 User Statistics");
            
            var users = await _unitOfWork.Users.GetAllAsync();
            var totalUsers = users.Count();
            var activeUsers = users.Count(u => u.Status == UserStatus.Active);
            var inactiveUsers = users.Count(u => u.Status == UserStatus.Inactive);
            var suspendedUsers = users.Count(u => u.Status == UserStatus.Suspended);

            var roleStats = users.GroupBy(u => u.Role)
                .Select(g => new { Role = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count);

            Console.WriteLine($"Total Users: {totalUsers}");
            Console.WriteLine($"Active: {activeUsers} ({(double)activeUsers / totalUsers * 100:F1}%)");
            Console.WriteLine($"Inactive: {inactiveUsers} ({(double)inactiveUsers / totalUsers * 100:F1}%)");
            Console.WriteLine($"Suspended: {suspendedUsers} ({(double)suspendedUsers / totalUsers * 100:F1}%)");

            Console.WriteLine("\nBy Role:");
            foreach (var roleStat in roleStats)
            {
                Console.WriteLine($"  {roleStat.Role}: {roleStat.Count} ({(double)roleStat.Count / totalUsers * 100:F1}%)");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error getting user statistics: {ex.Message}");
        }
    }

    // Placeholder methods for other CRUD operations
    private async Task ManageProductsAsync()
    {
        Console.WriteLine("\n📦 Product Management - Coming Soon!");
        Console.WriteLine("This will include product CRUD operations, inventory management, and more.");
    }

    private async Task ManageOrdersAsync()
    {
        Console.WriteLine("\n🛒 Order Management - Coming Soon!");
        Console.WriteLine("This will include order CRUD operations, status tracking, and more.");
    }

    private async Task ManageReviewsAsync()
    {
        Console.WriteLine("\n📝 Review Management - Coming Soon!");
        Console.WriteLine("This will include review CRUD operations, rating analysis, and more.");
    }

    private async Task ManageInventoryAsync()
    {
        Console.WriteLine("\n📊 Inventory Management - Coming Soon!");
        Console.WriteLine("This will include inventory CRUD operations, stock tracking, and more.");
    }

    private async Task BulkOperationsAsync()
    {
        Console.WriteLine("\n🔄 Bulk Operations - Coming Soon!");
        Console.WriteLine("This will include bulk create, update, delete operations.");
    }

    // Placeholder methods for other menus
    private async Task ShowDatabaseManagementMenuAsync()
    {
        Console.WriteLine("\n🗄️ Database Management - Coming Soon!");
    }

    private async Task ShowQueryMenuAsync()
    {
        Console.WriteLine("\n🔍 Query Operations - Coming Soon!");
    }

    private async Task ShowDatabaseObjectsMenuAsync()
    {
        Console.WriteLine("\n📊 Database Objects - Coming Soon!");
    }

    private async Task ShowMigrationTestingMenuAsync()
    {
        Console.WriteLine("\n🔄 Migration Testing - Coming Soon!");
    }

    private async Task ShowStatisticsMenuAsync()
    {
        Console.WriteLine("\n📈 Statistics & Reports - Coming Soon!");
    }

    private async Task ShowSettingsMenuAsync()
    {
        Console.WriteLine("\n⚙️ Settings - Coming Soon!");
    }

    private string GetUserChoice()
    {
        Console.Write("Enter your choice: ");
        return Console.ReadLine() ?? "";
    }
}
