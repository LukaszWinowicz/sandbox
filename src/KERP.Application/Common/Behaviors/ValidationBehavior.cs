using KERP.Application.Common.Abstractions;
using KERP.Application.Common.Models;
using KERP.Application.Validation;
namespace KERP.Application.Common.Behaviors;

public class ValidationBehavior<TCommand, TResult> : ICommandHandler<TCommand, TResult>
where TCommand : ICommand<TResult>
where TResult : Result // Ograniczamy do komend zwracających nasz obiekt Result
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
            // Jeśli nie ma walidatorów dla tej komendy, idź dalej
            return await _decorated.Handle(command, cancellationToken);
        }

        var errors = _validators
            .Select(v => v.Validate(command))
            .SelectMany(result => result.Errors)
            .ToList();

        if (errors.Any())
        {
            // Jeśli są błędy, przerwij potok i zwróć błąd walidacji
            // Używamy refleksji, aby stworzyć obiekt Result.Failure<TResult>
            var firstError = errors.First();
            var failureResult = typeof(Result).GetMethod(nameof(Result.Failure), new[] { typeof(Error) })!
                .MakeGenericMethod(typeof(TResult).GetGenericArguments()[0])
                .Invoke(null, new object[] { new Error(firstError.PropertyName, firstError.ErrorMessage) });

            return (TResult)failureResult!;
        }

        return await _decorated.Handle(command, cancellationToken);
    }
}
