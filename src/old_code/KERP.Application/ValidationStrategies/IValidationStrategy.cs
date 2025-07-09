using KERP.Application.Abstractions.Messaging;

namespace KERP.Application.Interfaces.ValidationStrategies;

/// <summary>
/// Defines a generic contract for a validation strategy.
/// Each strategy is responsible for validating a specific command.
/// </summary>
/// <typeparam name="TCommand">The type of the command this strategy can validate.</typeparam>
public interface IValidationStrategy<TCommand> where TCommand : CommandBase
{
    /// <summary>
    /// Executes the validation logic for the given command.
    /// </summary>
    /// <param name="command">The command object to validate.</param>
    /// <returns>A list of validation error messages. An empty list signifies success.</returns>
    Task<List<string>> ValidateAsync(TCommand command);
}
