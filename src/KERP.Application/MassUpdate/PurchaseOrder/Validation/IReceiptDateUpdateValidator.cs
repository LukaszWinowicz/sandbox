using KERP.Application.MassUpdate.PurchaseOrder.Commands;

namespace KERP.Application.MassUpdate.PurchaseOrder.Validation;

public interface IReceiptDateUpdateValidator
{
    Task<List<string>> ValidateAsync(PurchaseOrderUpdateDto dto);
}
