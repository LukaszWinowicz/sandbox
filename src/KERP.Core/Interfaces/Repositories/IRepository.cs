namespace KERP.Core.Interfaces.Repositories;

/// <summary>
/// A generic repository interface for common database operations.
/// </summary>
/// <typeparam name="TEntity">The type of the entity this repository works with.</typeparam>
public interface IRepository<TEntity> where TEntity : class
{
    /// <summary>
    /// Adds a collection of entities to the data store.
    /// </summary>
    Task AddRangeAsync(IEnumerable<TEntity> entities);

    /// <summary>
    /// Commits all pending changes to the database.
    /// </summary>
    Task<int> SaveChangesAsync();

    // W przyszłości możemy tu dodać inne generyczne metody, np.
    // Task<TEntity?> GetByIdAsync(int id);
    // void Delete(TEntity entity);
}
