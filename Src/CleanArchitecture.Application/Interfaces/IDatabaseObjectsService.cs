using CleanArchitecture.Domain.ViewModels;

namespace CleanArchitecture.Application.Interfaces;

/// <summary>
/// Service for demonstrating Views, Functions, and Stored Procedures
/// </summary>
public interface IDatabaseObjectsService
{
    // Views
    Task<List<UserProfileSummaryView>> GetUserProfileSummariesAsync();
    Task<List<ProductInventoryStatusView>> GetProductInventoryStatusAsync();
    Task<List<OrderDetailsSummaryView>> GetOrderDetailsSummariesAsync();
    
    // Functions
    Task<decimal> CalculateUserLifetimeValueAsync(Guid userId);
    Task<decimal> GetProductAverageRatingAsync(Guid productId);
    
    // Stored Procedures
    Task<Guid> CreateOrderWithItemsAsync(Guid userId, string orderNumber, List<OrderItemDto> items);
    Task UpdateOrderStatusAsync(Guid orderId, string newStatus, Guid updatedBy, string? notes = null);
    Task RestockProductInventoryAsync(Guid productId, string warehouseCode, int quantity, Guid restockedBy);
    Task<List<SalesReportDto>> GetSalesReportAsync(DateTime? fromDate = null, DateTime? toDate = null, string? status = null);
}

// DTOs for Stored Procedure parameters
public class OrderItemDto
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}

public class SalesReportDto
{
    public DateTime OrderDate { get; set; }
    public int TotalOrders { get; set; }
    public int UniqueCustomers { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal AverageOrderValue { get; set; }
    public decimal TotalTax { get; set; }
    public decimal TotalShipping { get; set; }
    public int TotalItemsSold { get; set; }
}


