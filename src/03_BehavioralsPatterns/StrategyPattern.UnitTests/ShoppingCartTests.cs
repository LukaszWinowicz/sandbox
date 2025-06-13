using System;
using Xunit;

namespace StrategyPattern.UnitTests;

public class ShoppingCartTests
{
    [Theory]
    [InlineData(100.0, "14:00", "16:00", "15:00", 90.0)] // Happy Hours
    [InlineData(200.0, "08:00", "10:00", "09:30", 180.0)] // Happy Hours
    public void CalculateTotalPrice_HappyHours_DiscountApplied(
        double price, string fromTime, string toTime, string orderTime, double expectedPrice)
    {
        // Arrange
        TimeSpan from = TimeSpan.Parse(fromTime);
        TimeSpan to = TimeSpan.Parse(toTime);
        DateTime specialDate = DateTime.Today; // Irrelevant for Happy Hours
        DateTime orderDate = DateTime.Today.Add(TimeSpan.Parse(orderTime));

        var cart = new ShoppingCart(price)
        {
            OrderDate = orderDate
        };

        var calculator = new HappyHoursPriceCalculator(from, to);
        calculator.SetDiscountStrategy(new PercentageDiscountStrategy(0.1));

        // Act
        double result = calculator.CalculateTotalPrice(cart);

        // Assert
        Assert.Equal(expectedPrice, result, 2); // Assert with precision
    }

    

    [Theory]
    [InlineData(100.0, "14:00", "16:00", "17:00", "2023-11-24", 100.0)] // No Discount
    [InlineData(200.0, "08:00", "10:00", "07:30", "2023-12-25", 200.0)] // No Discount
    public void CalculateTotalPrice_NoDiscount_FullPriceReturned(
        double price, string fromTime, string toTime, string orderTime, string specialDate, double expectedPrice)
    {
        // Arrange
        TimeSpan from = TimeSpan.Parse(fromTime);
        TimeSpan to = TimeSpan.Parse(toTime);
        DateTime blackFriday = DateTime.Parse(specialDate);
        DateTime order = DateTime.Today.Add(TimeSpan.Parse(orderTime));

        var cart = new ShoppingCart(price)
        {
            OrderDate = order
        };

        var calculator = new HappyHoursPriceCalculator(from, to);

        // Act
        double result = calculator.CalculateTotalPrice(cart);

        // Assert
        Assert.Equal(expectedPrice, result, 2);
    }
}