using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrototypePattern;

public class Offer
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

    public Offer Copy()
    {
        var copy = new Offer
        {
            Product = this.Product,
            BasePrice = this.BasePrice,
            ValidUntil = this.ValidUntil,
            DiscountPercent = this.DiscountPercent,            
        };

        return copy;
    }

    public override string ToString() =>
        $"{OfferNumber}: {Product} ({BasePrice:C} - {DiscountPercent}% = {GetFinalPrice():C}), valid until {ValidUntil:d}";
}

public class OfferOptions
{
    public bool IncludeInstallation { get; set; }
    public bool ExtendedWarranty { get; set; }
    public string Currency { get; set; } = "PLN";
}
