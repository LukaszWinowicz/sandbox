
Console.WriteLine("Hello, Legacy Printer!");

IPrinter printer = new CostDecorator( new LegacyPrinterObjectAdapter(), 0.1m);

printer.PrintDocument("a", 3);

// Abstract Component
public interface IPrinter
{
    void PrintDocument(string document, int copies);
}

public class PrinterDecorator : IPrinter
{
    public decimal Cost { get; set; }

    public virtual void PrintDocument(string document, int copies)
    {

    }
}

// Concrete Decorator A
public class CostDecorator : PrinterDecorator, IPrinter
{
    private readonly decimal costPerPage;

    // decoratee
    private readonly IPrinter printer;

    public CostDecorator(IPrinter printer, decimal costPerPage)
    {
        this.printer = printer;
        this.costPerPage = costPerPage;
    }

    public override void PrintDocument(string document, int copies)
    {
        printer.PrintDocument(document, copies);

        Cost = document.Length * copies * costPerPage;

        Console.WriteLine($"Total cost: {Cost}");
    }
}

// Adapter klasowy
// Concrete Component A
public class LegacyPrinterClassAdapter : LegacyPrinter, IPrinter
{
    public void PrintDocument(string document, int copies)
    {
        for (int i = 0; i < copies; i++)
        {
            base.PrintDocument(document); // Real Subject
        }
    }
}

// Adapter obiektowy
public class LegacyPrinterObjectAdapter : IPrinter
{
    private readonly LegacyPrinter legacyPrinter = new LegacyPrinter();

    public void PrintDocument(string document, int copies)
    {
        for (int i = 0; i < copies; i++)
        {
            legacyPrinter.PrintDocument(document); 
        }
    }
}

// Nowa drukarka
// Concrete Component B
public class LaserPrinter : IPrinter
{
    public void PrintDocument(string document, int copies)
    {
        Parallel.For(0, copies, i => Console.WriteLine(document));
    }
}


public class LegacyPrinter // nie możemy jej modyfikować!
{
    public void PrintDocument(string document)
    {
        Console.WriteLine($"Legacy Printer is printing: {document}");
    }
}