using System;

namespace StrategyPattern;


// Abstract Strategy
public interface IDiscountStrategy
{
    double GetDiscount(double price);
}

// Concrete Strategy A
public class PercentageDiscountStrategy : IDiscountStrategy
{
    private double _percentage;

    public PercentageDiscountStrategy(double percentage)
    {
        _percentage = percentage;
    }

    public double GetDiscount(double price)
    {
        return price * _percentage;
    }
}

// Concrete Strategy B
public class FixedDiscountStrategy : IDiscountStrategy
{
    private double _fixedDiscountAmount;

    public FixedDiscountStrategy(double fixedDiscountAmount)
    {
        _fixedDiscountAmount = fixedDiscountAmount;
    }

    public double GetDiscount(double price)
    {
        return _fixedDiscountAmount;
    }
}

public class ShoppingCart
{
    private double _price;
    private readonly TimeSpan from;
    private readonly TimeSpan to;

    public DateTime OrderDate { get; set; }

    private IDiscountStrategy strategy;

    public void SetDiscountStrategy(IDiscountStrategy strategy)
    {
        this.strategy = strategy;
    }

    public ShoppingCart(double price, TimeSpan from, TimeSpan to)
    {
        _price = price;
        this.from = from;
        this.to = to;
    }

    // Obliczanie ceny na podstawie zniżki
    public double CalculateTotalPrice()
    {
        // Happy Hours
        if (OrderDate.TimeOfDay >= from && OrderDate.TimeOfDay <= to)
        {
            return _price - strategy.GetDiscount(_price);
        }
        else
        {
            // No Discount
            return _price; // Brak zniżki
        }
    }
}