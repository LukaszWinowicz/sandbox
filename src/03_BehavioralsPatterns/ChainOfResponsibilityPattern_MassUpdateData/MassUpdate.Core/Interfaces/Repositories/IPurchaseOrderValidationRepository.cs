namespace MassUpdate.Core.Interfaces.Repositories;

public interface IPurchaseOrderValidationRepository
{
    /// <summary>
    /// Sprawdza, czy zlecenie o danym numerze istnieje.
    /// </summary>
    Task<bool> OrderExistsAsync(string orderNumber);

    /// <summary>
    /// Sprawdza, czy kombinacja zlecenia, linii i sekwencji istnieje.
    /// </summary>
    Task<bool> CombinationExistsAsync(string orderNumber, int lineNumber, int sequence);
}
