namespace KERP.Application.Abstractions.CQRS;

/// <summary>
/// Interfejs dla handlera obsługującego zapytanie.
/// Odpowiada za pobranie danych bez modyfikacji stanu.
/// </summary>
/// <typeparam name="TQuery">Typ zapytania.</typeparam>
/// <typeparam name="TResult">Typ wyniku zapytania.</typeparam>
public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
{
    /// <summary>
    /// Obsługuje zapytanie i zwraca wynik.
    /// </summary>
    /// <param name="query">Zapytanie do obsłużenia.</param>
    /// <param name="cancellationToken">Token anulujący operację.</param>
    /// <returns>Wynik zapytania.</returns>
    Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken);
}
