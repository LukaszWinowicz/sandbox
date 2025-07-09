using KERP.Application.Abstractions.Messaging;
using KERP.Application.Validation.Context;
using KERP.Application.Validators.Handlers;

namespace KERP.Application.Validation.Components;
/// <summary>
/// A generic validator that checks a complex business rule involving multiple fields
/// by invoking a provided function.
/// </summary>
/// <typeparam name="TCommand">The specific command type this validator works with.</typeparam>
/// <typeparam name="TRepository">The type of the repository needed for the check.</typeparam>
public class CombinationValidator<TCommand, TRepository> : ValidationHandler
    where TCommand : CommandBase
{
    private readonly TRepository _repository;
    private readonly Func<TCommand, TRepository, Task<bool>> _combinationCheckFunc;
    private readonly string _errorMessage;

    public CombinationValidator(TRepository repository, Func<TCommand, TRepository, Task<bool>> combinationCheckFunc, string errorMessage)
    {
        _repository = repository;
        _combinationCheckFunc = combinationCheckFunc;
        _errorMessage = errorMessage;
    }

    public override async Task ValidateAsync(ValidationRequest request)
    {
        if (request.Command is TCommand specificCommand)
        {
            if (!await _combinationCheckFunc(specificCommand, _repository))
            {
                request.Errors.Add(_errorMessage);
            }
        }

        await PassToNextAsync(request);
    }
}