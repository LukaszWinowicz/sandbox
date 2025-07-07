using KERP.Core.Abstractions.Messaging;

namespace KERP.Core.Validators.Context;

/// <summary>
/// Represents the context for a single validation operation,
/// carrying the object to be validated and collecting any validation errors.
/// </summary>
public class ValidationRequest
{
    /// <summary>
    /// The command object that is being validated.
    /// </summary>
    public CommandBase Command { get; }

    /// <summary>
    /// A list of validation error messages discovered during the process.
    /// </summary>
    public List<string> Errors { get; } = new List<string>();

    /// <summary>
    /// A convenience property to check if any errors have been recorded.
    /// </summary>
    public bool IsValid => !Errors.Any();

    public ValidationRequest(CommandBase command)
    {
        Command = command;
    }
}