namespace KERP.Core.Interfaces.Repositories;

/// <summary>
/// Defines a contract for a repository that provides read-only data
/// specifically for Purchase Order validation purposes.
/// </summary>
public interface IPurchaseOrderValidationRepository
{
    /// <summary>
    /// Checks if a Purchase Order with the given number exists.
    /// </summary>
    /// <param name="orderNumber">The Purchase Order number to check.</param>
    /// <returns>True if the order exists; otherwise, false.</returns>
    Task<bool> OrderExistsAsync(string orderNumber);

    /// <summary>
    /// Checks if a specific combination of a Purchase Order, Line, and Sequence exists.
    /// </summary>
    /// <param name="orderNumber">The Purchase Order number.</param>
    /// <param name="lineNumber">The line number.</param>
    /// <param name="sequence">The sequence number.</param>
    /// <returns>True if the combination exists; otherwise, false.</returns>
    Task<bool> CombinationExistsAsync(string orderNumber, int lineNumber, int sequence);
}