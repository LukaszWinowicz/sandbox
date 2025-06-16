namespace MassUpdateData.Services;

public interface IOrderDataService
{
    bool OrderExists(string order);
    bool LineCombinationExists(string order, int line, int sequence);
}
