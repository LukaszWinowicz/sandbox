using MassUpdate.Core.DTOs;

namespace MassUpdate.Core.Handlers;

public interface IValidationHandler
{
    void SetNext(IValidationHandler handler);
    void Validate(ValidationRequest request);
}
