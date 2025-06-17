using MassUpdateData.Handlers;
using MassUpdateData.Models;
using MassUpdateData.Services;

namespace MassUpdateData.Validators;

public class OrderExistenceValidator : ValidationHandler
{
    private readonly IOrderDataService _dataService;

    public OrderExistenceValidator(IOrderDataService dataService)
    {
        _dataService = dataService;
    }

    public override void Validate(UpdateRequest request)
    {
        if (!_dataService.OrderExists(request.Order))
        {
            // Step 1: Add critical error
            request.ValidationErrors.Add($"ERROR: Order '{request.Order}' does not exist in the database.");

            // Step 2: End validation. Do not call PassToNext().
            return;

        }

        // If everything is OK (order exists), continue the chain normally.
        PassToNext(request);
    }
}
