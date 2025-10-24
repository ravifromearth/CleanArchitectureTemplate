using System.ComponentModel.DataAnnotations;
using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.Domain.Entities;

public class OrderStatusHistory : BaseEntity
{
    [Required]
    public Guid OrderId { get; set; }
    
    [MaxLength(100)]
    public string? ChangedBy { get; set; }
    
    public OrderStatus OldStatus { get; set; }
    public OrderStatus NewStatus { get; set; }
    
    public DateTime ChangedAt { get; set; }
    
    [MaxLength(500)]
    public string? Reason { get; set; }
    
    [MaxLength(1000)]
    public string? Notes { get; set; }
    
    public string? ChangeData { get; set; }
    public string[] Tags { get; set; } = Array.Empty<string>();
    public byte[]? Document { get; set; }
    
    // Navigation property
    public virtual Order Order { get; set; } = null!;
}


