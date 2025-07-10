using KERP.Application.Interfaces;
using KERP.Application.MassUpdate.PurchaseOrder.Commands;
using KERP.Application.MassUpdate.PurchaseOrder.Validation.Rules;
using KERP.Domain.Interfaces.MassUpdate.PurchaseOrder;

namespace KERP.Application.MassUpdate.PurchaseOrder.Validation;

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
        // Używamy publicznej metody Add() z buildera
        return builder.Add(new OrderExistenceValidator(repository, userService));
    }

    public static ValidationPipelineBuilder<PurchaseOrderUpdateDto> WithCombinationCheck(
        this ValidationPipelineBuilder<PurchaseOrderUpdateDto> builder,
        IPurchaseOrderRepository repository,
        ICurrentUserService userService)
    {
        return builder.Add(new CombinationValidator(repository, userService));
    }
}