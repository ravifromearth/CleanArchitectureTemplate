namespace CleanArchitecture.Application.Interfaces;

/// <summary>
/// Interface for executing database scripts (Views, Functions, Stored Procedures)
/// </summary>
public interface IDatabaseScriptExecutor
{
    /// <summary>
    /// Executes all SQL scripts in the specified directory
    /// </summary>
    Task ExecuteScriptsAsync(string scriptsPath);
    
    /// <summary>
    /// Executes a single SQL script file
    /// </summary>
    Task ExecuteScriptFileAsync(string filePath);
    
    /// <summary>
    /// Checks if scripts need to be executed (on startup)
    /// </summary>
    Task<bool> ShouldExecuteScriptsAsync();
}


