using CleanArchitecture.Application.Interfaces;
using CleanArchitecture.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Infrastructure.Services;

/// <summary>
/// Service to execute database scripts for Views, Functions, and Stored Procedures
/// </summary>
public class DatabaseScriptExecutor : IDatabaseScriptExecutor
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<DatabaseScriptExecutor> _logger;

    public DatabaseScriptExecutor(
        ApplicationDbContext context,
        ILogger<DatabaseScriptExecutor> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task ExecuteScriptsAsync(string scriptsPath)
    {
        try
        {
            _logger.LogInformation("Starting execution of database scripts from: {ScriptsPath}", scriptsPath);

            if (!Directory.Exists(scriptsPath))
            {
                _logger.LogWarning("Scripts directory not found: {ScriptsPath}", scriptsPath);
                return;
            }

            // Get all .sql files and execute them in order
            var scriptFiles = Directory.GetFiles(scriptsPath, "*.sql")
                .OrderBy(f => f)
                .ToList();

            if (!scriptFiles.Any())
            {
                _logger.LogWarning("No SQL script files found in: {ScriptsPath}", scriptsPath);
                return;
            }

            _logger.LogInformation("Found {Count} script files to execute", scriptFiles.Count);

            foreach (var scriptFile in scriptFiles)
            {
                await ExecuteScriptFileAsync(scriptFile);
            }

            _logger.LogInformation("Successfully executed all database scripts");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing database scripts from: {ScriptsPath}", scriptsPath);
            throw;
        }
    }

    public async Task ExecuteScriptFileAsync(string filePath)
    {
        try
        {
            var fileName = Path.GetFileName(filePath);
            _logger.LogInformation("Executing script: {FileName}", fileName);

            var scriptContent = await File.ReadAllTextAsync(filePath);

            if (string.IsNullOrWhiteSpace(scriptContent))
            {
                _logger.LogWarning("Script file is empty: {FileName}", fileName);
                return;
            }

            // Split the script by GO statements (SQL Server batch separator)
            var batches = SplitScriptIntoBatches(scriptContent);

            foreach (var batch in batches)
            {
                if (!string.IsNullOrWhiteSpace(batch))
                {
                    try
                    {
                        await _context.Database.ExecuteSqlRawAsync(batch);
                    }
                    catch (Exception ex) when (ex.Message.Contains("already an object named") || 
                                             ex.Message.Contains("already exists") ||
                                             ex.Message.Contains("There is already"))
                    {
                        // Skip objects that already exist - this is expected behavior
                        _logger.LogDebug("Object already exists, skipping: {Batch}", batch.Substring(0, Math.Min(50, batch.Length)));
                        continue;
                    }
                }
            }

            _logger.LogInformation("Successfully executed script: {FileName}", fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing script file: {FilePath}", filePath);
            throw;
        }
    }

    public async Task<bool> ShouldExecuteScriptsAsync()
    {
        try
        {
            // Check if any of the views exist
            var viewExists = await _context.Database
                .SqlQueryRaw<int>("SELECT COUNT(*) AS Value FROM sys.views WHERE name = 'vw_UserProfileSummary'")
                .FirstOrDefaultAsync();

            // If view doesn't exist, we should execute scripts
            return viewExists == 0;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Could not check if scripts should be executed, will attempt to execute them");
            return true;
        }
    }

    private List<string> SplitScriptIntoBatches(string scriptContent)
    {
        // Split by GO statements (case-insensitive, must be on its own line)
        var batches = new List<string>();
        var lines = scriptContent.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
        var currentBatch = new List<string>();

        foreach (var line in lines)
        {
            var trimmedLine = line.Trim();

            // Check if line is a GO statement
            if (trimmedLine.Equals("GO", StringComparison.OrdinalIgnoreCase))
            {
                if (currentBatch.Any())
                {
                    batches.Add(string.Join(Environment.NewLine, currentBatch));
                    currentBatch.Clear();
                }
            }
            else
            {
                currentBatch.Add(line);
            }
        }

        // Add remaining batch
        if (currentBatch.Any())
        {
            batches.Add(string.Join(Environment.NewLine, currentBatch));
        }

        return batches;
    }
}

