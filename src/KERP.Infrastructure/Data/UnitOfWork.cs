using KERP.Domain.Abstractions;

namespace KERP.Infrastructure.Data;

/// <summary>
/// Implementacja Unit of Work na bazie EF Core.
/// Operuje na <see cref="AppDbContext"/>.
/// </summary>
public sealed class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _dbContext;

    public UnitOfWork(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}