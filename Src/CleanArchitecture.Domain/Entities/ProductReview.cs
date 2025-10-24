using System.ComponentModel.DataAnnotations;
using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.Domain.Entities;

public class ProductReview : BaseEntity
{
    [Required]
    public Guid ProductId { get; set; }
    
    [Required]
    public Guid UserId { get; set; }
    
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;
    
    [MaxLength(2000)]
    public string? Comment { get; set; }
    
    public int Rating { get; set; }
    
    public string? ReviewData { get; set; }
    public string[] Tags { get; set; } = Array.Empty<string>();
    public byte[]? ReviewImage { get; set; }
    
    public ReviewStatus Status { get; set; }
    public ReviewType Type { get; set; }
    
    // Navigation properties
    public virtual Product Product { get; set; } = null!;
    public virtual User User { get; set; } = null!;
}


