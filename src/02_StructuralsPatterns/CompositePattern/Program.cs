namespace CompositePattern;

class Program
{
    static void Main(string[] args)
    {
        InvestmentPlanTest();


        // ComposeLoggerTest();

        // DecisionTreeTest();

    }

    private static void InvestmentPlanTest()
    {
        var machine = new SimpleInvestment("CNC Machine", 120_000);
        var software = new SimpleInvestment("CAD License", 12_000);
        var training = new SimpleInvestment("Operator Training", 5_000);

        var modernization = new InvestmentPlan("Production Modernization");
        modernization.Add(machine);
        modernization.Add(software);
        modernization.Add(training);

        var marketing = new SimpleInvestment("Ad Campaign", 25_000);
        var website = new SimpleInvestment("Website Redesign", 8_000);

        var businessGrowth = new InvestmentPlan("Business Growth Plan");
        businessGrowth.Add(modernization);
        businessGrowth.Add(marketing);
        businessGrowth.Add(website);

        businessGrowth.Print();
    }

    private static void ComposeLoggerTest()
    {
        ILogger logger = new CompositeLogger(new ConsoleLogger(), new FileLogger(), new DbLogger());

        Printer printer = new Printer(logger);
        printer.Print("a", 3);
    }

    private static void DecisionTreeTest()
    {
        var notForYou = new Decision("The Course is not for you.", false);
        var welcome = new Decision("Welcome on Design Pattern in C# Course!", true);

        var dpQuestion = new Question("Do you know Design Pattern?", notForYou, welcome);

        var csharpQuestion = new Question("Do you know C#?", dpQuestion, notForYou);
        var developerQuestion = new Question("Are you developer?", csharpQuestion, notForYou);


        developerQuestion.Message();
    }
}

// Abstract Component
public interface IQuestion
{
    void Message();
}


// Concrete Component - Branch
public class Question : IQuestion
{
    private readonly string prompt;
    private readonly IQuestion positive;
    private readonly IQuestion negative;

    public Question(string prompt, IQuestion positive, IQuestion negative)
    {
        this.prompt = prompt;
        this.positive = positive;
        this.negative = negative;
    }

    public void Message()
    {
        Console.Write(prompt);

        if (Response)
        {
            positive.Message();
        }
        else
        {
            negative.Message();
        }
    }

    public static bool Response => Console.ReadKey().Key == ConsoleKey.Y;
}

// Concrete Component - Leaf
public class Decision : IQuestion
{
    private readonly string message;
    private readonly bool positive;

    public Decision(string message, bool positive)
    {
        this.message = message;
        this.positive = positive;
    }

    public void Message()
    {
        Console.BackgroundColor = positive ? ConsoleColor.Green : ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ResetColor();
    }
}

