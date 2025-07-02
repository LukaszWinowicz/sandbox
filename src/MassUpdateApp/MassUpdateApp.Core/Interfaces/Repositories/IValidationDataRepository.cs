namespace MassUpdateApp.Core.Interfaces.Repositories;

/// <summary>
/// Defines a contract for a repository that provides read-only data from validation purposes.
/// </summary>
public interface IValidationDataRepository
{
    /// <summary>
    /// Checks if a Purchase Order with the given number exists in the validation data source.
    /// </summary>
    /// <param name="orderNumber">The Purchase Order number to check.</param>
    /// <returns>True if the order exists, otherwise false.</returns>
    Task<bool> PurchaseOrderExistsAsync(string orderNumber);

    /// <summary>
    /// Check if a specific combination of Purchase Order, Line and Sequence exists.
    /// </summary>
    /// <param name="orderNumber">The Purchase Order number.</param>
    /// <param name="lineNumber">The line number.</param>
    /// <param name="sequence">The sequence number.</param>
    /// <returns>True if the combination exists, otherwise false.</returns>
    Task<bool> CombinationExistsAsync(string orderNumber, int lineNumber, int sequence);

}
