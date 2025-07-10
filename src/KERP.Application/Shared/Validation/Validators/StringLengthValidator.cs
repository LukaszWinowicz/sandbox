using System.Linq.Expressions;

namespace KERP.Application.Shared.Validation.Validators;

public class StringLengthValidator<T> : AbstractValidationHandler<T> where T : class
{
    private readonly Func<T, string?> _valueProvider;
    private readonly int _exactLength;
    private readonly string _fieldName;

    public StringLengthValidator(Expression<Func<T, string?>> propertyExpression, int exactLength)
    {
        _valueProvider = propertyExpression.Compile();
        _fieldName = (propertyExpression.Body as MemberExpression)?.Member.Name ?? "Field";
        _exactLength = exactLength;
    }

    protected override Task HandleValidation(ValidationRequest<T> request)
    {
        var value = _valueProvider(request.DtoToValidate);
        if (!string.IsNullOrEmpty(value) && value.Length != _exactLength)
        {
            request.Errors.Add($"{_fieldName} must be exactly {_exactLength} characters long.");
        }
        return Task.CompletedTask;
    }
}