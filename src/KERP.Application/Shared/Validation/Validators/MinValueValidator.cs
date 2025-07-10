using System.Linq.Expressions;

namespace KERP.Application.Shared.Validation.Validators;

public class MinValueValidator<T, TProperty> : AbstractValidationHandler<T>
    where T : class
    where TProperty : IComparable<TProperty>
{
    private readonly Func<T, TProperty> _valueProvider;
    private readonly TProperty _minValue;
    private readonly string _fieldName;

    public MinValueValidator(Expression<Func<T, TProperty>> propertyExpression, TProperty minValue)
    {
        _valueProvider = propertyExpression.Compile();
        _fieldName = (propertyExpression.Body as MemberExpression)?.Member.Name ?? "Field";
        _minValue = minValue;
    }

    protected override Task HandleValidation(ValidationRequest<T> request)
    {
        var value = _valueProvider(request.DtoToValidate);
        if (value.CompareTo(_minValue) < 0)
        {
            request.Errors.Add($"{_fieldName} must be greater than or equal to {_minValue}.");
        }
        return Task.CompletedTask;
    }
}