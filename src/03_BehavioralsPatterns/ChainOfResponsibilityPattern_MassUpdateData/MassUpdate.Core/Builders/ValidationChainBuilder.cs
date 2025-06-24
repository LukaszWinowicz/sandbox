using MassUpdate.Core.DTOs;
using MassUpdate.Core.Handlers;
using MassUpdate.Core.Validators.Components;

namespace MassUpdate.Core.Builders;

public class ValidationChainBuilder
{
    private IValidationHandler? _head; // Pierwsze ogniwo w budowanym łańcuchu
    private IValidationHandler? _tail; // Ostatnie ogniwo, aby efektywnie dodawać kolejne

    // Prywatna metoda pomocnicza do dodawania nowego ogniwa na końcu łańcucha
    private void AddHandler(IValidationHandler handler)
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

    // --- Metody Budujące (zwracają 'this', aby można je było łączyć) ---
    public ValidationChainBuilder WithNotEmptyCheck(Func<MassUpdateDto, string?> valueProvider, string fieldName)
    {
        AddHandler(new NotEmptyValidator(valueProvider, fieldName));
        return this;
    }

    public ValidationChainBuilder WithStringLengthCheck(Func<MassUpdateDto, string?> valueProvider, int length, string fieldName)
    {
        AddHandler(new StringLengthValidator(valueProvider, length, fieldName));
        return this;
    }

    public ValidationChainBuilder WithExistenceCheck<T>(Func<MassUpdateDto, T> valueProvider, Func<T, bool> existenceCheckFunc, string fieldName)
    {
        AddHandler(new ExistenceValidator<T>(valueProvider, existenceCheckFunc, fieldName));
        return this;
    }

    public ValidationChainBuilder WithFutureDateCheck(Func<MassUpdateDto, DateTime> dateProvider, string fieldName)
    {
        AddHandler(new FutureDateValidator(dateProvider, fieldName));
        return this;
    }

    public ValidationChainBuilder WithMinValueCheck<T>(Func<MassUpdateDto, T> valueProvider, T minValue, string fieldName) where T : IComparable<T>
    {
        // 1. Tworzysz nową instancję generycznego walidatora MinValueValidator<T>,
        //    przekazując mu wszystkie potrzebne parametry, które otrzymała ta metoda.
        var newHandler = new MinValueValidator<T>(valueProvider, minValue, fieldName);

        // 2. Dodajesz ten nowo stworzony handler na koniec aktualnie budowanego łańcucha.
        AddHandler(newHandler);

        // 3. Zwracasz 'this' (czyli samego siebie - Budowniczego),
        //    co pozwala na dalsze łączenie metod w płynny interfejs (np. .WithMinValueCheck(...).WithAnotherCheck(...)).
        return this;
    }

    // Metoda finalizująca, która zwraca gotowy produkt - głowę łańcucha
    public IValidationHandler Build()
    {
        if (_head == null)
        {
            throw new InvalidOperationException("Cannot build an empty validation chain.");
        }
        return _head;
    }
}
