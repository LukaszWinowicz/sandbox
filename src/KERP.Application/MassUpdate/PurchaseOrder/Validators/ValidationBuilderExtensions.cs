using KERP.Application.Interfaces;
using KERP.Application.MassUpdate.PurchaseOrder.Commands;
using KERP.Application.Shared.Validation;
using KERP.Domain.Interfaces.MassUpdate.PurchaseOrder;
namespace KERP.Application.MassUpdate.PurchaseOrder.Validators;

/// <summary>
/// Metody rozszerzające dla ValidationPipelineBuilder, specyficzne dla PurchaseOrder.
/// </summary>
public static class ValidationBuilderExtensions
{
    public static ValidationPipelineBuilder<PurchaseOrderUpdateDto> WithOrderExistenceCheck(
        this ValidationPipelineBuilder<PurchaseOrderUpdateDto> builder,
        IPurchaseOrderRepository repository,
        ICurrentUserService userService)
    {
        builder.Add(new OrderExistenceValidator(repository, userService));
        return builder;
    }

    public static ValidationPipelineBuilder<PurchaseOrderUpdateDto> WithCombinationCheck(
        this ValidationPipelineBuilder<PurchaseOrderUpdateDto> builder,
        IPurchaseOrderRepository repository,
        ICurrentUserService userService)
    {
        builder.Add(new CombinationValidator(repository, userService));
        return builder;
    }
}