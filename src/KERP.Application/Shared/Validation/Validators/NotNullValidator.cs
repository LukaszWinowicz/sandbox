using System.Linq.Expressions;

namespace KERP.Application.Shared.Validation.Validators;

public class NotNullValidator<T, TProperty> : AbstractValidationHandler<T> where T : class
{
    private readonly Func<T, TProperty?> _valueProvider;
    private readonly string _fieldName;

    public NotNullValidator(Expression<Func<T, TProperty?>> propertyExpression)
    {
        _valueProvider = propertyExpression.Compile();
        _fieldName = (propertyExpression.Body as MemberExpression)?.Member.Name ?? "Field";
    }

    protected override Task HandleValidation(ValidationRequest<T> request)
    {
        var value = _valueProvider(request.DtoToValidate);
        if (value == null)
        {
            request.Errors.Add($"{_fieldName} is required.");
        }
        return Task.CompletedTask;
    }
}