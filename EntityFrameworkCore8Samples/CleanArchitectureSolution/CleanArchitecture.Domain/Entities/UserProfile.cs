using System.ComponentModel.DataAnnotations;
using EntityFrameworkCore8Samples.Domain.ValueObjects;

namespace EntityFrameworkCore8Samples.Domain.Entities;

public class UserProfile : BaseEntity
{
    [Required]
    public Guid UserId { get; set; }
    
    [MaxLength(100)]
    public string? FirstName { get; set; }
    
    [MaxLength(100)]
    public string? LastName { get; set; }
    
    [MaxLength(20)]
    public string? PhoneNumber { get; set; }
    
    public Address? HomeAddress { get; set; }
    public Address? WorkAddress { get; set; }
    
    public string? Preferences { get; set; }
    public string[] Skills { get; set; } = Array.Empty<string>();
    public string[] Languages { get; set; } = Array.Empty<string>();
    
    public DateOnly? Anniversary { get; set; }
    public byte[]? Avatar { get; set; }
    
    // Navigation property
    public virtual User User { get; set; } = null!;
}


