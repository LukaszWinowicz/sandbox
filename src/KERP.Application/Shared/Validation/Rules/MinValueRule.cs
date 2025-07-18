using KERP.Application.Abstractions.Validation;

namespace KERP.Application.Shared.Validation.Rules;

/// <summary>
/// Reguła walidacyjna sprawdzająca, czy wartość jest większa lub równa podanej wartości minimalnej.
/// Działa dla dowolnego typu, który można porównywać (IComparable).
/// </summary>
public class MinValueRule<T> : IValidationRule<T> where T : IComparable<T>
{
    private readonly T _minValue;
    private readonly string _message;

    public MinValueRule(T minValue, string? message = null)
    {
        _minValue = minValue;
        _message = message ?? $"Wartość musi być większa lub równa {_minValue}.";
    }

    public Task<string?> ValidateAsync(T value, CancellationToken cancellationToken = default)
    {
        if (value.CompareTo(_minValue) < 0)
        {
            return Task.FromResult<string?>(_message);
        }

        return Task.FromResult<string?>(null);
    }
}