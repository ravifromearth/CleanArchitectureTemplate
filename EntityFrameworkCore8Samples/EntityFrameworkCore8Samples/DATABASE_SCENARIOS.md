# Database Management Scenarios

This document explains how the application handles different database scenarios and user interactions.

## 🎯 **Database Scenarios Handled**

### **Scenario 1: Database Doesn't Exist**
**What happens:**
- ✅ Application detects database is not accessible
- ✅ Automatically creates the database using `EnsureCreatedAsync()`
- ✅ Applies any pending migrations
- ✅ Executes database scripts (Views, Functions, Stored Procedures)
- ✅ Prompts user for data seeding options

**User Options:**
1. **Seed with 1,000 records per entity** (Recommended for testing)
2. **Keep database empty** (Tables only)
3. **Custom amount** (1-1000 records per entity)

### **Scenario 2: Database Exists but Empty**
**What happens:**
- ✅ Application detects database exists but has no data
- ✅ Applies any pending migrations
- ✅ Executes database scripts
- ✅ Prompts user for data seeding options

**User Options:**
1. **Seed with 1,000 records per entity** (Recommended for testing)
2. **Keep database empty** (Tables only)
3. **Custom amount** (1-1000 records per entity)

### **Scenario 3: Database Exists with Data**
**What happens:**
- ✅ Application detects existing data
- ✅ Shows data statistics (record counts)
- ✅ Prompts user for data management options

**User Options:**
1. **Keep existing data** (Skip seeding)
2. **Add more test data** (Append to existing)
3. **Clear and reseed** (⚠️ WARNING: Deletes all data!)

## 🔧 **Configuration Options**

### **appsettings.json**
```json
{
  "DatabaseSettings": {
    "UseSqlServer": true,
    "AutoCreateDatabase": true,
    "AutoApplyMigrations": true,
    "AutoExecuteScripts": true,
    "AutoSeedData": false,
    "PromptForSeeding": true,
    "SeedDataCount": 1000
  }
}
```

### **Connection Strings**
```json
{
  "ConnectionStrings": {
    "SqlServer": "Data Source=SERVER\\INSTANCE;Initial Catalog=ComplexDB;Integrated Security=SSPI;Encrypt=False;",
    "PostgreSQL": "Host=localhost;Database=ComplexDB;Username=postgres;Password=password"
  }
}
```

## 🚀 **Application Flow**

### **Step 1: Database Accessibility Check**
```
✓ Checking database accessibility...
✓ Database is accessible
```

**If database is not accessible:**
```
✗ Database is not accessible: Login failed for user 'username'
Connection String: Data Source=SERVER\INSTANCE;Initial Catalog=ComplexDB;...
```

### **Step 2: Database Creation**
```
✓ Ensuring database exists...
✓ Database created successfully
```
**OR**
```
✓ Database already exists
```

### **Step 3: Migration Application**
```
✓ Applying database migrations...
✓ Database is up to date (no pending migrations)
```
**OR**
```
Found 3 pending migrations:
  - 20240101000001_Initial
  - 20240101000002_AddUserProfile
  - 20240101000003_AddProductInventory
✓ Migrations applied successfully
```

### **Step 4: Script Execution**
```
✓ Executing database scripts...
✓ Database scripts executed successfully
```

### **Step 5: Data Seeding Management**

#### **Empty Database:**
```
=== Database Seeding Options ===
1. Yes - Seed with 1,000 records per entity (Recommended for testing)
2. No - Keep database empty (Tables only)
3. Custom - Seed with custom amount

Enter your choice (1-3): 
```

#### **Existing Data:**
```
=== Existing Data Detected ===
Database already contains data:
  - Users: 1,000
  - Products: 1,000
  - Orders: 1,000
  - Total Records: 3,000

What would you like to do?
1. Keep existing data (Skip seeding)
2. Add more test data (Append to existing)
3. Clear and reseed (WARNING: This will delete all existing data!)

Enter your choice (1-3): 
```

## 🛡️ **Safety Features**

### **Data Protection**
- ✅ **Never deletes data without explicit confirmation**
- ✅ **Shows clear warnings before destructive operations**
- ✅ **Requires typing "YES" to confirm data deletion**
- ✅ **Preserves existing data by default**

### **Error Handling**
- ✅ **Graceful handling of connection failures**
- ✅ **Detailed error messages with connection string info**
- ✅ **Continues operation even if some scripts fail**
- ✅ **Logs all operations for debugging**

### **User Control**
- ✅ **Interactive prompts for all data operations**
- ✅ **Clear options with descriptions**
- ✅ **Ability to cancel at any time**
- ✅ **Custom seeding amounts**

## 📊 **Database Statistics**

The application provides detailed statistics:

```csharp
public class DatabaseStatistics
{
    public bool DatabaseExists { get; set; }
    public bool HasData { get; set; }
    public int UserCount { get; set; }
    public int ProductCount { get; set; }
    public int OrderCount { get; set; }
    public int TotalRecords { get; set; }
    public DateTime? LastSeeded { get; set; }
    public string DatabaseName { get; set; }
    public string ServerName { get; set; }
}
```

## 🔄 **Migration Scenarios**

### **SQL Server to PostgreSQL**
1. Change `UseSqlServer: false` in appsettings.json
2. Update PostgreSQL connection string
3. Run application - it will:
   - Create PostgreSQL database
   - Apply migrations (converted for PostgreSQL)
   - Execute scripts (may need manual conversion)
   - Handle data seeding

### **Schema Updates**
- Application automatically detects pending migrations
- Applies them safely without data loss
- Logs all migration steps

## 🎮 **Interactive Commands**

### **During Application Startup:**
- **1-3**: Choose seeding option
- **YES**: Confirm data deletion
- **Custom numbers**: Specify record counts

### **Example Session:**
```
=== Entity Framework Core 8 - Database Management ===

1. Checking database accessibility...
✓ Database is accessible

2. Ensuring database exists...
✓ Database already exists

3. Applying database migrations...
✓ Database is up to date (no pending migrations)

4. Executing database scripts...
✓ Database scripts executed successfully

5. Managing data seeding...

=== Database Seeding Options ===
1. Yes - Seed with 1,000 records per entity (Recommended for testing)
2. No - Keep database empty (Tables only)
3. Custom - Seed with custom amount

Enter your choice (1-3): 1

✓ User chose to seed with 1,000 records per entity
✓ Data seeding completed successfully

=== Testing Complex Queries ===
✓ Found 1,000 users
✓ Found 1,000 products
✓ Found 1,000 orders
```

## 🚨 **Troubleshooting**

### **Common Issues:**

1. **"Cannot connect to database"**
   - Check SQL Server is running
   - Verify connection string
   - Check firewall settings
   - Ensure user has permissions

2. **"Failed to create database"**
   - Check user has CREATE DATABASE permission
   - Verify disk space
   - Check for naming conflicts

3. **"Failed to apply migrations"**
   - Check for schema conflicts
   - Verify migration files are valid
   - Check for data type incompatibilities

4. **"Scripts execution failed"**
   - Check SQL syntax compatibility
   - Verify user has EXECUTE permissions
   - Check for existing objects

### **Recovery Options:**
- **Reset database**: Delete and recreate
- **Manual seeding**: Use custom amounts
- **Skip problematic scripts**: Continue with others
- **Check logs**: Detailed error information provided

This robust system ensures your application handles all database scenarios gracefully while giving users full control over their data! 🎯




