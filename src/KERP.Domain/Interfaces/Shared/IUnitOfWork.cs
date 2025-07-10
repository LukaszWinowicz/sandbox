namespace KERP.Domain.Interfaces.Shared;

/// <summary>
/// Reprezentuje wzorzec Unit of Work, pozwalając na atomowe zapisywanie zmian.
/// </summary>
public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
