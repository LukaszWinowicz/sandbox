using MassUpdate.Core.DTOs;

namespace MassUpdateData.Tests;

public class PurchaseOrderDtoBuilder
{
    private string _purchaseOrder;
    private int _lineNumber;
    private int _sequence;
    private DateTime _receiptDate;

    // Konstruktor ustawia domyślnie, w PEŁNI POPRAWNE wartości.
    public PurchaseOrderDtoBuilder()
    {
        _purchaseOrder = "PO12345678"; 
        _lineNumber = 10;
        _sequence = 1;
        _receiptDate = DateTime.Now.AddDays(7);
    }

    // Metody "With" pozwalają na płynne nadpisywanie domyślnych wartości.
    // Każda z nich zwraca samego siebie, aby umożliwić łączenie wywołań.
    public PurchaseOrderDtoBuilder WithPurchaseOrder(string purchaseOrder)
    {
        _purchaseOrder = purchaseOrder;
        return this;
    }

    public PurchaseOrderDtoBuilder WithLineNumber(int lineNumber)
    {
        _lineNumber = lineNumber;
        return this;
    }

    public PurchaseOrderDtoBuilder WithSequence(int sequence)
    {
        _sequence = sequence;
        return this;
    }

    public PurchaseOrderDtoBuilder WithReceiptDate(DateTime receiptDate)
    {
        _receiptDate = receiptDate;
        return this;
    }

    // Metoda Build składa finalny obiekt z przygotowanych wartości.
    public MassUpdatePurchaseOrderDto Build()
    {
        return new MassUpdatePurchaseOrderDto
        { 
            PurchaseOrder = _purchaseOrder, 
            LineNumber = _lineNumber,
            Sequence = _sequence,
            ReceiptDate = _receiptDate 
        };
    }
}