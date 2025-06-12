using FacadePattern.Models;

namespace FacadePattern.Services;


public interface IEmailService
{
    void Send(Ticket ticket);
}

public class SmtpEmailService : IEmailService
{
    public void Send(Ticket ticket)
    {
        System.Console.WriteLine($"Send {ticket}");
    }
}
