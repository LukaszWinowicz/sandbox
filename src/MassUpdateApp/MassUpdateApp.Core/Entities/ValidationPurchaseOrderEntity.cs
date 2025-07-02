using System.ComponentModel.DataAnnotations;

namespace MassUpdateApp.Core.Entities;

// Ta prosta encja symuluje strukturę danych tylko do odczytu,
// które służą do walidacji.
public class ValidationPurchaseOrderEntity
{
    [Key]
    public string PurchaseOrder {  get; set; }
    public int LineNumber { get; set; }
    public int Sequence {  get; set; }
}
