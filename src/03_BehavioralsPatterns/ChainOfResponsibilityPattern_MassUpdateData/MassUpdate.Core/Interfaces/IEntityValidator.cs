using MassUpdate.Core.DTOs;

namespace MassUpdate.Core.Interfaces;

public interface IEntityValidator<T> where T : MassUpdateDto
{
    List<string> Validate(T dto);
}
