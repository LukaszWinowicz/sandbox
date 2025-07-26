using KERP.Application.Validation.Chain.Handlers;

namespace KERP.Application.Validation.Chain;

/// <summary>
/// Umożliwia płynne budowanie łańucha walidacji dla dowlonego typu.
/// </summary>
/// <typeparam name="T"></typeparam>
public class ValidationChainBuilder<T>
{
    private ValidationHandler<T>? _head;
    private ValidationHandler<T>? _tail;

    private void AddHandler(ValidationHandler<T> handler)
    {
        if (_head == null)
        {
            _head = handler;
            _tail = handler;
        }
        else
        {
            _tail!.SetNext(handler);
            _tail = handler;
        }
    }

    public ValidationChainBuilder<T> WithNotEmpty(Func<T, string> valueProvider, string fieldName)
    {
        AddHandler(new NotEmptyValidator<T>(valueProvider, fieldName));
        return this;
    }

    // Tutaj można dodawać kolejne metody ...

    /// <summary>
    /// Buduje łańcuch i zwraca jego pierwsze ogniwo.
    /// </summary>
    public ValidationHandler<T> Build()
    {
        if (_head == null)
        {
            throw new InvalidOperationException("Łańcuch walidacji nie zawiera żadnych handlerów.");
        }
        return _head;
    }

}
