# PowerShell script to test SQL Server connection
$serverName = "TRI02L-PW03VMZD\SQLEXPRESS"
$database = "ComplexDB"
$username = "pocuser"
$password = "StrongPassword123!"

Write-Host "Testing SQL Server Connection..." -ForegroundColor Yellow
Write-Host "Server: $serverName"
Write-Host "Database: $database"
Write-Host "User: $username"
Write-Host ""

try {
    $connectionString = "Server=$serverName;Database=master;User Id=$username;Password=$password;Encrypt=False;TrustServerCertificate=True;Connect Timeout=5;"
    $connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
    
    Write-Host "Attempting to open connection..." -ForegroundColor Cyan
    $connection.Open()
    
    Write-Host "✓ Connection successful!" -ForegroundColor Green
    Write-Host "SQL Server Version: $($connection.ServerVersion)" -ForegroundColor Green
    
    # Check if database exists
    $command = $connection.CreateCommand()
    $command.CommandText = "SELECT COUNT(*) FROM sys.databases WHERE name = '$database'"
    $dbExists = $command.ExecuteScalar()
    
    if ($dbExists -eq 1) {
        Write-Host "✓ Database '$database' exists" -ForegroundColor Green
    } else {
        Write-Host "✗ Database '$database' does NOT exist" -ForegroundColor Red
    }
    
    $connection.Close()
}
catch {
    Write-Host "✗ Connection failed!" -ForegroundColor Red
    Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Red
    
    if ($_.Exception.Message -like "*timeout*") {
        Write-Host "`nThis is a TIMEOUT issue. Possible causes:" -ForegroundColor Yellow
        Write-Host "  1. SQL Server is not running"
        Write-Host "  2. SQL Server Browser service is not running"
        Write-Host "  3. Firewall is blocking the connection"
        Write-Host "  4. TCP/IP is not enabled for SQL Server"
    }
    elseif ($_.Exception.Message -like "*login failed*") {
        Write-Host "`nThis is an AUTHENTICATION issue. Possible causes:" -ForegroundColor Yellow
        Write-Host "  1. User 'pocuser' does not exist"
        Write-Host "  2. Password is incorrect"
        Write-Host "  3. SQL Server is not configured for SQL Authentication"
    }
}

Write-Host "`nPress any key to continue..."
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")


