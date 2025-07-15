namespace KERP.Application.Abstractions.CQRS;

/// <summary>
/// Interfejs markerowy reprezentujący zapytanie w architekturze CQRS.
/// Zapytania nie zmieniają stanu aplikacji i zwracają wynik.
/// </summary>
/// <typeparam name="TResult">Typ danych, które zapytanie ma zwrócić.</typeparam>
public interface IQuery<TResult> { }