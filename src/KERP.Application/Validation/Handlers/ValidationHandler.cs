using KERP.Application.Validation.Context;
using KERP.Application.Validation.Handlers;

namespace KERP.Application.Validators.Handlers;

/// <summary>
/// An abstract base class for validation handlers, providing core
/// functionality for chaining handlers together.
/// </summary>
public abstract class ValidationHandler : IValidationHandler
{
    protected IValidationHandler? _nextHandler;

    /// <inheritdoc />
    public void SetNext(IValidationHandler nextHandler)
    {
        _nextHandler = nextHandler;
    }

    /// <inheritdoc />
    public abstract Task ValidateAsync(ValidationRequest request);

    /// <summary>
    /// Passes the request to the next handler in the chain, if one exists.
    /// </summary>
    protected async Task PassToNextAsync(ValidationRequest request)
    {
        if (_nextHandler != null)
        {
            await _nextHandler.ValidateAsync(request);
        }
    }
}
