using MassUpdateData.Handlers;
using MassUpdateData.Models;

namespace MassUpdateData.Validators.Components;

// TDto - to będzie konkretny typ naszego DTO, np. MassUpdatePurchaseOrderDto
// TService - to będzie typ serwisu danych, np. IOrderDataService
public class CombinationValidator<TDto, TService> : ValidationHandler
    where TDto : MassUpdateDto // Ograniczenie, aby mieć pewność, że pracujemy na naszych DTO
{
    private readonly TService _service;
    private readonly Func<TDto, TService, bool> _combinationCheckFunc;
    private readonly string _errorMessage;

    /// <summary>
    /// Tworzy generyczny walidator kombinacji.
    /// </summary>
    /// <param name="service">Instancja serwisu danych.</param>
    /// <param name="combinationCheckFunc">"Przepis" na walidację. Funkcja, która przyjmuje DTO i serwis, a zwraca bool.</param>
    /// <param name="errorMessage">Komunikat błędu do wyświetlenia.</param>
    public CombinationValidator(TService service, Func<TDto, TService, bool> combinationCheckFunc, string errorMessage)
    {
        _service = service;
        _combinationCheckFunc = combinationCheckFunc;
        _errorMessage = errorMessage;
    }

    public override void Validate(ValidationRequest request)
    {
        // Sprawdzamy, czy DTO jest właściwego typu, którego oczekuje nasz "przepis"
        if (request.Dto is TDto specificDto)
        {
            // Wykonujemy nasz "przepis", podając mu DTO i serwis
            if (!_combinationCheckFunc(specificDto, _service))
            {
                request.ValidationErrors.Add(_errorMessage);
            }
        }

        PassToNext(request);

    }

}
