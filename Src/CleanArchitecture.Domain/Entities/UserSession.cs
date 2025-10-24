using System.ComponentModel.DataAnnotations;
using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.Domain.Entities;

public class UserSession : BaseEntity
{
    [Required]
    public Guid UserId { get; set; }
    
    [MaxLength(500)]
    public string SessionToken { get; set; } = string.Empty;
    
    [MaxLength(45)]
    public string? IpAddress { get; set; }
    
    [MaxLength(500)]
    public string? UserAgent { get; set; }
    
    public DateTime? ExpiresAt { get; set; }
    public DateTime? LastActivityAt { get; set; }
    
    public string? SessionData { get; set; }
    public string[] Permissions { get; set; } = Array.Empty<string>();
    public byte[]? EncryptionKey { get; set; }
    
    public SessionStatus Status { get; set; }
    public SessionType Type { get; set; }
    
    // Navigation property
    public virtual User User { get; set; } = null!;
}


