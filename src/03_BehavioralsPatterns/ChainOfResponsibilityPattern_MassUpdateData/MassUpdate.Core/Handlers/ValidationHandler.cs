using MassUpdate.Core.DTOs;

namespace MassUpdate.Core.Handlers;

public abstract class ValidationHandler : IValidationHandler
{
    protected IValidationHandler? _nextHandler;

    public void SetNext(IValidationHandler handler)
    {
        _nextHandler = handler;
    }

    public abstract void Validate(ValidationRequest request);

    protected void PassToNext(ValidationRequest request)
    {
        _nextHandler?.Validate(request);
    }
}
