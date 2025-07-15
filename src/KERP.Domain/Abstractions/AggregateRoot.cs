namespace KERP.Domain.Abstractions;

/// <summary>
/// Bazowa generyczna klasa agregatu domenowego.
/// Zapewnia identyfikator typu <typeparamref name="TId"/> oraz możliwość rozszerzenia o eventy.
/// </summary>
/// <typeparam name="TId">Typ identyfikatora agregatu, np. Guid lub int.</typeparam>
public abstract class AggregateRoot<TId> : IAggregateRoot
{
    /// <summary>
    /// Unikalny identyfikator agregatu.
    /// </summary>
    public TId Id { get; protected set; }

    protected AggregateRoot(TId id)
    {
        Id = id;
    }

    // 🔧 Przestrzeń na zdarzenia domenowe (opcjonalna):
    // private readonly List<IDomainEvent> _domainEvents = new();
    // public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    // protected void AddDomainEvent(IDomainEvent @event) => _domainEvents.Add(@event);
}