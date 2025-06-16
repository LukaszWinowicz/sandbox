using MassUpdateData.Handlers;
using MassUpdateData.Models;

namespace MassUpdateData.Validators;

public class FutureDateValidator : ValidationHandler
{
    public override void Validate(UpdateRequest request)
    {
        if (request.ConfirmationDate.Date <= DateTime.Now.Date)
        {
            request.ValidationErrors.Add($"Error: ConfirmationDate ({request.ConfirmationDate:yyyy-MM-dd}) shoudl be from future.");
        }
    }
}
