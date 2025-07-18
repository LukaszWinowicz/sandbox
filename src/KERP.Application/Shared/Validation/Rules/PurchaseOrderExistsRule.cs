using KERP.Application.Abstractions.Queries.Repositories;
using KERP.Application.Abstractions.Validation;

namespace KERP.Application.Shared.Validation.Rules;

/// <summary>
/// Reguła walidacyjna sprawdzająca, czy zamówienie o danym numerze istnieje w zewnętrznym repozytorium.
/// </summary>
public class PurchaseOrderExistsRule : IValidationRule<string>
{
    private readonly IExternalPurchaseOrderRepository _repository;

    public PurchaseOrderExistsRule(IExternalPurchaseOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<string?> ValidateAsync(string value, CancellationToken cancellationToken = default)
    {
        // Pusta wartość jest obsługiwana przez NotEmptyRule, tutaj ją ignorujemy.
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        if (!await _repository.ExistsAsync(value, cancellationToken))
        {
            return $"Zamówienie o numerze '{value}' nie istnieje.";
        }

        return null;
    }
}