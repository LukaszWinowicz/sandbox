namespace CompositePattern;

public interface ILogger
{
    void Info(string message);
}

public class ConsoleLogger : ILogger
{
    public void Info(string message)
    {
        Console.WriteLine(message);
    }
}

public class FileLogger : ILogger
{
    public void Info(string message)
    {
        File.AppendAllText("printer.log", message);
    }
}

public class DbLogger : ILogger
{
    public void Info(string message)
    {
        Console.WriteLine($"Save to db {message}");
    }
}

public class CompositeLogger : ILogger
{
    private readonly ILogger[] loggers;

    public CompositeLogger(params ILogger[] loggers)
    {
        this.loggers = loggers;
    }

    public void Info(string message)
    {
        foreach (ILogger logger in loggers)
        {
            logger.Info(message);
        }

    }
}

public class Printer
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
