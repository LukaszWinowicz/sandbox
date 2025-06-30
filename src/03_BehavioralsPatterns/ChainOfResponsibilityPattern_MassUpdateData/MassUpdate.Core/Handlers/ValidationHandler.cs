﻿using MassUpdate.Core.DTOs;

namespace MassUpdate.Core.Handlers;

public abstract class ValidationHandler : IValidationHandler
{
    protected IValidationHandler? _nextHandler;
    public void SetNext(IValidationHandler handler) => _nextHandler = handler;
    public abstract Task ValidateAsync(ValidationRequest request);
    protected async Task PassToNextAsync(ValidationRequest request)
    {
        if (_nextHandler != null) await _nextHandler.ValidateAsync(request);
    }
}
