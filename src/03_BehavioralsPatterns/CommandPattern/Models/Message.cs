using System;
using System.IO;
using System.Reflection.Metadata;
using System.Threading;
using System.Threading.Tasks;

namespace CommandPattern;

// Abstract Command
public interface ICommand
{
    void Execute();
    bool CanExecute(); // opcjonalnie
}

public class RelayCommand : ICommand
{
    private ICommand execute;
    private ICommand canExecute;

    public RelayCommand(ICommand execute, ICommand canExecute = null)
    {
        this.execute = execute;
        this.canExecute = canExecute;
    }

    public bool CanExecute() => canExecute == null || canExecute.CanExecute();

    public void Execute()
    {
        execute?.Execute();
    }
}


// Concrete Command A

public class SendCommand : ICommand
{
    private readonly Message message;

    public SendCommand(Message message)
    {
        this.message = message;
    }

    public bool CanExecute()
    {
        return true;
    }

    public void Execute()
    {
        Send();
    }

    private void Send()
    {
        Console.WriteLine($"Send message from <{message.From}> to <{message.To}> {message.Content}");
    }
}

// Concrete Command B
public class PrintCommand(Message message) : ICommand
{
    public bool CanExecute()
    {
        return true;
    }

    public void Execute()
    {
        Print();
    }

    private void Print(byte copies = 1)
    {
        for (int i = 0; i < copies; i++)
        {
            Console.WriteLine($"Print message from <{message.From}> to <{message.To}> {message.Content}");
        }
    }
}

// Concrete Command C
public class SaveCommand(Message message, string filename) : ICommand
{
    public bool CanExecute()
    {
        return true;
    }

    public void Execute()
    {
        File.AppendAllText(filename, message.Content);
    }
}

public class Message
{
    public Message(string from, string to, string content)
    {
        From = from;
        To = to;
        Content = content;
    }

    public string From { get; set; }
    public string To { get; set; }
    public string Content { get; set; }

 
   


    public bool CanSend()
    {
        return !(string.IsNullOrEmpty(From) || string.IsNullOrEmpty(To) || string.IsNullOrEmpty(Content));
    }


    

    public bool CanPrint()
    {
        return string.IsNullOrEmpty(Content);
    }



}
