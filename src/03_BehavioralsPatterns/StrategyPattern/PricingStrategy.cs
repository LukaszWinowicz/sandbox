namespace StrategyPattern;

// Model danych
public class Product
{
    public string Name { get; }
    public decimal BasePrice { get; }
    public IPricingStrategy PricingStrategy { get; }

    public Product(string name, decimal basePrice, IPricingStrategy strategy)
    {
        Name = name;
        BasePrice = basePrice;
        PricingStrategy = strategy;
    }

    public decimal GetPrice(decimal quantity)
    {
        return PricingStrategy.CalculatePrice(this, quantity);
    }
}


// Abstract Strategy
public interface IPricingStrategy
{
    decimal CalculatePrice(Product product, decimal quantity);
}

public class PerUnitPricingStrategy : IPricingStrategy
{
    public decimal CalculatePrice(Product product, decimal quantity)
        => product.BasePrice * quantity;
}

public class PerSquareMeterPricingStrategy : IPricingStrategy
{
    public decimal CalculatePrice(Product product, decimal squareMeters)
        => product.BasePrice * squareMeters;
}

// Decorator
public class BulkDiscountDecorator : IPricingStrategy
{
    private readonly IPricingStrategy _baseStrategy;
    private readonly decimal _threshold;
    private readonly decimal _discountPercent;

    public BulkDiscountDecorator(IPricingStrategy baseStrategy, decimal threshold, decimal discountPercent)
    {
        _baseStrategy = baseStrategy;
        _threshold = threshold;
        _discountPercent = discountPercent;
    }

    public decimal CalculatePrice(Product product, decimal quantity)
    {
        var basePrice = _baseStrategy.CalculatePrice(product, quantity);
        return quantity >= _threshold ? basePrice * (1 - _discountPercent) : basePrice;
    }
}
