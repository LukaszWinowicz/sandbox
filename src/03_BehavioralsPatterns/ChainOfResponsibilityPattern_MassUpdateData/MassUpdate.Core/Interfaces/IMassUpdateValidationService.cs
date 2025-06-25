using MassUpdate.Core.DTOs;

namespace MassUpdate.Core.Interfaces;

public interface IMassUpdateValidationService
{
    List<RowValidationResult> Validate<T>(List<T> dtoList) where T : MassUpdateDto;
}