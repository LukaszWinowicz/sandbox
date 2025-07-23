namespace KERP.Application.Common.Validation.Rules;

/// <summary>
/// Walidator pola który łączy wiele reguł w łańcuch z mechanizmem fail-fast.
/// Zatrzymuje walidację na pierwszym błędzie.
/// </summary>
/// <typeparam name="T">Typ walidowanej wartości</typeparam>
public class FieldValidator<T>
{
    private readonly List<IValidationRule<T>> _rules = new();
    private readonly List<IAsyncValidationRule<T>> _asyncRules = new();

    /// <summary>
    /// Nazwa pola które jest walidowane (używane w komunikatach błędów).
    /// </summary>
    public string FieldName { get; }

    /// <summary>
    /// Tworzy nowy walidator dla pola.
    /// </summary>
    /// <param name="fieldName">Nazwa pola</param>
    public FieldValidator(string fieldName)
    {
        FieldName = fieldName ?? throw new ArgumentNullException(nameof(fieldName));
    }

    /// <summary>
    /// Dodaje synchroniczną regułę do łańcucha walidacji.
    /// </summary>
    /// <param name="rule">Reguła do dodania</param>
    /// <returns>Bieżąca instancja dla fluent API</returns>
    public FieldValidator<T> AddRule(IValidationRule<T> rule)
    {
        _rules.Add(rule ?? throw new ArgumentNullException(nameof(rule)));
        return this;
    }

    /// <summary>
    /// Dodaje asynchroniczną regułę do łańcucha walidacji.
    /// </summary>
    /// <param name="rule">Reguła do dodania</param>
    /// <returns>Bieżąca instancja dla fluent API</returns>
    public FieldValidator<T> AddAsyncRule(IAsyncValidationRule<T> rule)
    {
        _asyncRules.Add(rule ?? throw new ArgumentNullException(nameof(rule)));
        return this;
    }

    /// <summary>
    /// Wykonuje synchroniczną walidację wartości.
    /// Zatrzymuje się na pierwszym błędzie (fail-fast).
    /// </summary>
    /// <param name="value">Wartość do walidacji</param>
    /// <returns>Lista błędów (pusta jeśli walidacja przeszła)</returns>
    public List<string> Validate(T value)
    {
        // Najpierw sprawdzamy synchroniczne reguły
        foreach (var rule in _rules)
        {
            if (!rule.Validate(value))
            {
                return new List<string> { rule.ErrorMessage };
            }
        }

        // Jeśli są async reguły, zwracamy błąd
        if (_asyncRules.Any())
        {
            return new List<string> {
                $"Pole {FieldName} wymaga asynchronicznej walidacji. Użyj ValidateAsync."
            };
        }

        return new List<string>();
    }

    /// <summary>
    /// Wykonuje asynchroniczną walidację wartości.
    /// Najpierw wykonuje wszystkie synchroniczne reguły, potem asynchroniczne.
    /// Zatrzymuje się na pierwszym błędzie (fail-fast).
    /// </summary>
    /// <param name="value">Wartość do walidacji</param>
    /// <param name="cancellationToken">Token anulowania</param>
    /// <returns>Lista błędów (pusta jeśli walidacja przeszła)</returns>
    public async Task<List<string>> ValidateAsync(T value, CancellationToken cancellationToken = default)
    {
        // Najpierw synchroniczne reguły
        foreach (var rule in _rules)
        {
            if (!rule.Validate(value))
            {
                return new List<string> { rule.ErrorMessage };
            }
        }

        // Potem asynchroniczne
        foreach (var rule in _asyncRules)
        {
            if (!await rule.ValidateAsync(value, cancellationToken))
            {
                return new List<string> { rule.ErrorMessage };
            }
        }

        return new List<string>();
    }
}