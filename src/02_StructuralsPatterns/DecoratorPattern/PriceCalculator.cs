using System.Collections.Generic;

namespace DecoratorPattern;

// Abstract Component
public interface IPricingRepository
{
    decimal GetPrice(string symbol);
}

// Concrete Decorator A
public class DiscountedPricingRepository : IPricingRepository
{
    private readonly IPricingRepository repository;
    private readonly decimal discountRate;

    public DiscountedPricingRepository(IPricingRepository repository, decimal discountRate)
    {
        this.repository = repository;
        this.discountRate = discountRate;
    }

    public decimal GetPrice(string symbol)
    {
        var originalPrice = repository.GetPrice(symbol);

        return originalPrice * (1 - discountRate);
    }
}


public interface ICurrencyService
{
    decimal GetRatio(string currencySymbol);
}

public class FakeCurrencyService : ICurrencyService
{
    public decimal GetRatio(string currencySymbol)
    {
        return 4.01m;
    }
}

// Concrete Decorator B
public class CurrencyPricingRepository : IPricingRepository
{
    private readonly IPricingRepository repository;
    private readonly ICurrencyService currencyService;

    public CurrencyPricingRepository(IPricingRepository repository, ICurrencyService currencyService)
    {
        this.repository = repository;
        this.currencyService = currencyService;
    }

    public decimal GetPrice(string symbol)
    {
        var price = repository.GetPrice(symbol);

        return price * currencyService.GetRatio(symbol);
    }
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



public class Product
{
    public string Symbol { get; set; }
    public string Name { get; set; }
    public decimal UnitPrice { get; set; }
}