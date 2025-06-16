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
            request.ValidationErrors.Add($"ERROR: Order '{request.Order}' does not exist in the database.");
        }

        PassToNext(request);
    }
}
