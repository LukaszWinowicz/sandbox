using KERP.Application.MassUpdate.PurchaseOrder.Commands;

namespace KERP.Application.MassUpdate.PurchaseOrder.Validators;

public interface IReceiptDateUpdateValidator
{
    Task<List<string>> ValidateAsync(PurchaseOrderUpdateDto dto);
}
