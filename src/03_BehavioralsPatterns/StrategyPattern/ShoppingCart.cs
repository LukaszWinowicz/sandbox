
using System;

namespace StrategyPattern;


// Abstract Strategy
public interface IDiscountStrategy
{
    double GetDiscount(double price);
}

public interface ICanDiscountStrategy
{
    bool CanDiscount(ShoppingCart cart);
}

public class HappyHoursCanDiscountStrategy : ICanDiscountStrategy
{
    private readonly TimeSpan from;
    private readonly TimeSpan to;

    public HappyHoursCanDiscountStrategy(TimeSpan from, TimeSpan to)
    {
        this.from = from;
        this.to = to;
    }

    public bool CanDiscount(ShoppingCart cart) => cart.OrderDate.TimeOfDay >= from && cart.OrderDate.TimeOfDay <= to;
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
    public double price;   

    public DateTime OrderDate { get; set; }

    public ShoppingCart(double price)
    {
        this.price = price;
    }
}

public class PriceCalculator
{
    private IDiscountStrategy strategy;
    private ICanDiscountStrategy canDiscountStrategy;

    public void SetDiscountStrategy(
        IDiscountStrategy strategy, 
        ICanDiscountStrategy canDiscountStrategy)
    {
        this.strategy = strategy;
        this.canDiscountStrategy = canDiscountStrategy;
    }

    public double CalculateTotalPrice(ShoppingCart cart)
    {
        if (canDiscountStrategy.CanDiscount(cart))
        {
            return cart.price - strategy.GetDiscount(cart.price);
        }
        else
        {
            // No Discount
            return cart.price; // Brak zniżki
        }
    }
}