using FacadePattern.Models;
using FacadePattern.Repositories;
using FacadePattern.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net.Http.Headers;

namespace FacadePattern.UnitTests
{
    [TestClass]
    public class TicketTests
    {
        [TestMethod]
        public void BuyTicket()
        {
            // Arrange
            string from = "Bydgoszcz";
            string to = "Warszawa";
            DateTime when = DateTime.Parse("2022-07-15");
            byte numberOfPlaces = 3;

            var options = new RailwayConnectionOptions(from, to, when, numberOfPlaces);

            ITicketBuilder ticketBuilder = new PkpTicketBuilder(
                new RailwayConnectionRepository(),
                 new TicketCalculator(), new ReservationService());

            ITicketService ticketService = new TicketServiceDirector(ticketBuilder);

            // Act
            var ticket = ticketService.Buy(options);


            // Assert
            Assert.AreEqual("Bydgoszcz", ticket.RailwayConnection.From);
            Assert.AreEqual("Warszawa", ticket.RailwayConnection.To);
            Assert.AreEqual(DateTime.Parse("2022-07-15"), ticket.RailwayConnection.When);
            Assert.AreEqual(3, ticket.NumberOfPlaces);
        }

        [TestMethod]
        public void CancelTicket()
        {
            // Arrange
            string from = "Bydgoszcz";
            string to = "Warszawa";
            DateTime when = DateTime.Parse("2022-07-15");
            byte numberOfPlaces = 3;

            RailwayConnectionRepository railwayConnectionRepository = new RailwayConnectionRepository();
            TicketCalculator ticketCalculator = new TicketCalculator();
            ReservationService reservationService = new ReservationService();
            PaymentService paymentService = new PaymentService();
            EmailService emailService = new EmailService();

            RailwayConnection railwayConnection = railwayConnectionRepository.Find(from, to, when);
            decimal price = ticketCalculator.Calculate(railwayConnection, numberOfPlaces);
            Reservation reservation = reservationService.MakeReservation(railwayConnection, numberOfPlaces);
            Ticket ticket = new Ticket { RailwayConnection = reservation.RailwayConnection, NumberOfPlaces = reservation.NumberOfPlaces, Price = price };
            Payment payment = paymentService.CreateActivePayment(ticket);

            // Act
            reservationService.CancelReservation(ticket.RailwayConnection, ticket.NumberOfPlaces);
            paymentService.RefundPayment(payment);



        }
    }
}
