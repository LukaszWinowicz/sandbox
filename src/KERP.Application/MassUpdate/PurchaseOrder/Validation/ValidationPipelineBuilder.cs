using KERP.Application.Shared.Validation;
using KERP.Application.Shared.Validation.Validators;
using System.Linq.Expressions;

namespace KERP.Application.MassUpdate.PurchaseOrder.Validation;

/// <summary>
/// Finałowa, płynna wersja budowniczego do tworzenia potoków walidacyjnych.
/// </summary>
public class ValidationPipelineBuilder<T> where T : class
{
    private IValidationHandler<T>? _head;
    private IValidationHandler<T>? _current;

    // Publiczna metoda Add jest potrzebna dla metod rozszerzających
    public ValidationPipelineBuilder<T> Add(IValidationHandler<T> handler)
    {
        if (_head == null)
        {
            _head = handler;
            _current = handler;
        }
        else
        {
            _current?.SetNext(handler);
            _current = handler;
        }
        return this;
    }

    // --- Generyczne, reużywalne metody walidacyjne ---

    public ValidationPipelineBuilder<T> WithNotEmpty(Expression<Func<T, string?>> property)
    {
        return Add(new NotEmptyValidator<T>(property));
    }

    public ValidationPipelineBuilder<T> WithStringLength(Expression<Func<T, string?>> property, int exactLength)
    {
        return Add(new StringLengthValidator<T>(property, exactLength));
    }

    public ValidationPipelineBuilder<T> WithMinValue<TProperty>(Expression<Func<T, TProperty>> property, TProperty minValue) where TProperty : IComparable<TProperty>
    {
        return Add(new MinValueValidator<T, TProperty>(property, minValue));
    }

    public ValidationPipelineBuilder<T> WithFutureDate(Expression<Func<T, DateTime>> property)
    {
        var fieldName = (property.Body as MemberExpression)?.Member.Name ?? "Date";
        var compiledProperty = property.Compile();
        return Add(new FutureDateValidator<T>(compiledProperty, fieldName));
    }

    public IValidationHandler<T> Build()
    {
        if (_head == null) throw new InvalidOperationException("Cannot build an empty validation pipeline.");
        return _head;
    }
}
