namespace EntityFrameworkCore8Samples.Domain.Entities.ViewModels;

/// <summary>
/// Represents the vw_UserProfileSummary view
/// </summary>
public class UserProfileSummaryView
{
    public Guid UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public int CreditScore { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public int TotalOrders { get; set; }  // COUNT(*) cast to INT in SQL Server
    public decimal? TotalSpent { get; set; }
}

