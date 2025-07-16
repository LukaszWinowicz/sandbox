namespace KERP.Application.Abstractions.CQRS;

/// <summary>
/// Interfejs markerowy reprezentujący komendę w architekturze CQRS.
/// Komendy służą do wykonywania operacji zmieniających stan aplikacji.
/// </summary>
// Zmieniamy ICommand, aby mógł zwracać wynik
public interface ICommand<TResult> { }