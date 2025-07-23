using KERP.Application.Common.Abstractions;
using KERP.Application.Common.Models;
using KERP.Application.Validation;
using System.Reflection;

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

            if (typeof(TResult) == typeof(Result))
            {
                return (Result.Failure(error) as TResult)!;
            }

            // --- POCZĄTEK POPRAWIONEJ LOGIKI ---
            var resultType = typeof(TResult).GetGenericArguments()[0];

            // Znajdź metodę generyczną o nazwie "Failure"
            var failureMethod = typeof(Result)
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .First(m => m.Name == nameof(Result.Failure) && m.IsGenericMethodDefinition);

            // Stwórz konkretną metodę generyczną (np. Failure<List<RowValidationResult>>)
            var genericFailureMethod = failureMethod.MakeGenericMethod(resultType);

            // Wywołaj ją
            var failureResult = genericFailureMethod.Invoke(null, new object[] { error });
            // --- KONIEC POPRAWIONEJ LOGIKI ---

            return (TResult)failureResult!;
        }

        return await _decorated.Handle(command, cancellationToken);
    }
}