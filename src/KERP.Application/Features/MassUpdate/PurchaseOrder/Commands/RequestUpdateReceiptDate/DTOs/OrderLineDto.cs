namespace KERP.Application.Features.MassUpdate.PurchaseOrder.Commands.RequestUpdateReceiptDate.DTOs;

/// <summary>
/// DTO reprezentujący pojedynczą linię żądania aktualizacji daty odbioru.
/// Używany jako część komendy.
/// </summary>
public class OrderLineDto
{
    // Używamy publicznych setterów, aby Blazor mógł łatwo bindować dane z formularza.
    public string PurchaseOrder { get; set; } = string.Empty;
    public int LineNumber { get; set; }
    public int Sequence { get; set; }
    public DateTime NewReceiptDate { get; set; } = DateTime.Today;
}

