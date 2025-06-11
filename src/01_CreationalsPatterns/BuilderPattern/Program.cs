using BuilderPattern.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BuilderPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Builder Pattern!");

            // DelegateTest();

           //  return;

            // Console.WriteLine(Invoice.GetTax());

           //  Invoice invoice = Invoice.Create();

            // PresentationBuilderTest();

            //PhoneTest();

            //  SalesReportTest();

            SalesReportDirectorTest();

            // PersonTest();

            // RoomTest();
        }

        private static void SalesReportDirectorTest()
        {
            var sales = new List<Sale>
            {
                new Sale { Date = DateTime.Today.AddDays(-3), Product = "Woda", Amount = 100 },
                new Sale { Date = DateTime.Today.AddDays(-2), Product = "Sok", Amount = 150 },
                new Sale { Date = DateTime.Today.AddDays(-1), Product = "Woda", Amount = 200 }
            };

            var builder = new MarkdownSalesReportBuilder();
            var director = new SalesReportDirector();

            director.ConstructFull(builder, "Raport sprzedaży - czerwiec", DateTime.Today.AddDays(-7), DateTime.Today, sales);

            var report = builder.GetReport();
            Console.WriteLine(report.Body);

        }

        private static void DelegateTest()
        {
            Printer printer = new Printer();
            printer.Log += LogToConsole;
            printer.Log += LogToFile;
            printer.Log += LogToDb;

            printer.Log += Console.WriteLine;

            printer.Log += (msg) => Console.WriteLine($"Lambda: {msg}");

            printer.Print("Hello, World!", 3);
        }

        private static void LogToDb(string message)
        {
            Console.WriteLine($"Save to db {message}");
        }

        private static void LogToFile(string message)
        {
            File.AppendAllText("printer.log", message);
        }

        private static void LogToConsole(string message)
        {
            Console.WriteLine(message);
        }

        private static void PresentationBuilderTest()
        {
            Presentation presentation = new Presentation();
            presentation.AddSlide(new Slide("a"));
            presentation.AddSlide(new Slide("b"));
            presentation.AddSlide(new Slide("c"));

            MoviePresentationBuilder presentationBuilder = new MoviePresentationBuilder();
            PresentationDirector director = new PresentationDirector(presentationBuilder);

            director.Build(presentation);

            var product = presentationBuilder.Build();
        }

        private static void RoomTest()
        {
            // Opcje konfiguracji pokoju
            var roomWidth = 500;
            var roomHeight = 300;

            // Tworzenie ścian
            var walls = new List<Wall>
            {
                new Wall("Red", 200, 250, WallPosition.North),
                new Wall("Red", 200, 250, WallPosition.South)
            };

            // Tworzenie sufitu
            var ceiling = new Ceiling(roomWidth, roomHeight);

            // Tworzenie pokoju
            var room = new Room(roomWidth, roomHeight, walls, ceiling);


            // Wyświetlenie pokoju

            Console.WriteLine(room);
        }

        private static void PersonTest()
        {
            var person = new Person();
             
            person.Name = "Marcin";
            person.Position = "developer";
            person.AddSkill("C#");
            person.AddSkill("design-patterns");
            person.AddSkill("tdd");

            Console.WriteLine(person);
        }

        private static void SalesReportTest()
        {
            FakeOrdersService ordersService = new FakeOrdersService();
            IEnumerable<Order> orders = ordersService.Get();

            SalesReportBuilder builder = new SalesReportBuilder(orders);
            builder
                .AddHeader("Raport sprzedaży")
                .AddSectionProductDetails()
                .AddSectionGenderDetails(); // Fluent Api

            Models.SalesReport salesReport = builder.Build();

            // Footer

            Console.WriteLine(salesReport);

        }

       

     
       

        private static void PhoneTest()
        {
            Phone phone = new Phone();
            phone.Call("555999123", "555000321", ".NET Design Patterns");
        }

       
    }

    public class FakeOrdersService
    {
        private readonly IList<Product> products;
        private readonly IList<Customer> customers;

        public FakeOrdersService()
            : this(CreateProducts(), CreateCustomers())
        {

        }

        public FakeOrdersService(IList<Product> products, IList<Customer> customers)
        {
            this.products = products;
            this.customers = customers;
        }

        private static IList<Customer> CreateCustomers()
        {
            return new List<Customer>
            {
                 new Customer("Anna", "Kowalska"),
                 new Customer("Jan", "Nowak"),
                 new Customer("John", "Smith"),
            };

        }

        private static IList<Product> CreateProducts()
        {
            return new List<Product>
            {
                new Product(1, "Książka C#", unitPrice: 100m),
                new Product(2, "Książka Praktyczne Wzorce projektowe w C#", unitPrice: 150m),
                new Product(3, "Zakładka do książki", unitPrice: 10m),
            };
        }

        public IEnumerable<Order> Get()
        {
            Order order1 = new Order(DateTime.Parse("2020-06-12 14:59"), customers[0]);
            order1.AddDetail(products[0], 2);
            order1.AddDetail(products[1], 2);
            order1.AddDetail(products[2], 3);

            yield return order1;

            Order order2 = new Order(DateTime.Parse("2020-06-12 14:59"), customers[1]);
            order2.AddDetail(products[0], 2);
            order2.AddDetail(products[1], 4);

            yield return order2;

            Order order3 = new Order(DateTime.Parse("2020-06-12 14:59"), customers[2]);
            order2.AddDetail(products[0], 2);
            order2.AddDetail(products[2], 5);

            yield return order3;


        }
    }




}