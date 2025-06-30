using MassUpdate.Core.DTOs;
using MassUpdate.Core.Factories;
using MassUpdate.Core.Interfaces;

namespace MassUpdate.Core.Services;

public class MassUpdateValidationService : IMassUpdateValidationService
{
    private readonly ValidatorFactory _validatorFactory;

    // Wstrzykujemy naszą fabrykę
    public MassUpdateValidationService(ValidatorFactory validatorFactory)
    {
        _validatorFactory = validatorFactory;
    }

    public List<RowValidationResult> Validate<T>(List<T> dtoList) where T : MassUpdateDto
    {
        // Używamy fabryki do stworzenia odpowiedniego walidatora
        var validator = _validatorFactory.Create<T>();

        var allResults = new List<RowValidationResult>();
        int rowNumber = 1;

        foreach (var dto in dtoList)
        {
            List<string> errors = validator.Validate(dto);
            if (errors.Any())
            {
                allResults.Add(new RowValidationResult(rowNumber, errors));
            }
            rowNumber++;
        }
        return allResults;
    }
}