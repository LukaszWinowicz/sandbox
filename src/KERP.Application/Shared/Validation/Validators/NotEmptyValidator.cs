using System.Linq.Expressions;

namespace KERP.Application.Shared.Validation.Validators;

public class NotEmptyValidator<T> : AbstractValidationHandler<T> where T : class
{
    private readonly Func<T, string?> _valueProvider;
    private readonly string _fieldName;

    public NotEmptyValidator(Expression<Func<T, string?>> propertyExpression)
    {
        // Używamy Expression<Func<>> aby automatycznie uzyskać nazwę pola
        _valueProvider = propertyExpression.Compile();
        _fieldName = (propertyExpression.Body as MemberExpression)?.Member.Name ?? "Field";
    }

    protected override Task HandleValidation(ValidationRequest<T> request)
    {
        var value = _valueProvider(request.DtoToValidate);
        if (string.IsNullOrWhiteSpace(value))
        {
            request.Errors.Add($"{_fieldName} cannot be empty.");
        }
        return Task.CompletedTask;
    }
}