using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompositePattern;

public interface IInvestment
{
    string Name { get; }
    decimal GetEstimatedCost();
    void Print(int indent = 0);
}


// Leaf: SimpleInvestment
public class SimpleInvestment : IInvestment
{
    public string Name { get; }
    public decimal Cost { get; }

    public SimpleInvestment(string name, decimal cost)
    {
        Name = name;
        Cost = cost;
    }

    public decimal GetEstimatedCost() => Cost;

    public void Print(int indent = 0)
    {
        Console.WriteLine($"{new string(' ', indent)}- {Name}: {Cost:C}");
    }
}



// Composite: InvestmentPlan
public class InvestmentPlan : IInvestment
{
    public string Name { get; }
    private readonly List<IInvestment> _items = new();

    public InvestmentPlan(string name) => Name = name;

    public void Add(IInvestment investment) => _items.Add(investment);

    public decimal GetEstimatedCost() =>
        _items.Sum(i => i.GetEstimatedCost());

    public void Print(int indent = 0)
    {
        Console.WriteLine($"{new string(' ', indent)}+ {Name} (Total: {GetEstimatedCost():C})");
        foreach (var item in _items)
        {
            item.Print(indent + 2);
        }
    }
}
