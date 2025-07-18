using KERP.Application.Abstractions.Queries.Repositories;
using KERP.Application.Abstractions.Validation;
using KERP.Application.Shared.Validation.Rules;

namespace KERP.Application.Features.MassUpdate.PurchaseOrder.Validation;

public static class PurchaseOrderValidationExtensions
{
    /// <summary>
    /// Dodaje regułę sprawdzającą, czy zamówienie istnieje w repozytorium.
    /// </summary>
    public static ValidationBuilder<string> MustExistInRepo(this ValidationBuilder<string> builder, IExternalPurchaseOrderRepository repository)
    {
        return builder.AddRule(new PurchaseOrderExistsRule(repository));
    }
}
