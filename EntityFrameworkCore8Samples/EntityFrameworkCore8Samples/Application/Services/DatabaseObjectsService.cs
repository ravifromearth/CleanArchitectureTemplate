using EntityFrameworkCore8Samples.Application.Interfaces;
using EntityFrameworkCore8Samples.Domain.Entities.ViewModels;
using EntityFrameworkCore8Samples.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace EntityFrameworkCore8Samples.Application.Services;

/// <summary>
/// Service implementation for demonstrating Views, Functions, and Stored Procedures
/// </summary>
public class DatabaseObjectsService : IDatabaseObjectsService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<DatabaseObjectsService> _logger;

    public DatabaseObjectsService(
        ApplicationDbContext context,
        ILogger<DatabaseObjectsService> logger)
    {
        _context = context;
        _logger = logger;
    }

    #region Views

    public async Task<List<UserProfileSummaryView>> GetUserProfileSummariesAsync()
    {
        try
        {
            _logger.LogInformation("Querying vw_UserProfileSummary view");

            var results = await _context.Database
                .SqlQuery<UserProfileSummaryView>($"SELECT * FROM vw_UserProfileSummary")
                .ToListAsync();

            _logger.LogInformation("Retrieved {Count} user profile summaries", results.Count);
            return results;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error querying user profile summaries view");
            throw;
        }
    }

    public async Task<List<ProductInventoryStatusView>> GetProductInventoryStatusAsync()
    {
        try
        {
            _logger.LogInformation("Querying vw_ProductInventoryStatus view");

            var results = await _context.Database
                .SqlQuery<ProductInventoryStatusView>($"SELECT * FROM vw_ProductInventoryStatus")
                .ToListAsync();

            _logger.LogInformation("Retrieved {Count} product inventory status records", results.Count);
            return results;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error querying product inventory status view");
            throw;
        }
    }

    public async Task<List<OrderDetailsSummaryView>> GetOrderDetailsSummariesAsync()
    {
        try
        {
            _logger.LogInformation("Querying vw_OrderDetailsSummary view");

            var results = await _context.Database
                .SqlQuery<OrderDetailsSummaryView>($"SELECT * FROM vw_OrderDetailsSummary")
                .ToListAsync();

            _logger.LogInformation("Retrieved {Count} order details summaries", results.Count);
            return results;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error querying order details summary view");
            throw;
        }
    }

    #endregion

    #region Functions

    public async Task<decimal> CalculateUserLifetimeValueAsync(Guid userId)
    {
        try
        {
            _logger.LogInformation("Calculating lifetime value for user: {UserId}", userId);

            var userIdParam = new SqlParameter("@UserId", userId);
            
            var result = await _context.Database
                .SqlQuery<decimal>($"SELECT dbo.fn_CalculateUserLifetimeValue({userIdParam}) AS Value")
                .FirstOrDefaultAsync();

            _logger.LogInformation("User {UserId} lifetime value: {Value:C}", userId, result);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating user lifetime value for: {UserId}", userId);
            throw;
        }
    }

    public async Task<decimal> GetProductAverageRatingAsync(Guid productId)
    {
        try
        {
            _logger.LogInformation("Getting average rating for product: {ProductId}", productId);

            var productIdParam = new SqlParameter("@ProductId", productId);
            
            var result = await _context.Database
                .SqlQuery<decimal>($"SELECT dbo.fn_GetProductAverageRating({productIdParam}) AS Value")
                .FirstOrDefaultAsync();

            _logger.LogInformation("Product {ProductId} average rating: {Rating}", productId, result);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting product average rating for: {ProductId}", productId);
            throw;
        }
    }

    #endregion

    #region Stored Procedures

    public async Task<Guid> CreateOrderWithItemsAsync(Guid userId, string orderNumber, List<OrderItemDto> items)
    {
        try
        {
            _logger.LogInformation("Creating order for user: {UserId} with {ItemCount} items", userId, items.Count);

            // Calculate totals
            var subTotal = items.Sum(i => i.Quantity * i.UnitPrice);
            var taxAmount = subTotal * 0.08m; // 8% tax
            var shippingCost = 10.00m; // Flat shipping

            // Serialize items to JSON
            var itemsJson = JsonSerializer.Serialize(items.Select(i => new
            {
                ProductId = i.ProductId.ToString(),
                i.Quantity,
                i.UnitPrice
            }));

            // Execute stored procedure
            var userIdParam = new SqlParameter("@UserId", userId);
            var orderNumberParam = new SqlParameter("@OrderNumber", orderNumber);
            var shippingAddressParam = new SqlParameter("@ShippingAddressJson", "{}");
            var paymentMethodParam = new SqlParameter("@PaymentMethod", "CreditCard");
            var subTotalParam = new SqlParameter("@SubTotal", subTotal);
            var taxAmountParam = new SqlParameter("@TaxAmount", taxAmount);
            var shippingCostParam = new SqlParameter("@ShippingCost", shippingCost);
            var itemsParam = new SqlParameter("@Items", itemsJson);
            var orderIdParam = new SqlParameter
            {
                ParameterName = "@OrderId",
                SqlDbType = System.Data.SqlDbType.UniqueIdentifier,
                Direction = System.Data.ParameterDirection.Output
            };

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_CreateOrderWithItems @UserId, @OrderNumber, @ShippingAddressJson, @PaymentMethod, " +
                "@SubTotal, @TaxAmount, @ShippingCost, @Items, @OrderId OUTPUT",
                userIdParam, orderNumberParam, shippingAddressParam, paymentMethodParam,
                subTotalParam, taxAmountParam, shippingCostParam, itemsParam, orderIdParam);

            var orderId = (Guid)orderIdParam.Value;
            _logger.LogInformation("Order created successfully: {OrderId}", orderId);
            
            return orderId;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating order for user: {UserId}", userId);
            throw;
        }
    }

    public async Task UpdateOrderStatusAsync(Guid orderId, string newStatus, Guid updatedBy, string? notes = null)
    {
        try
        {
            _logger.LogInformation("Updating order {OrderId} status to: {NewStatus}", orderId, newStatus);

            var orderIdParam = new SqlParameter("@OrderId", orderId);
            var newStatusParam = new SqlParameter("@NewStatus", newStatus);
            var updatedByParam = new SqlParameter("@UpdatedBy", updatedBy);
            var notesParam = new SqlParameter("@Notes", (object?)notes ?? DBNull.Value);

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_UpdateOrderStatus @OrderId, @NewStatus, @UpdatedBy, @Notes",
                orderIdParam, newStatusParam, updatedByParam, notesParam);

            _logger.LogInformation("Order {OrderId} status updated successfully", orderId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating order status for: {OrderId}", orderId);
            throw;
        }
    }

    public async Task RestockProductInventoryAsync(Guid productId, string warehouseCode, int quantity, Guid restockedBy)
    {
        try
        {
            _logger.LogInformation("Restocking product {ProductId} at warehouse {WarehouseCode} with {Quantity} units",
                productId, warehouseCode, quantity);

            var productIdParam = new SqlParameter("@ProductId", productId);
            var warehouseCodeParam = new SqlParameter("@WarehouseCode", warehouseCode);
            var quantityParam = new SqlParameter("@Quantity", quantity);
            var restockedByParam = new SqlParameter("@RestockedBy", restockedBy);

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_RestockProductInventory @ProductId, @WarehouseCode, @Quantity, @RestockedBy",
                productIdParam, warehouseCodeParam, quantityParam, restockedByParam);

            _logger.LogInformation("Product {ProductId} restocked successfully", productId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error restocking product: {ProductId}", productId);
            throw;
        }
    }

    public async Task<List<SalesReportDto>> GetSalesReportAsync(DateTime? fromDate = null, DateTime? toDate = null, string? status = null)
    {
        try
        {
            _logger.LogInformation("Generating sales report from {FromDate} to {ToDate}",
                fromDate?.ToString("yyyy-MM-dd") ?? "30 days ago", toDate?.ToString("yyyy-MM-dd") ?? "today");

            var fromDateParam = new SqlParameter("@FromDate", (object?)fromDate ?? DBNull.Value);
            var toDateParam = new SqlParameter("@ToDate", (object?)toDate ?? DBNull.Value);
            var statusParam = new SqlParameter("@Status", (object?)status ?? DBNull.Value);

            var results = await _context.Database
                .SqlQuery<SalesReportDto>($"EXEC sp_GetSalesReport {fromDateParam}, {toDateParam}, {statusParam}")
                .ToListAsync();

            _logger.LogInformation("Sales report generated with {Count} records", results.Count);
            return results;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating sales report");
            throw;
        }
    }

    #endregion
}





