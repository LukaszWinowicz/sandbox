using MassUpdateData.Models;

namespace MassUpdateData.Handlers;

public abstract class ValidationHandler : IValidationHandler
{
    protected IValidationHandler _nextHandler;

    public void SetNext(IValidationHandler handler)
    {
        _nextHandler = handler;
    }

    public abstract void Validate(UpdateRequest request);

    protected void PassToNext(UpdateRequest request)
    {
        _nextHandler?.Validate(request);
    }
}
