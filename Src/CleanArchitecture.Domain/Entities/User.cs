using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.Domain.Entities;

public class User : BaseEntity
{
    [Required]
    [MaxLength(100)]
    public string Username { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;
    
    [MaxLength(500)]
    public string? Bio { get; set; }
    
    public DateTime? LastLoginAt { get; set; }
    public DateOnly BirthDate { get; set; }
    public TimeOnly PreferredLoginTime { get; set; }
    
    public string? Metadata { get; set; }
    public string[] Tags { get; set; } = Array.Empty<string>();
    public int[] FavoriteNumbers { get; set; } = Array.Empty<int>();
    
    [Column(TypeName = "decimal(18,6)")]
    public decimal CreditScore { get; set; }
    
    [Column(TypeName = "money")]
    public decimal Balance { get; set; }
    
    public byte[]? ProfilePicture { get; set; }
    
    public UserStatus Status { get; set; }
    public UserRole Role { get; set; }
    
    // Navigation properties
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    public virtual ICollection<UserSession> Sessions { get; set; } = new List<UserSession>();
    public virtual UserProfile? Profile { get; set; }
}


