namespace KERP.Application.Common.Models;

// Używamy rekordu, aby zapewnić niezmienność (immutability)
public sealed record Error(string Code, string Description)
{
    public static readonly Error None = new(string.Empty, string.Empty);
}
