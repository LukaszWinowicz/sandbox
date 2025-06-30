using MassUpdate.Core.DTOs;

namespace MassUpdate.Core.Interfaces;

/// <summary>
/// Generyczny kontrakt dla strategii walidacji. Ten interfejs może być zaimplementowany dla typów T,
/// które dziedziczą po (abstrakcyjnym) MassUpdateDto.
/// </summary>
/// <typeparam name="T">Typ DTO, który ta strategia potrafi walidować.</typeparam>
public interface IValidationStrategy<T> where T : MassUpdateDto
{
    /// <summary>
    /// Uruchamia logikę walidacji dla danego obiektu DTO.
    /// </summary>
    /// <param name="dto">Obiekt DTO do zwalidowania.</param>
    /// <returns>Lista stringów z komunikatami o błędach. Pusta lista oznacza sukces.</returns>
    List<string> Validate(T dto);
}
