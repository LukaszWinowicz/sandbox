using KERP.Application.MassUpdate.PurchaseOrder.Commands;
using KERP.Application.Shared.Validation;

namespace KERP.Application.MassUpdate.PurchaseOrder.Validators;

/// <summary>
/// Definiuje kontrakt dla fabryki, która tworzy potoki walidacyjne
/// dla przypadków użycia w module Purchase Order.
/// </summary>
public interface IValidationPipelineFactory
{
    /// <summary>
    /// Tworzy i zwraca gotowy potok walidacyjny dla DTO aktualizacji daty odbioru.
    /// </summary>
    IValidationHandler<PurchaseOrderUpdateDto> CreateForReceiptDateUpdate();
}