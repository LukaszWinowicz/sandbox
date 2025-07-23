using KERP.Domain.Common;

namespace KERP.Domain.Aggregates.Common;

/// <summary>
/// Agregat reprezentujący fabrykę.
/// </summary>
public class Factory : AggregateRoot<Guid>
{
    /// <summary>
    /// Identyfikator fabryki.
    /// </summary>
    public int FactoryId { get; private set; }

    /// <summary>
    /// Opis fabryki.
    /// </summary>
    public string Description { get; private set; }

    /// <summary>
    /// Konstruktor dla EF.
    /// </summary>
    protected Factory() { }
}