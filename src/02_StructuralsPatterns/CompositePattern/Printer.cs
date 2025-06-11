namespace CompositePattern;

interface ILogger
{
    void Info(string message);
}

class ConsoleLogger : ILogger
{
    public void Info(string message)
    {
        Console.WriteLine(message);
    }
}

class FileLogger : ILogger
{
    public void Info(string message)
    {
        File.AppendAllText("printer.log", message);
    }
}

class DbLogger : ILogger
{
    public void Info(string message)
    {
        Console.WriteLine($"Save to db {message}");
    }
}

class Printer
{
    private readonly ILogger _logger;

    public Printer(ILogger logger)
    {
        this._logger = logger;
    }

    public void Print(string content, int copies = 1)
    {
        // TODO: dodaj logowanie do wielu loggerów
        _logger.Info($"[{DateTime.UtcNow}] content: {content} copies: {copies}");

        for (int i = 0; i < copies; i++)
        {
            Console.WriteLine($"Printing {content}... copy #{i}");
        }
    }
}
