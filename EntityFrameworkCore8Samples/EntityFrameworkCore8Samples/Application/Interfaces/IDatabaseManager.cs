using EntityFrameworkCore8Samples.Domain.Entities;

namespace EntityFrameworkCore8Samples.Application.Interfaces;

/// <summary>
/// Manages database lifecycle and data seeding scenarios
/// </summary>
public interface IDatabaseManager
{
    /// <summary>
    /// Checks if database exists and is accessible
    /// </summary>
    Task<bool> IsDatabaseAccessibleAsync();
    
    /// <summary>
    /// Checks if database has any data in tables
    /// </summary>
    Task<bool> HasExistingDataAsync();
    
    /// <summary>
    /// Creates database if it doesn't exist
    /// </summary>
    Task<bool> CreateDatabaseIfNotExistsAsync();
    
    /// <summary>
    /// Applies migrations to ensure schema is up to date
    /// </summary>
    Task<bool> ApplyMigrationsAsync();
    
    /// <summary>
    /// Executes database scripts (Views, Functions, Stored Procedures)
    /// </summary>
    Task<bool> ExecuteDatabaseScriptsAsync();
    
    /// <summary>
    /// Seeds data based on user preference and existing data
    /// </summary>
    Task<bool> SeedDataAsync(bool forceSeed = false);
    
    /// <summary>
    /// Gets database statistics
    /// </summary>
    Task<DatabaseStatistics> GetDatabaseStatisticsAsync();
    
    /// <summary>
    /// Prompts user for data seeding preference
    /// </summary>
    Task<bool> PromptUserForSeedingAsync();
}

public class DatabaseStatistics
{
    public bool DatabaseExists { get; set; }
    public bool HasData { get; set; }
    public int UserCount { get; set; }
    public int ProductCount { get; set; }
    public int OrderCount { get; set; }
    public int TotalRecords { get; set; }
    public DateTime? LastSeeded { get; set; }
    public string DatabaseName { get; set; } = string.Empty;
    public string ServerName { get; set; } = string.Empty;
}




