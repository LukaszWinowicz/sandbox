using MassUpdate.Core.DTOs;
using MassUpdate.Core.Interfaces;
using MassUpdate.Core.Interfaces.Repositories;
using MassUpdate.Core.Validators.Orchestrators;

namespace MassUpdate.Core.Factories;

public class ValidatorFactory
{
    // Fabryka zależy od wszystkich repozytoriów, których mogą potrzebować jej produkty
    private readonly IPurchaseOrderValidationRepository _poValidationRepository;
    // W przyszłości: private readonly IProductionOrderValidationRepository _prodValidationRepo;

    public ValidatorFactory(IPurchaseOrderValidationRepository poValidationRepository)
    {
        _poValidationRepository = poValidationRepository;
    }

    public IEntityValidator<T> Create<T>() where T : MassUpdateDto
    {
        if (typeof(T) == typeof(UpdateReceiptDateDto))
        {
            // Teraz wywołanie jest poprawne - przekazujemy jeden wymagany argument
            return (IEntityValidator<T>)new UpdateReceiptDateValidationStrategy(_poValidationRepository);
        }

        // ... inne warunki w przyszłości ...

        throw new NotSupportedException($"No validator registered for type {typeof(T).Name}");
    }
}
