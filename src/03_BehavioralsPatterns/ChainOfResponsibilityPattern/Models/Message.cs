using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;

namespace ChainOfResponsibilityPattern.Models;

// Model danych
public class Message
{
    public string From { get; set; }
    public string Title { get; set; }   
    public string Body { get; set; }
}

// Context
public class MessageContext
{
    // Wejście (Request)
    public Message Message { get; set; }

    // Wyjście
    public MessageResult Result { get; set; } = new MessageResult();

}

public class MessageResult
{
    public string Nip { get; set; }
}

// Abstract Handler
public interface IMessageHandler
{
    void Handle(MessageContext context);
    void SetNext(IMessageHandler next);
}

public abstract class MessageHandler : IMessageHandler
{
    private IMessageHandler _next;
    public void SetNext(IMessageHandler next)
    {
        this._next = next;
    }

    public virtual void Handle(MessageContext context)
    {
        if (_next != null) 
            _next.Handle(context);
    }
}

// Concrete Handler A
public class ValidateFromWhiteListMessageHandler : MessageHandler, IMessageHandler
{
    private string[] whiteList;

    public ValidateFromWhiteListMessageHandler(string[] whiteList)
    {
        this.whiteList = whiteList;
    }

    public override void Handle(MessageContext context)
    {
        ValidateFromWhiteList(context.Message);

        base.Handle(context);
    }

    private void ValidateFromWhiteList(Message message)
    {
        if (!whiteList.Contains(message.From))
        {
            throw new Exception();
        }
    }
}


public class MessageHandlerFactory
{
    public IMessageHandler Create(string messageType = "")
    {
        string[] whiteList = new string[] { "john@domain.com", "bob@domain.com" };

        IMessageHandler exceptionLoggerHandler = new ExceptionLoggerHandler();
        IMessageHandler whitelistHandler = new ValidateFromWhiteListMessageHandler(whiteList);
        IMessageHandler titleHandler = new ValidateTitleContainsOrderMessageHandler();
        IMessageHandler nipHandler = new ExtractNipMessageHandler();
        IMessageHandler dbHandler = new SaveDbMessageHandler(new CustomerRepository());
        IMessageHandler smsHandler = new SendSmsMessageHandler();

        // Tworzymy łańcuch odpowiedzialności
        exceptionLoggerHandler.SetNext(whitelistHandler);
        whitelistHandler.SetNext(titleHandler);
        titleHandler.SetNext(nipHandler);
        nipHandler.SetNext(dbHandler);

        if (messageType == "priority")
        {
            dbHandler.SetNext(smsHandler);
        }

        var firstHandler = exceptionLoggerHandler;

        return firstHandler;
    }
}

// Concrete Handler B
public class ValidateTitleContainsOrderMessageHandler : MessageHandler, IMessageHandler
{
    public override void Handle(MessageContext context)
    {
        ValidateTitleContainsOrder(context.Message);

        base.Handle(context);
    }


    private static void ValidateTitleContainsOrder(Message message)
    {
        if (!message.Title.Contains("Order"))
        {
            throw new Exception();
        }
    }
}

public class SendSmsMessageHandler : MessageHandler, IMessageHandler
{
    public override void Handle(MessageContext context)
    {
        Console.WriteLine($"{context.Message.From} {context.Result.Nip}");

        base.Handle(context);
    }
}

// Concrete Handler C
public class ExtractNipMessageHandler : MessageHandler, IMessageHandler
{
    public override void Handle(MessageContext context)
    {        
        context.Result.Nip = ExtractNip(context.Message);

        base.Handle(context);
    }

    private static string ExtractNip(Message message)
    {
        string pattern = @"\b(\d{10}|\d{3}-\d{3}-\d{2}-\d{2})\b";
        Regex regex = new Regex(pattern);
        Match match = regex.Match(message.Body);

        if (match.Success)
        {
            string taxNumber = match.Value;

            return taxNumber;
        }
        else
        {
            throw new FormatException();
        }
    }
}

public class CustomerRepository
{
    public void Add(string nip)
    {

    }
}

public class ExceptionLoggerHandler : MessageHandler, IMessageHandler
{
    public override void Handle(MessageContext context)
    {
        try
        {
            base.Handle(context);
        }
        catch(Exception e)
        {
            Console.WriteLine($"{DateTime.UtcNow} {e.Message}");

            throw;
        }
    }

}

public class SaveDbMessageHandler : MessageHandler, IMessageHandler
{
    private readonly CustomerRepository repository;

    public SaveDbMessageHandler(CustomerRepository repository)
    {
        this.repository = repository;
    }

    public override void Handle(MessageContext context)
    {
        repository.Add(context.Result.Nip);

        base.Handle(context);
    }
}




public class MessageProcessor
{
    private readonly IMessageHandler firstHandler;

    public MessageProcessor(IMessageHandler firstHandler)
    {
       this.firstHandler = firstHandler;
    }

    public string Process(Message message)
    {
        var context = new MessageContext { Message = message };

        firstHandler.Handle(context);

        return context.Result.Nip;

    }
   
}
