using System;
using Xunit;

namespace StrategyPattern.UnitTests;

public class PricingStrategyTests
{

    [Fact]
    public void Test()
    {        
        var screw = new Product("Śruba", 1.50m, new BulkDiscountDecorator( new PerUnitPricingStrategy(), 100, 0.1m));
        var panel = new Product("Panel podłogowy", 10.00m, new PerSquareMeterPricingStrategy());
        var bricks = new Product("Cegła", 2.00m, new PerUnitPricingStrategy());

        Console.WriteLine($"Cena śrub (30 szt): {screw.GetPrice(30)} zł");
        Console.WriteLine($"Cena paneli (20 m²): {panel.GetPrice(20m)} zł");
        Console.WriteLine($"Cena cegieł (120 szt): {bricks.GetPrice(120)} zł");

    }

}
