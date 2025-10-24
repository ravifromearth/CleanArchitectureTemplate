namespace CleanArchitecture.Application.Interfaces;

/// <summary>
/// Service interface for seeding test data
/// Follows Interface Segregation Principle
/// </summary>
public interface IDataSeederService
{
    Task SeedComplexDataAsync(CancellationToken cancellationToken = default);
    Task<(Guid UserId, Guid ProductId)> SeedUserAndProductAsync(CancellationToken cancellationToken = default);
}


