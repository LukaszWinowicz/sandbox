using KERP.Application.Validation;
using KERP.Application.Validation.Chain;

namespace KERP.Application.Features.MassUpdates.PurchaseOrder.UpdateReceiptDate;

public class RequestPurchaseOrderReceiptDateChangeCommandValidator 
    : IValidator<RequestPurchaseOrderReceiptDateChangeCommand>
{
    private readonly IServiceProvider _services;

    public RequestPurchaseOrderReceiptDateChangeCommandValidator(IServiceProvider services)
    {
        _services = services;
    }

    public ValidationResult Validate(RequestPurchaseOrderReceiptDateChangeCommand command)
    {
        var validationErrors = new List<ValidationError>();
        int rowNumber = 1;

        // Budujemy szablon walidacji TYLKO RAZ
        var chain = new ValidationChainBuilder<OrderLineDto>()
            .WithNotEmpty(line => line.PurchaseOrder, "PurchaseOrder")
            .Build();

        if (chain is null) return ValidationResult.Success();

        foreach (var line in command.OrderLines)
        {
            var context = new ValidationContext<OrderLineDto>(line, _services);
            chain.HandleAsync(context).GetAwaiter().GetResult();
            
            if (context.Errors.Any())
            {
                validationErrors.AddRange(context.Errors.Select(e => e with { PropertyName = $"Wiersz[{rowNumber}]:{e.PropertyName}" }));
            }
            rowNumber++;
        }

        return new ValidationResult(validationErrors);
    }
}
