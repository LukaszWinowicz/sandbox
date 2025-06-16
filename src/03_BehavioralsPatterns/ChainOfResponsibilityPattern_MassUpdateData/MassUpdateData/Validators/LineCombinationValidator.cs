using MassUpdateData.Handlers;
using MassUpdateData.Models;
using MassUpdateData.Services;

namespace MassUpdateData.Validators;

public class LineCombinationValidator : ValidationHandler
{
    private readonly OrderDataService _dataService;

    // Wstrzykujemy serwis przez konstruktor
    public LineCombinationValidator(OrderDataService dataService)
    {
        _dataService = dataService;
    }

    public override void Validate(UpdateRequest request)
    {
        if (!_dataService.LineCombinationExists(request.Order, request.Line, request.Sequence))
        {
            request.ValidationErrors.Add($"ERROR: Kombinacja Linii i Sekwencji ({request.Line}-{request.Sequence}) nie istnieje dla zlecenia '{request.Order}'.");
        }
        PassToNext(request);
    }
}
