namespace KERP.Domain.Entities.Shared;

/// <summary>
/// Reprezentuje fabrykę (jednostkę operacyjną) w systemie.
/// </summary>
public class Factory
{
    /// <summary>
    /// Identyfikator fabryki (np. 241, 276).
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Pełna nazwa fabryki (np. "Kalmar Stargard").
    /// </summary>
    public required string Name { get; set; }
}
