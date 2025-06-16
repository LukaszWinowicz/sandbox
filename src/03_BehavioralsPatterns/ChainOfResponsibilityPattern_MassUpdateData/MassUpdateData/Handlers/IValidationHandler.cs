using MassUpdateData.Models;

namespace MassUpdateData.Handlers;

public interface IValidationHandler
{
    void SetNext(IValidationHandler handler);
    void Validate(UpdateRequest request);
}
