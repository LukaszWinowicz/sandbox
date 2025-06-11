using System;

namespace PrototypePattern;

// Własny interfejs generyczny bo oryginalny ICloneable zwraca tylko object
public interface ICloneable<T> : ICloneable
{
    new T Clone();
}

public class Offer : ICloneable<Offer>
{
    public string OfferNumber { get; set; }
    public string Product { get; set; }
    public decimal BasePrice { get; set; }
    public decimal DiscountPercent { get; set; }
    public DateTime ValidUntil { get; set; }

    public OfferOptions Options { get; set; } = new(); // <--- zagnieżdżony obiekt

    public decimal GetFinalPrice()
    {
        var price = BasePrice * (1 - DiscountPercent / 100);
        if (Options.IncludeInstallation) price += 200; // np. koszt montażu
        if (Options.ExtendedWarranty) price += 150;
        return price;
    }

    private Offer Copy()
    {
        var copy = FastDeepCloner.DeepCloner.Clone(this); // głęboka kopia (Deep Copy)

        return copy;
    }

    public override string ToString() =>
        $"{OfferNumber}: {Product} ({BasePrice:C} - {DiscountPercent}% = {GetFinalPrice():C}), valid until {ValidUntil:d}";

    public Offer Clone()
    {
        return Copy();
    }

    object ICloneable.Clone()
    {
        return Clone();
    }
}

public class OfferOptions
{
    public bool IncludeInstallation { get; set; }
    public bool ExtendedWarranty { get; set; }
    public string Currency { get; set; } = "PLN";

    public OfferOptions Copy()
    {
        return (OfferOptions) this.MemberwiseClone();
    }
}
