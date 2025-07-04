using MassUpdateApp.Core.Interfaces.Repositories;

namespace MassUpdateApp.Infrastructure.Persistence.Repositories;

/// <summary>
/// Implements the read-only repository for fetching data used in validation processes.
/// </summary>
public class ValidationDataRepository : IValidationDataRepository
{
    private readonly AppDbContext _context;
    public ValidationDataRepository(AppDbContext context)
    {
        _context = context; 
    }


}
