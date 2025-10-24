using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CleanArchitecture.Domain.Enums;
using CleanArchitecture.Domain.ValueObjects;

namespace CleanArchitecture.Domain.Entities;

public class Product : BaseEntity
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(1000)]
    public string? Description { get; set; }
    
    [MaxLength(100)]
    public string? SKU { get; set; }
    
    [MaxLength(50)]
    public string? Barcode { get; set; }
    
    [Column(TypeName = "decimal(18,4)")]
    public decimal Price { get; set; }
    
    [Column(TypeName = "decimal(18,4)")]
    public decimal? SalePrice { get; set; }
    
    [Column(TypeName = "decimal(18,4)")]
    public decimal? Cost { get; set; }
    
    public string? Specifications { get; set; }
    public string[] Tags { get; set; } = Array.Empty<string>();
    public string[] Categories { get; set; } = Array.Empty<string>();
    public string[] Images { get; set; } = Array.Empty<string>();
    
    public ProductDimensions? Dimensions { get; set; }
    public ProductWeight? Weight { get; set; }
    
    public byte[]? Thumbnail { get; set; }
    public byte[]? ManualPdf { get; set; }
    
    public DateTime? DiscontinuedAt { get; set; }
    
    public ProductStatus Status { get; set; }
    public ProductType Type { get; set; }
    
    // Navigation properties
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public virtual ICollection<ProductReview> Reviews { get; set; } = new List<ProductReview>();
    public virtual ICollection<ProductInventory> Inventory { get; set; } = new List<ProductInventory>();
}


