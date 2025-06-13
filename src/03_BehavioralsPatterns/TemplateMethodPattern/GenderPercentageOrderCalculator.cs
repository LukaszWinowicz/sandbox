namespace TemplateMethodPattern;

public interface IOrderCalculator
{
    decimal CalculateDiscount(Order order);
}


public abstract class PercentageOrderCalculator(decimal percentage) : OrderCalculator
{
    public override decimal GetDiscount(Order order) => order.Amount * percentage;
}

// Stała zniżka
public abstract class FixedOrderCalculator(decimal amount) : OrderCalculator
{
    public override decimal GetDiscount(Order order) => amount;
}

// Template 
public abstract class OrderCalculator
{
    public abstract bool CanDiscount(Order order);
    public abstract decimal GetDiscount(Order order);
    public decimal NoDiscount => 0;

    // Template Method
    public decimal CalculateDiscount(Order order)
    {
        if (CanDiscount(order)) // Warunek (Predykat)
        {
            return GetDiscount(order); // Upust (procentowy)
        }
        else
            return NoDiscount; // Brak upustu
    }
}


// Gender - 20% upustu dla kobiet
public class GenderPercentageOrderCalculator : PercentageOrderCalculator, IOrderCalculator
{
    private readonly Gender gender;

    public GenderPercentageOrderCalculator(Gender gender, decimal percentage)
        : base(percentage)
    {
        this.gender = gender;
    }

    public override bool CanDiscount(Order order) => order.Customer.Gender == gender;
}

// Problem: kopiujemy logikę CanDiscount. Lepiej zastosować w tym przypadku most
public class GenderFixedOrderCalculator : FixedOrderCalculator, IOrderCalculator
{
    private readonly Gender gender;

    public GenderFixedOrderCalculator(Gender gender, decimal percentage)
        : base(percentage)
    {
        this.gender = gender;
    }

    public override bool CanDiscount(Order order) => order.Customer.Gender == gender;

}