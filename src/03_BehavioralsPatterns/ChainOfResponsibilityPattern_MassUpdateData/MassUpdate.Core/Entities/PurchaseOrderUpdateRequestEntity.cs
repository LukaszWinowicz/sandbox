using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MassUpdate.Core.Entities;

/// <summary>
/// Reprezentuje pojedynczą prośbę o masową aktualizację zlecenia zakupu,
/// która zostanie zapisana w bazie danych.
/// </summary>
public class PurchaseOrderUpdateRequestEntity
{
    // Klucz główny, nadawany automatycznie przez bazę danych
    [Key]
    public int Id { get; set; }

    // --- Dane wprowadzone przez użytkownika (zmapowane z DTO) ---

    [Required]
    [MaxLength(10)]
    public required string PurchaseOrder { get; set; }

    [Required]
    public required int LineNumber { get; set; }

    [Required]
    public required int Sequence { get; set; }

    [Required]
    [Column(TypeName = "date")]
    public required DateTime ReceiptDate { get; set; }

    [Required]
    public required string DateType { get; set; } // "Confirmed" lub "Changed"

    // --- Metadane dodawane przez system w momencie zapisu ---

    [Required]
    public DateTime AddedDate { get; set; }

    [Required]
    public required string UserId { get; set; }

    [Required]
    public bool IsProcessed { get; set; } = false;

    [Column(TypeName = "date")]
    public DateTime? ProcessedDate { get; set; }

}