using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CleanArchitecture.Domain.Enums;
using CleanArchitecture.Domain.ValueObjects;

namespace CleanArchitecture.Domain.Entities;

public class Order : BaseEntity
{
    [Required]
    public Guid UserId { get; set; }
    
    [MaxLength(50)]
    public string OrderNumber { get; set; } = string.Empty;
    
    public DateTime? ShippedAt { get; set; }
    public DateTime? DeliveredAt { get; set; }
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal SubTotal { get; set; }
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal TaxAmount { get; set; }
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal ShippingCost { get; set; }
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal Total { get; set; }
    
    public string? OrderData { get; set; }
    public string[] Tags { get; set; } = Array.Empty<string>();
    
    public OrderStatus Status { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    
    public Address? ShippingAddress { get; set; }
    public Address? BillingAddress { get; set; }
    
    public byte[]? ReceiptPdf { get; set; }
    
    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public virtual ICollection<OrderStatusHistory> StatusHistory { get; set; } = new List<OrderStatusHistory>();
}


