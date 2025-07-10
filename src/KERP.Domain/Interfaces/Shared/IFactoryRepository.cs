using KERP.Domain.Entities.Shared;

namespace KERP.Domain.Interfaces.Shared;

public interface IFactoryRepository
{
    Task<Factory?> GetByIdAsync(int factoryId, CancellationToken cancellationToken = default);
}
