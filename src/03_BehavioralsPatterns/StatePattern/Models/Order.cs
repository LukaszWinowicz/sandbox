using Stateless;
using System;
using System.Diagnostics.Tracing;
using System.Reflection.PortableExecutable;



namespace StatePattern;

public class OrderStateMachine : StateMachine<OrderStatus, OrderTrigger>
{
    public OrderStateMachine(Order order, OrderStatus initialState = OrderStatus.Placement) : base(initialState)
    {
        this.Configure(OrderStatus.Placement)
            .Permit(OrderTrigger.Confirm, OrderStatus.Picking)
            .Permit(OrderTrigger.Cancel, OrderStatus.Canceled);

        this.Configure(OrderStatus.Picking)
            .PermitIf(OrderTrigger.Confirm, OrderStatus.Shipping, () => order.IsPaid, "Paid") // jesli zapłacono
            .PermitIf(OrderTrigger.Confirm, OrderStatus.Canceled, () => !order.IsPaid, "NotPaid"); // jeśli nie zapłacono

        this.Configure(OrderStatus.Shipping)
            .Permit(OrderTrigger.Confirm, OrderStatus.Delivered);

        this.Configure(OrderStatus.Delivered)
            .OnEntry(() => Console.WriteLine("You order was delivered."))
            .Permit(OrderTrigger.Confirm, OrderStatus.Completed)
            .Permit(OrderTrigger.Cancel, OrderStatus.Canceled);
    }
}

public interface IOrder
{
    OrderStatus Status { get; }
    void Confirm();
    void Cancel();
    bool CanConfirm { get; }
    bool CanCancel { get; }

}

// Proxy
// wariant klasowy
public class OrderProxy : Order, IOrder
{
    private OrderStateMachine machine;

    // dotnet add package Stateless
    public OrderProxy()
    {
        this.machine = new OrderStateMachine(this, OrderStatus.Placement);
    }

    public override OrderStatus Status => machine.State;

    public override void Confirm() => machine.Fire(OrderTrigger.Confirm);

    public override void Cancel() => machine.Fire(OrderTrigger.Cancel);

    public override bool CanConfirm =>  machine.CanFire(OrderTrigger.Confirm);
    public override bool CanCancel => machine.CanFire(OrderTrigger.Cancel);

    public string Graph => Stateless.Graph.MermaidGraph.Format(machine.GetInfo());
}

public class Order : IOrder
{

    public Order(OrderStatus initialState = OrderStatus.Placement)
    {
        Id = Guid.NewGuid();
        OrderDate = DateTime.Now;
    }

    public Guid Id { get; set; }
    public DateTime OrderDate { get; set; }
    public virtual OrderStatus Status { get; set; }
    public bool IsPaid { get; private set; }

    public void Paid()
    {
        IsPaid = true;
    }

    public virtual void Confirm() => throw new NotImplementedException();

    public virtual void Cancel() => throw new NotImplementedException();

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

public enum OrderTrigger
{
    Confirm,
    Cancel,
}
