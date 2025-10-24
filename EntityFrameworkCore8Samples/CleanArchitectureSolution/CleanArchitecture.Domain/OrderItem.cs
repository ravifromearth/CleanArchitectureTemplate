using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityFrameworkCore8Samples.Domain.Entities;

public class OrderItem : BaseEntity
{
    [Required]
    public Guid OrderId { get; set; }
    
    [Required]
    public Guid ProductId { get; set; }
    
    [MaxLength(200)]
    public string ProductName { get; set; } = string.Empty;
    
    [MaxLength(1000)]
    public string? Description { get; set; }
    
    public int Quantity { get; set; }
    
    [Column(TypeName = "decimal(18,4)")]
    public decimal UnitPrice { get; set; }
    
    [Column(TypeName = "decimal(18,4)")]
    public decimal TotalPrice { get; set; }
    
    public string? ProductData { get; set; }
    public string[] Attributes { get; set; } = Array.Empty<string>();
    public string[] Categories { get; set; } = Array.Empty<string>();
    public byte[]? ProductImage { get; set; }
    
    public DateTime AddedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
    
    // Navigation properties
    public virtual Order Order { get; set; } = null!;
    public virtual Product Product { get; set; } = null!;
}


