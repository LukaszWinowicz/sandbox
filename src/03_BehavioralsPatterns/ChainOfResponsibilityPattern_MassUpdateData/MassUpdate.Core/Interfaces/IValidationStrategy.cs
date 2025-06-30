using MassUpdate.Core.DTOs;

namespace MassUpdate.Core.Interfaces;

/// <summary>
/// "Ten interfejs może być implementowany tylko dla typów T, które dziedziczą po MassUpdateDto."
/// </summary>
public interface IValidationStrategy<T> where T : MassUpdateDto
{
}
