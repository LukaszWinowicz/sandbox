namespace KERP.Application.Common.Abstractions;

/// <summary>
/// Interfejs-znacznik dla komendy, która zwraca określony rezultat.
/// </summary>
public interface ICommand<out TResult>
{
}

/// <summary>
/// Interfejs-znacznik dla komendy, która nie zwraca żadnej wartości.
/// </summary>
public interface ICommand
{
}
