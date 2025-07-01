using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MassUpdateApp.Core.Entities;

/// <summary>
/// Reprezentuje pojedynczą, zwalidowaną prośbę o aktualizację Purchase Order,
/// przeznaczoną do zapisu w lokalnej bazie danych aplikacji.
/// </summary>
public class PurchaseOrderUpdateRequestEntity
{
    /// <summary>
    /// Klucz główny, generowany przez bazę danych.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Numer Purchase Order, którego dotyczy aktualizacja.
    /// </summary>
    [Required]
    [MaxLength(10)]
    public required string PurchaseOrder { get; set; }

    /// <summary>
    /// Numer lini na Purchase Order.
    /// </summary>
    [Required]
    public required int LineNumber { get; set; }

    /// <summary>
    /// Numer sekwencji na Purchase Order.
    /// </summary>
    [Required]
    public required int Sequence { get; set; }

    /// <summary>
    /// Nowa data, która ma zostać ustawiona.
    /// </summary>
    [Required]
    [Column(TypeName = "date")]
    public required DateTime ReceiptDate { get; set; }

    /// <summary>
    /// Typ aktualizacji daty (np. "Confirmed", "Changed").
    /// </summary>
    [Required]
    public required string DateType { get; set; }

    /// <summary>
    /// Identyfikator użytkownika, który zlecił aktualizację.
    /// </summary>
    [Required]
    public required string UserId { get; set; }

    /// <summary>
    /// Data i czas dodania rekordu do bazy. Ustawiana automatycznie.
    /// </summary>
    public DateTime AddedDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Flaga informująca, czy ta prośba została już przetworzona i wysłana do systemu ERP.
    /// </summary>
    public bool IsProcessed { get; set; } = false;

    /// <summary>
    /// Data przetworzenia. Może być null, jeśli jeszcze nieprzetworzone.
    /// </summary>
    [Column(TypeName = "date")]
    public DateTime? ProcessedDate { get; set; }
}
