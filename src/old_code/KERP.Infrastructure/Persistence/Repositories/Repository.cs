using KERP.Domain.Interfaces.Repositories;
namespace KERP.Infrastructure.Persistence.Repositories;

/// <summary>
/// A generic repository implementation for common database write operations.
/// </summary>
public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    protected readonly AppDbContext _context;

    public Repository(AppDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task AddRangeAsync(IEnumerable<TEntity> entities)
    {
        await _context.Set<TEntity>().AddRangeAsync(entities);
    }

    /// <inheritdoc />
    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
