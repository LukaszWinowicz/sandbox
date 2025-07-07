using KERP.Core.Validators.Context;

namespace KERP.Core.Validators.Handlers;

/// <summary>
/// Defines the contract for a single link in the validation chain.
/// </summary>
public interface IValidationHandler
{
    /// <summary>
    /// Sets the next handler in the chain.
    /// </summary>
    /// <param name="nextHandler">The next handler to be executed.</param>
    void SetNext(IValidationHandler nextHandler);

    /// <summary>
    /// Executes the validation logic for this handler.
    /// </summary>
    /// <param name="request">The validation request containing the DTO.</param>
    Task ValidateAsync(ValidationRequest request);

}
