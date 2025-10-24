# Database Scripts - Views, Functions, and Stored Procedures

This directory contains SQL scripts for creating database views, functions, and stored procedures that demonstrate complex database operations.

## 📁 Script Files

### 1. **01_Views.sql**
Creates database views for common data aggregation queries.

#### Views Created:
- **vw_UserProfileSummary**: User profile information with order statistics
- **vw_ProductInventoryStatus**: Product details with inventory status and stock levels
- **vw_OrderDetailsSummary**: Order details with customer and item information
- **vw_ProductReviewsSummary**: Product review aggregations and ratings breakdown
- **vw_ActiveUserSessions**: Currently active user sessions with activity tracking

### 2. **02_Functions.sql**
Creates scalar and table-valued functions for calculations and data retrieval.

#### Scalar Functions:
- **fn_CalculateUserLifetimeValue**: Calculates total value of all orders for a user
  ```sql
  SELECT dbo.fn_CalculateUserLifetimeValue('USER-GUID-HERE') AS LifetimeValue
  ```

- **fn_GetProductAverageRating**: Gets average rating for a product
  ```sql
  SELECT dbo.fn_GetProductAverageRating('PRODUCT-GUID-HERE') AS AverageRating
  ```

- **fn_CalculateOrderDiscount**: Calculates role-based discounts for orders
  ```sql
  SELECT dbo.fn_CalculateOrderDiscount(1000.00, 'Admin') AS Discount
  ```

#### Table-Valued Functions:
- **fn_GetUserOrderHistory**: Returns order history for a user with date filtering
  ```sql
  SELECT * FROM dbo.fn_GetUserOrderHistory('USER-GUID-HERE', '2024-01-01', '2024-12-31')
  ```

- **fn_GetTopSellingProducts**: Returns top N selling products
  ```sql
  SELECT * FROM dbo.fn_GetTopSellingProducts(10, '2024-01-01', '2024-12-31')
  ```

- **fn_GetProductsLowOnStock**: Returns products with inventory below threshold
  ```sql
  SELECT * FROM dbo.fn_GetProductsLowOnStock(20)
  ```

- **fn_GetUserActivitySummary**: Returns comprehensive user activity statistics
  ```sql
  SELECT * FROM dbo.fn_GetUserActivitySummary('USER-GUID-HERE')
  ```

### 3. **03_StoredProcedures.sql**
Creates stored procedures for complex business operations with transactions.

#### Stored Procedures:

- **sp_CreateOrderWithItems**: Creates an order with items and updates inventory
  ```sql
  DECLARE @OrderId UNIQUEIDENTIFIER;
  EXEC sp_CreateOrderWithItems 
    @UserId = 'USER-GUID-HERE',
    @OrderNumber = 'ORD-2024-001',
    @ShippingAddressJson = '{}',
    @PaymentMethod = 'CreditCard',
    @SubTotal = 100.00,
    @TaxAmount = 8.00,
    @ShippingCost = 10.00,
    @Items = '[{"ProductId":"PRODUCT-GUID","Quantity":2,"UnitPrice":50.00}]',
    @OrderId = @OrderId OUTPUT;
  SELECT @OrderId AS NewOrderId;
  ```

- **sp_UpdateOrderStatus**: Updates order status with inventory management
  ```sql
  EXEC sp_UpdateOrderStatus 
    @OrderId = 'ORDER-GUID-HERE',
    @NewStatus = 'Shipped',
    @UpdatedBy = 'USER-GUID-HERE',
    @Notes = 'Order shipped via FedEx';
  ```

- **sp_RestockProductInventory**: Restocks product inventory
  ```sql
  EXEC sp_RestockProductInventory 
    @ProductId = 'PRODUCT-GUID-HERE',
    @WarehouseCode = 'WH-001',
    @Quantity = 50,
    @RestockedBy = 'USER-GUID-HERE';
  ```

- **sp_GetSalesReport**: Generates sales report for date range
  ```sql
  EXEC sp_GetSalesReport 
    @FromDate = '2024-01-01',
    @ToDate = '2024-12-31',
    @Status = 'Delivered';
  ```

