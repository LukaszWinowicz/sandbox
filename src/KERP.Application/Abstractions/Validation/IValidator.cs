namespace KERP.Application.Abstractions.Validation;

public interface IValidator<TCommand>
{
    Task<IReadOnlyCollection<string>> ValidateAsync(TCommand command, CancellationToken cancellationToken = default);
}
