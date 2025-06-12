using FacadePattern.Models;
using FacadePattern.Repositories;
using FacadePattern.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FacadePattern;

// Abstract Facade
public interface ITicketService
{
    Ticket Buy(RailwayConnectionOptions options);
}

public record RailwayConnectionOptions(string From, string To, DateTime When, byte NumberOfPlaces);

// Concrete Facade A
public class LegacyPkpTicketService : ITicketService
{
    public Ticket Buy(RailwayConnectionOptions options)
    {
        string from = options.From;
        string to = options.To;
        DateTime when = options.When;
        byte numberOfPlaces = options.NumberOfPlaces;

        RailwayConnectionRepository railwayConnectionRepository = new RailwayConnectionRepository();
        TicketCalculator ticketCalculator = new TicketCalculator();
        ReservationService reservationService = new ReservationService();
        PaymentService paymentService = new PaymentService();
        EmailService emailService = new EmailService();

        // Act
        RailwayConnection railwayConnection = railwayConnectionRepository.Find(from, to, when);
        decimal price = ticketCalculator.Calculate(railwayConnection, numberOfPlaces);
        Reservation reservation = reservationService.MakeReservation(railwayConnection, numberOfPlaces);
        Ticket ticket = new Ticket { RailwayConnection = reservation.RailwayConnection, NumberOfPlaces = reservation.NumberOfPlaces, Price = price };
        Payment payment = paymentService.CreateActivePayment(ticket);

        if (payment.IsPaid)
        {
            emailService.Send(ticket);
        }

        return ticket;
    }
}

// Concrete Facade B
// Director
public class TicketServiceDirector(ITicketBuilder builder) : ITicketService
{
    public Ticket Buy(RailwayConnectionOptions options)
    {
        builder.Find(options);
        builder.CalculatePrice();
        builder.MakeReservation();

        return builder.Build();
    }
}

// Abstract Builder
public interface ITicketBuilder
{
    void Find(RailwayConnectionOptions options);
    void CalculatePrice();
    void MakeReservation();
    Ticket Build();
}


// Concrete Builder
public class PkpTicketBuilder(
    RailwayConnectionRepository railwayConnectionRepository, 
    TicketCalculator ticketCalculator, 
    ReservationService reservationService) : ITicketBuilder
{
    private RailwayConnection railwayConnection;
    private RailwayConnectionOptions options;

    private Ticket ticket = new Ticket();

    public void Find(RailwayConnectionOptions options)
    {
        this.options = options;

        railwayConnection = railwayConnectionRepository.Find(options.From, options.To, options.When);
    }

    public void CalculatePrice()
    {
        ticket.Price = ticketCalculator.Calculate(railwayConnection, options.NumberOfPlaces);
    }

    public void MakeReservation()
    {
        reservationService.MakeReservation(railwayConnection, options.NumberOfPlaces);
        ticket.RailwayConnection = railwayConnection;
        ticket.NumberOfPlaces = options.NumberOfPlaces;
    }

    public Ticket Build()
    {
        return ticket;
    }
}