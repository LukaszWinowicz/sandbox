using MassUpdateApp.Core.Entities;

namespace MassUpdateApp.Core.Interfaces.Repositories;

/// <summary>
/// Defines a contract for a repository that handles write operations (Commands).
/// </summary>
public interface ICommandRepository
{
    /// <summary>
    /// Adds a collection of new Purchase Order update requests to the database.
    /// </summary>
    /// <param name="entities">A collection of fully validated and mapped entities to be saved.</param>
    Task AddPurchaseOrderUpdateRequestsAsync(IEnumerable<PurchaseOrderUpdateRequestEntity> entities);

    /// <summary>
    /// Commits all pending changes to the underlying database.
    /// </summary>
    Task<int> SaveChangesAsync();
}