using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PrototypePattern.UnitTests;

// {Method}_{Scenario}_{ExpectedBehavior}
// Copy_WhenValidOffer_ReturnsCopy
// Copy_WhenInvalidOffer_ThrowInvalidArgumentException

public class OfferTests
{
    [Fact]
    public void ManualCopy_ShouldCreateIndependentOfferButIsErrorProne()
    {
        // Arrange — szablon oferty
        var template = new Offer
        {
            OfferNumber = "TEMPLATE",
            Product = "5 Gallon Water",
            BasePrice = 25.00m,
            DiscountPercent = 10,
            ValidUntil = DateTime.Today.AddDays(14)
        };

        // Act — kopiowanie ręczne (ktoś zapomniał o polu DiscountPercent!)
        var copied = template.Copy();
        copied.OfferNumber = "OFFER-001";

        // Assert — pole DiscountPercent NIE zostało skopiowane (ma wartość domyślną 0)
        Assert.Equal(template.DiscountPercent, copied.DiscountPercent); // ❌ test nie przechodzi, pokazuje błąd projektowy
        Assert.Equal(22.5m, template.GetFinalPrice()); // 25 - 10%
        Assert.Equal(22.5m, copied.GetFinalPrice());   // brak rabatu
        Assert.NotSame(template, copied);
    }


    [Fact]
    public void ManualCopy_ShouldCreateShallowCopy_WhenOptionsNotCloned()
    {
        // Arrange – szablon oferty z opcjami
        var template = new Offer
        {
            OfferNumber = "TEMPLATE",
            Product = "Pergola 125",
            BasePrice = 7000,
            DiscountPercent = 5,
            ValidUntil = DateTime.Today.AddDays(30),
            Options = new OfferOptions
            {
                IncludeInstallation = true,
                ExtendedWarranty = false,
                Currency = "PLN"
            }
        };

        // Act – ręczna kopia (naiwna)
        var copied = template.Copy();

        // Modyfikujemy kopię
        copied.Options.Currency = "USD";

        // Assert – zmiana zaszła także w szablonie (błąd!)
        Assert.Equal("PLN", template.Options.Currency);      // ❌ niepożądany efekt uboczny
        Assert.NotSame(template.Options, copied.Options);        // ❌ współdzielona referencja
    }
}
