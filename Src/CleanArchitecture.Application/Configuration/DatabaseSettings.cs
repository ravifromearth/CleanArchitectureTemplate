namespace CleanArchitecture.Application.Configuration;

public class DatabaseSettings
{
    public bool UseSqlServer { get; set; } = true;
    public ConnectionStrings ConnectionStrings { get; set; } = new();
    public bool AutoCreateDatabase { get; set; } = true;
    public bool AutoApplyMigrations { get; set; } = true;
    public bool AutoExecuteScripts { get; set; } = true;
    public bool AutoSeedData { get; set; } = false;
    public bool PromptForSeeding { get; set; } = true;
    public int SeedDataCount { get; set; } = 1000;
}

public class ConnectionStrings
{
    public string SqlServer { get; set; } = string.Empty;
    public string PostgreSQL { get; set; } = string.Empty;
}

public class ApplicationSettings
{
    public string ApplicationName { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public string Environment { get; set; } = string.Empty;
}

