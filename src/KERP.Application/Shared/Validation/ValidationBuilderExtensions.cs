using KERP.Application.Abstractions.Validation;
using KERP.Application.Shared.Validation.Rules;

namespace KERP.Application.Shared.Validation;

/// <summary>
/// Zawiera metody rozszerzające dla klasy ValidationBuilder,
/// które umożliwiają płynne (fluent) i bardziej deklaratywne budowanie łańcucha reguł walidacyjnych.
/// </summary>
public static class ValidationBuilderExtensions
{
    /// <summary>
    /// Dodaje regułę sprawdzającą, czy ciąg znaków nie jest pusty.
    /// </summary>
    public static ValidationBuilder<string> NotEmpty(this ValidationBuilder<string> builder, string message = "Wartość nie może być pusta.")
    {
        return builder.AddRule(new NotEmptyRule(message));
    }

    /// <summary>
    /// Dodaje regułę sprawdzającą, czy ciąg znaków ma dokładnie określoną długość.
    /// </summary>
    public static ValidationBuilder<string> HasExactLength(this ValidationBuilder<string> builder, int exactLength, string? message = null)
    {
        return builder.AddRule(new StringLengthRule(exactLength, message));
    }

    /// <summary>
    /// Dodaje regułę sprawdzającą, czy wartość jest większa lub równa podanej wartości minimalnej.
    /// </summary>
    public static ValidationBuilder<TProperty> IsGreaterThanOrEqualTo<TProperty>(this ValidationBuilder<TProperty> builder, TProperty minValue, string? message = null)
        where TProperty : IComparable<TProperty>
    {
        return builder.AddRule(new MinValueRule<TProperty>(minValue, message));
    }

    /// <summary>
    /// Dodaje regułę sprawdzającą, czy data nie jest w przeszłości.
    /// </summary>
    public static ValidationBuilder<DateTime> IsNotInPast(this ValidationBuilder<DateTime> builder, string message = "Nie można ustawić daty z przeszłości.")
    {
        return builder.AddRule(new DateNotInPastRule(message));
    }
}
