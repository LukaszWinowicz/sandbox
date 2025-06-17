namespace MassUpdateData.Models;

// Kontener na żądanie
public class ValidationRequest
{
    public MassUpdateDto Dto { get; set; }
    public List<string> Errors { get; } = new List<string>();
    public bool IsValid => Errors.Count == 0;

    public ValidationRequest(MassUpdateDto dto)
    {
        Dto = dto;
    }
}
