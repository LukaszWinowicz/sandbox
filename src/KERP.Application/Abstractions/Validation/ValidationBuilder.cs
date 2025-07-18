namespace KERP.Application.Abstractions.Validation;

/// <summary>
/// Budowniczy do tworzenia łańcucha reguł walidacyjnych dla określonej właściwości.
/// Umożliwia płynne (fluent) dodawanie kolejnych reguł.
/// </summary>
/// <typeparam name="TProperty">Typ właściwości, dla której budowany jest łańcuch walidacji.</typeparam>
public class ValidationBuilder<TProperty>
{
    private readonly List<IValidationRule<TProperty>> _rules = new();

    /// <summary>
    /// Dodaje nową regułę walidacyjną do łańcucha.
    /// </summary>
    /// <param name="rule">Instancja reguły do dodania.</param>
    /// <returns>Instancja budowniczego w celu umożliwienia dalszego łączenia wywołań.</returns>
    public ValidationBuilder<TProperty> AddRule(IValidationRule<TProperty> rule)
    {
        _rules.Add(rule);
        return this;
    }

    /// <summary>
    /// Kompiluje dodane reguły w jedną funkcję walidacyjną.
    /// Funkcja ta wykonuje wszystkie reguły sekwencyjnie i zwraca pierwszy napotkany błąd.
    /// </summary>
    /// <returns>Funkcja (delegate) walidacyjna, która przyjmuje wartość i CancellationToken, a zwraca Task<string?>.</returns>
    public Func<TProperty, CancellationToken, Task<string?>> Build()
    {
        return async (value, cancellationToken) =>
        {
            foreach (var rule in _rules)
            {
                var error = await rule.ValidateAsync(value, cancellationToken);
                if (!string.IsNullOrEmpty(error))
                {
                    return error; // Zwróć pierwszy napotkany błąd
                }
            }
            return null; // Wszystkie reguły przeszły pomyślnie
        };
    }
}