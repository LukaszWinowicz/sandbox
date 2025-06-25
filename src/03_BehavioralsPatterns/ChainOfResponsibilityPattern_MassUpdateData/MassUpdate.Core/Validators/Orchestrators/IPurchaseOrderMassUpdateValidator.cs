using MassUpdate.Core.DTOs;

namespace MassUpdate.Core.Validators.Orchestrators;
public interface IPurchaseOrderMassUpdateValidator
{
    List<string> Validate(MassUpdatePurchaseOrderDto dto);
}
