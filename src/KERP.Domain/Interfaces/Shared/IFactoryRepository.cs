using KERP.Domain.Entities.Shared;

namespace KERP.Domain.Interfaces.Shared;

public interface IFactoryRepository
{
    Task<FactoryEntity?> GetByIdAsync(int factoryId, CancellationToken cancellationToken = default);
}
