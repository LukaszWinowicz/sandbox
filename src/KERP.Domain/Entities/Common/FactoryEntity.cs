namespace KERP.Domain.Entities.Common;

/// <summary>
/// Reprezentuje fabrykę jako encję referencyjną w systemie.
/// Przechowuje konfigurację dotyczącą zamówień.
/// </summary>
public class FactoryEntity
{
    /// <summary>
    /// Identyfikator fabryki (np. 241, 276).
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    /// Przyjazna nazwa fabryki (np. "Factory A").
    /// </summary>
    public string Name { get; private set; }
}