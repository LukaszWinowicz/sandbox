using System;

namespace TemplateMethodPattern;

public class PeriodPercentageOrderCalculator : PercentageOrderCalculator, IOrderCalculator
{
    private readonly DateTime from;
    private readonly DateTime to;

    public PeriodPercentageOrderCalculator(DateTime from, DateTime to, decimal percentage)
        : base(percentage)
    {
        this.from = from;
        this.to = to;
    }

    public override bool CanDiscount(Order order) => order.OrderDate >= from && order.OrderDate <= to;
}
