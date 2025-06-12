using System.Collections.Generic;

namespace DecoratorPattern;

// Model danych
public class Product
{
    public string Symbol { get; set; }
    public string Name { get; set; }
    public decimal UnitPrice { get; set; }
}

// Abstract Component
public interface IPricingRepository
{
    decimal GetPrice(string symbol);
}

// Concrete Component
public class FakePricingRepository : IPricingRepository
{
    private Dictionary<string, Product> _products = new Dictionary<string, Product>
    {
        ["a"] = new Product { Name = "Aaa", Symbol = "a", UnitPrice = 10m },
        ["b"] = new Product { Name = "Bbb", Symbol = "b", UnitPrice = 20m },
        ["c"] = new Product { Name = "Ccc", Symbol = "c", UnitPrice = 30m },
    };

    public decimal GetPrice(string symbol)
    {
        if (_products.TryGetValue(symbol, out var product))
        {
            return product.UnitPrice;
        }

        throw new KeyNotFoundException();
    }
}

// Abstract Decorator
public abstract class PricingDecorator : IPricingRepository
{
    public abstract decimal GetPrice(string symbol);
}

// Concrete Decorator A
public class DiscountedPricingDecorator(IPricingRepository repository, decimal discountRate) : PricingDecorator
{
    public override decimal GetPrice(string symbol)
    {
        var originalPrice = repository.GetPrice(symbol);

        return originalPrice * (1 - discountRate);
    }
}


// Concrete Decorator B
public class CurrencyPricingDecorator(IPricingRepository repository, ICurrencyService currencyService) : PricingDecorator
{
    public override decimal GetPrice(string symbol)
    {
        var price = repository.GetPrice(symbol);

        return price * currencyService.GetRatio(symbol);
    }
}


public interface ICurrencyService
{
    decimal GetRatio(string currencySymbol);
}

public class FakeCurrencyService : ICurrencyService
{
    public decimal GetRatio(string currencySymbol) => 4.01m;
}



