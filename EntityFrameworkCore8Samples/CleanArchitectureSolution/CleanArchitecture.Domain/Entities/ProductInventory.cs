using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EntityFrameworkCore8Samples.Domain.Enums;

namespace EntityFrameworkCore8Samples.Domain.Entities;

public class ProductInventory : BaseEntity
{
    [Required]
    public Guid ProductId { get; set; }
    
    [MaxLength(100)]
    public string? WarehouseCode { get; set; }
    
    [MaxLength(200)]
    public string? Location { get; set; }
    
    public int Quantity { get; set; }
    public int? ReservedQuantity { get; set; }
    public int? AvailableQuantity { get; set; }
    
    [Column(TypeName = "decimal(18,4)")]
    public decimal? UnitCost { get; set; }
    
    [Column(TypeName = "decimal(18,4)")]
    public decimal? UnitPrice { get; set; }
    
    public DateTime LastUpdated { get; set; }
    public DateTime? LastRestocked { get; set; }
    public DateTime? ExpiryDate { get; set; }
    
    public string? InventoryData { get; set; }
    public string[] BatchNumbers { get; set; } = Array.Empty<string>();
    public string[] SerialNumbers { get; set; } = Array.Empty<string>();
    public byte[]? BarcodeImage { get; set; }
    
    public InventoryStatus Status { get; set; }
    public InventoryType Type { get; set; }
    
    // Navigation property
    public virtual Product Product { get; set; } = null!;
}