- **sp_ProcessUserRegistration**: Processes new user registration with profile creation
  ```sql
  DECLARE @UserId UNIQUEIDENTIFIER;
  EXEC sp_ProcessUserRegistration 
    @Username = 'newuser',
    @Email = 'newuser@example.com',
    @PasswordHash = 0x123456,
    @FirstName = 'John',
    @LastName = 'Doe',
    @PhoneNumber = '+1234567890',
    @UserId = @UserId OUTPUT;
  SELECT @UserId AS NewUserId;
  ```

- **sp_GetCustomerInsights**: Returns customer analytics and insights
  ```sql
  EXEC sp_GetCustomerInsights @TopN = 100;
  ```

## 🚀 Usage in Application

The scripts are automatically executed on application startup through the `DatabaseScriptExecutor` service.

### Manual Execution

If you need to manually run the scripts:

```bash
# SQL Server Management Studio (SSMS)
1. Open SQL Server Management Studio
2. Connect to your SQL Server instance
3. Open each script file (01_Views.sql, 02_Functions.sql, 03_StoredProcedures.sql)
4. Execute them in order

# Command Line (sqlcmd)
sqlcmd -S SERVER_NAME -d DATABASE_NAME -i 01_Views.sql
sqlcmd -S SERVER_NAME -d DATABASE_NAME -i 02_Functions.sql
sqlcmd -S SERVER_NAME -d DATABASE_NAME -i 03_StoredProcedures.sql
```

## 📊 Using in Entity Framework Core

### Querying Views
```csharp
// Get user profile summaries
var summaries = await context.UserProfileSummaries.ToListAsync();

// Get product inventory status
var inventory = await context.ProductInventoryStatuses
    .Where(p => p.StockLevel == "Low Stock")
    .ToListAsync();
```

### Calling Functions
```csharp
// Calculate user lifetime value
var userId = new SqlParameter("@UserId", userId);
var lifetimeValue = await context.Database
    .SqlQuery<decimal>($"SELECT dbo.fn_CalculateUserLifetimeValue({userId}) AS Value")
    .FirstOrDefaultAsync();
```

### Executing Stored Procedures
```csharp
// Update order status
await context.Database.ExecuteSqlRawAsync(
    "EXEC sp_UpdateOrderStatus @OrderId, @NewStatus, @UpdatedBy, @Notes",
    new SqlParameter("@OrderId", orderId),
    new SqlParameter("@NewStatus", "Shipped"),
    new SqlParameter("@UpdatedBy", userId),
    new SqlParameter("@Notes", "Order shipped"));
```

## 🔧 PostgreSQL Support

**Note**: These scripts are designed for SQL Server. For PostgreSQL migration, you'll need to:

1. Convert `UNIQUEIDENTIFIER` to `UUID`
2. Replace `GETUTCDATE()` with `NOW()` or `CURRENT_TIMESTAMP`
3. Convert stored procedures to PostgreSQL functions
4. Adjust JSON handling (`OPENJSON` → `json_array_elements`)
5. Change `EXEC` to `CALL` for procedures
6. Use PostgreSQL-specific syntax for functions

## 📝 Migration Challenges

These database objects represent common challenges when migrating from SQL Server to PostgreSQL:

1. **Stored Procedures**: PostgreSQL uses functions instead of procedures
2. **OUTPUT Parameters**: Different syntax in PostgreSQL
3. **Table-Valued Functions**: PostgreSQL uses `RETURNS TABLE` or `SETOF`
4. **JSON Handling**: Different JSON functions
5. **Error Handling**: `TRY/CATCH` vs `EXCEPTION` blocks
6. **Transactions**: Similar but different syntax

## ✅ Benefits

- **Performance**: Pre-compiled and optimized execution plans
- **Reusability**: Can be called from multiple applications
- **Security**: Parameterized to prevent SQL injection
- **Maintainability**: Business logic centralized in database
- **Consistency**: Same operations across different clients

## 🎯 Testing

The application automatically tests all views, functions, and stored procedures on startup and displays results in the console.





