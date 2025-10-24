# Complex Entity Framework Core 8 Features for SQL Server to PostgreSQL Migration Testing

This application demonstrates complex Entity Framework Core 8 features that can cause issues during SQL Server to PostgreSQL migration. It's designed to help identify potential problems and test migration strategies.

## Complex Features Tested

### 1. Array Data Types
- **SQL Server**: Uses JSON serialization with `nvarchar(max)`
- **PostgreSQL**: Native array support with `text[]`, `integer[]`
- **Challenge**: Different query syntax and performance characteristics

### 2. JSON Data Storage
- **SQL Server**: `nvarchar(max)` with JSON functions
- **PostgreSQL**: Native `jsonb` type with advanced indexing
- **Challenge**: Different JSON query syntax and performance

### 3. Complex Types (Value Objects)
- **Both**: Owned entity types
- **Challenge**: Different column naming conventions and null handling

### 4. Decimal Precision
- **SQL Server**: `decimal(18,6)`, `money` type
- **PostgreSQL**: `numeric(18,6)`, `money` type
- **Challenge**: Slight differences in precision handling

### 5. Enum Storage
- **Both**: String-based enum storage
- **Challenge**: Case sensitivity and string comparison differences

### 6. Binary Data
- **Both**: `varbinary(max)` / `bytea`
- **Challenge**: Different handling of large binary objects

### 7. Date/Time Types
- **Both**: `DateTime`, `DateOnly`, `TimeOnly`
- **Challenge**: Timezone handling and precision differences

### 8. Complex Relationships
- **Both**: One-to-One, One-to-Many, Many-to-Many
- **Challenge**: Cascade delete behavior and foreign key constraints

## Entity Structure

### Core Entities
- **User**: Complex user with arrays, JSON, enums, and relationships
- **UserProfile**: One-to-One relationship with complex types
- **UserSession**: One-to-Many relationship with arrays and JSON

### Product Entities
- **Product**: Complex product with dimensions, weight, and arrays
- **ProductReview**: Many-to-One relationship with JSON data
- **ProductInventory**: One-to-Many relationship with arrays

### Order Entities
- **Order**: Complex order with addresses and JSON data
- **OrderItem**: Many-to-One relationship with arrays
- **OrderStatusHistory**: One-to-Many relationship with JSON

## Data Types That Cause Migration Issues

### 1. Arrays
```csharp
// SQL Server: Stored as JSON
public string[] Tags { get; set; }

// PostgreSQL: Native arrays
entity.Property(e => e.Tags).HasColumnType("text[]");
```

### 2. JSON Data
```csharp
// SQL Server: nvarchar(max)
public string? Metadata { get; set; }

// PostgreSQL: jsonb
entity.Property(e => e.Metadata).HasColumnType("jsonb");
```

### 3. Complex Types
```csharp
// Both: Owned entity types
public Address? HomeAddress { get; set; }

// Configuration differences in OnModelCreating
```

### 4. Decimal Precision
```csharp
// SQL Server
[Column(TypeName = "decimal(18,6)")]
public decimal CreditScore { get; set; }

// PostgreSQL
entity.Property(e => e.CreditScore).HasColumnType("numeric(18,6)");
```

## Migration Commands

### SQL Server
```bash
# Add migration
add-migration Initial -Context EntityFrameworkCore8Samples.ComplexDbContext

# Update database
update-database -Context EntityFrameworkCore8Samples.ComplexDbContext
```

### PostgreSQL
```bash
# Add migration
add-migration Initial -Context EntityFrameworkCore8Samples.PostgreSQLComplexDbContext

# Update database
update-database -Context EntityFrameworkCore8Samples.PostgreSQLComplexDbContext
```

## Connection Strings

### SQL Server
```csharp
"Data Source=RBHUSHAN-LFI\\SQLEXPRESS07;Initial Catalog=ComplexDB;Integrated Security=SSPI;Encrypt=False;"
```

### PostgreSQL
```csharp
"Host=localhost;Database=ComplexDB;Username=postgres;Password=password"
```

## Common Migration Issues

### 1. Array Queries
```csharp
// This might work differently between databases
var users = context.Users.Where(u => u.Tags.Contains("premium"));
```

### 2. JSON Queries
```csharp
// SQL Server
var users = context.Users.Where(u => u.Metadata.Contains("dark"));

// PostgreSQL (better performance)
var users = context.Users.Where(u => EF.Functions.JsonContains(u.Metadata, "dark"));
```

### 3. Decimal Precision
- SQL Server `money` type vs PostgreSQL `money` type
- Different rounding behaviors
- Precision loss in some cases

### 4. Enum Handling
- Case sensitivity differences
- String comparison variations
- Index usage differences

### 5. Complex Type Mapping
- Column naming conventions
- Null handling differences
- Query generation variations

## Testing Strategy

1. **Create Test Data**: Use the `ComplexTestProgram` to generate complex test data
2. **Run Queries**: Test various query patterns that might fail
3. **Compare Results**: Ensure data integrity across both databases
4. **Performance Testing**: Compare query performance between databases
5. **Migration Testing**: Test actual migration scripts

## Performance Considerations

### SQL Server
- JSON functions can be slow on large datasets
- Array queries require JSON parsing
- Limited JSON indexing options

### PostgreSQL
- Native array support is faster
- Advanced JSON indexing with GIN indexes
- Better query optimization for complex types

## Best Practices for Migration

1. **Test Array Queries**: Ensure array operations work correctly
2. **Validate JSON Data**: Check JSON parsing and querying
3. **Test Complex Types**: Verify owned entity mapping
4. **Check Decimal Precision**: Validate numeric calculations
5. **Test Relationships**: Ensure foreign key constraints work
6. **Performance Testing**: Compare query performance
7. **Data Validation**: Ensure data integrity after migration

## Running the Application

1. **Install Dependencies**: `dotnet restore`
2. **Create SQL Server Database**: Run migrations for `ComplexDbContext`
3. **Create PostgreSQL Database**: Run migrations for `PostgreSQLComplexDbContext`
4. **Run Tests**: Execute the application to test all features
5. **Compare Results**: Analyze differences between databases

## Troubleshooting

### Common Errors
- Array query syntax differences
- JSON function compatibility
- Decimal precision mismatches
- Enum string comparison issues
- Complex type mapping problems

### Solutions
- Use database-specific query syntax
- Implement custom value converters
- Adjust decimal precision settings
- Handle enum case sensitivity
- Configure complex type mapping correctly

This application provides a comprehensive test suite for identifying and resolving SQL Server to PostgreSQL migration challenges in Entity Framework Core 8.




