using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace TemplateMethodPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Template Method Pattern!");

            var from = TimeSpan.Parse("09:00");
            var to = TimeSpan.Parse("15:00");

            Order order = new Order(DateTime.Parse("2020-06-12 14:59"), new Customer("Anna", "Kowalska", Gender.Female), 100);

            HappyHoursPercentageOrderCalculator calculator = new HappyHoursPercentageOrderCalculator(from, to, 0.1m);
            decimal discount = calculator.CalculateDiscount(order);

            Console.WriteLine($"Original amount: {order.Amount:C2} Discount: {discount:C2}");
        }
    }

#region Models

    

    #endregion

}
