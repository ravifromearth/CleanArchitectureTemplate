namespace EntityFrameworkCore8Samples.Domain.Entities.ViewModels;

/// <summary>
/// Represents the vw_ProductInventoryStatus view
/// </summary>
public class ProductInventoryStatusView
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal? SalePrice { get; set; }
    public string ProductStatus { get; set; } = string.Empty;
    public string ProductType { get; set; } = string.Empty;
    public string? WarehouseCode { get; set; }
    public int? Quantity { get; set; }
    public int? AvailableQuantity { get; set; }
    public int? ReservedQuantity { get; set; }
    public string? InventoryStatus { get; set; }
    public DateTime? LastUpdated { get; set; }
    public string? StockLevel { get; set; }
}


