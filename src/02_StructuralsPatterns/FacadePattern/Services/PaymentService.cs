using FacadePattern.Models;

namespace FacadePattern.Services;

public interface IPaymentService
{
    Payment CreateActivePayment(Ticket ticket);
    void RefundPayment(Payment payment);
}

public class PaymentService : IPaymentService
{
    public Payment CreateActivePayment(Ticket ticket)
    {
        return new Payment { TotalAmount = ticket.Price };
    }

    public void RefundPayment(Payment payment)
    {

    }
}
