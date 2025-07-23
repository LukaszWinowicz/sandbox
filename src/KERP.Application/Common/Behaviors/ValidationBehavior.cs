using KERP.Application.Common.Abstractions;
using KERP.Application.Common.Models;
using KERP.Application.Validation;
namespace KERP.Application.Common.Behaviors;

public class ValidationBehavior<TCommand, TResult> : ICommandHandler<TCommand, TResult>
    where TCommand : ICommand<TResult>
    where TResult : Result
{
    private readonly ICommandHandler<TCommand, TResult> _decorated;
    private readonly IEnumerable<IValidator<TCommand>> _validators;

    public ValidationBehavior(
        ICommandHandler<TCommand, TResult> decorated,
        IEnumerable<IValidator<TCommand>> validators)
    {
        _decorated = decorated;
        _validators = validators;
    }

    public async Task<TResult> Handle(TCommand command, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await _decorated.Handle(command, cancellationToken);
        }

        var errors = _validators
            .Select(v => v.Validate(command))
            .SelectMany(result => result.Errors)
            .ToList();

        if (errors.Any())
        {
            var firstError = errors.First();
            var error = new Error(firstError.PropertyName, firstError.ErrorMessage);

            // Sprawdzamy, czy oczekiwany rezultat to prosty, niegeneryczny Result
            if (typeof(TResult) == typeof(Result))
            {
                // Jeśli tak, tworzymy go i rzutujemy.
                return (Result.Failure(error) as TResult)!;
            }

            // Jeśli nie, to znaczy, że jest to generyczny Result<TValue> i używamy refleksji
            var resultType = typeof(TResult).GetGenericArguments()[0];
            var failureResult = typeof(Result)
                .GetMethod(nameof(Result.Failure), new[] { typeof(Error) })!
                .MakeGenericMethod(resultType)
                .Invoke(null, new object[] { error });

            return (TResult)failureResult!;
        }

        return await _decorated.Handle(command, cancellationToken);
    }
}