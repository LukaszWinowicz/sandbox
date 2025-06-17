namespace MassUpdateData.Models;

// Kontener na żądanie
public class ValidationRequest
{
    public MassUpdateDto Dto { get; set; }
    public List<string> ValidationErrors { get; } = new List<string>();
    public bool IsValid => ValidationErrors.Count == 0;

    public ValidationRequest(MassUpdateDto dto)
    {
        Dto = dto;
    }
}
