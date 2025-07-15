namespace KERP.Application.Common.Context;

public interface ICurrentUserContext
{
    string UserId { get; }
    int FactoryId { get; }
}