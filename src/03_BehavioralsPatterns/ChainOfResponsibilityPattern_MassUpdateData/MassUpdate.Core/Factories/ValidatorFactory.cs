using MassUpdate.Core.DTOs;
using MassUpdate.Core.Interfaces;
using MassUpdate.Core.Validators.Orchestrators;

namespace MassUpdate.Core.Factories;

public class ValidatorFactory
{
    private readonly IOrderDataService _orderDataService;
    // W przyszłości dodamy tu inne serwisy, np. IProductionOrderService

    public ValidatorFactory(IOrderDataService orderDataService)
    {
        _orderDataService = orderDataService;
    }

    // Generyczna metoda tworząca walidator
    public IEntityValidator<T> Create<T>() where T : MassUpdateDto
    {
        // Logika "decyzyjna" - serce fabryki
        if (typeof(T) == typeof(MassUpdatePurchaseOrderDto))
        {
            // Rzutowanie ...
            return (IEntityValidator<T>)new PurchaseOrderMassUpdateValidator(_orderDataService);
        }
        // W przyszłości dodamy kolejne warunki
        // if (typeof(T) == typeof(MassUpdateProductionOrderDto))
        // {
        //     return (IEntityValidator<T>)new ProductionOrderMassUpdateValidator(new FakeProductionOrderService());
        // }

        // Jeśli nie znamy typu, rzucamy wyjątkiem
        throw new NotSupportedException($"No validator registered for type {typeof(T).Name}");
    }
}
