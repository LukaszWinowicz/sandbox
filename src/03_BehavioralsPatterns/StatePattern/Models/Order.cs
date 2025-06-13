using System;
using System.Diagnostics.Tracing;

namespace StatePattern
{
    public class Order
    {

        public Order(OrderStatus initialState = OrderStatus.Placement)
        {            
            Id = Guid.NewGuid();
            OrderDate = DateTime.Now;
            Status = initialState;
        }

        public Guid Id { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; private set; }
        public bool IsPaid { get; private set; }       

        public void Paid()
        {
            IsPaid = true;
        }

        public void Confirm()
        {
            if (Status == OrderStatus.Placement)
            {
                if (IsPaid)
                {
                    Status = OrderStatus.Picking;
                }                
            }
            else if (Status == OrderStatus.Picking)
            {
                Status = OrderStatus.Shipping;
            }
            else if (Status == OrderStatus.Shipping)
            {
                Status = OrderStatus.Delivered;

                Console.WriteLine("You order was delivered.");
            }
            else if (Status == OrderStatus.Delivered)
            {
                Status = OrderStatus.Completed;
            }
            else
            {
                throw new InvalidOperationException();
            }

        }

        public void Cancel()
        {
            if (Status == OrderStatus.Placement)
            {
                Status = OrderStatus.Canceled;
            }
            else if (Status == OrderStatus.Delivered)
            {
                Status = OrderStatus.Canceled;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public override string ToString() => $"Order {Id} created on {OrderDate}{Environment.NewLine}";

        public virtual bool CanConfirm => Status == OrderStatus.Placement || Status == OrderStatus.Shipping || Status == OrderStatus.Delivered;
        public virtual bool CanCancel => Status == OrderStatus.Placement || Status == OrderStatus.Delivered;
    }

    public enum OrderStatus
    {
        // The customer places an order on the company's website
        Placement,
        // The items from the order are picked from the inventory
        Picking,
        // The package is handed over to the shipping carrier or courier for delivery to the customer.      
        Shipping,
        // The package has been successfully delivered to the customer's specified address.
        Delivered,
        // The order has been successfully delivered, and the transaction is considered complete.
        Completed,
        // The customer canceled order
        Canceled

    }

}
