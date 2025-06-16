namespace MassUpdateData.Models;

public class UpdateRequest
{
    public string Order { get; set; }
    public int Line { get; set; }
    public int Sequence { get; set; }

    public DateTime ConfirmationDate { get; set; }

    public List<string> ValidationErrors { get; } = new List<string>();

    public bool IsValid => ValidationErrors.Count() == 0;
}

