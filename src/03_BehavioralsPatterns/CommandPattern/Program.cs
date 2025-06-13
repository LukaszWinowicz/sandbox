using System;
using System.Collections.Generic;

namespace CommandPattern;


public class CommandFactory
{
    private readonly IDictionary<string, ICommand> commmands = new Dictionary<string, ICommand>();

    private Message message;

    public CommandFactory()
    {
        commmands.Add("print", new PrintCommand(message));
        commmands.Add("send", new PrintCommand(message));
        commmands.Add("save", new SaveCommand(message, "plik"));
    }

    public ICommand Create(string arg)
    {
        return commmands[arg];
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello Command Pattern!");

        CommandFactory factory = new CommandFactory();

        ICommand command = factory.Create(args[0]);


        QueueCommandTest();

    }

    private static void QueueCommandTest()
    {
        Message message = new Message("555000123", "555888000", "Hello World!");

        Queue<ICommand> queue = new Queue<ICommand>();

        queue.Enqueue(new PrintCommand(message));
        queue.Enqueue(new SendCommand(message));
        queue.Enqueue(new SendCommand(message));
        queue.Enqueue(new SaveCommand(message, "message.txt"));

        while (queue.Count > 0)
        {
            ICommand command = queue.Dequeue();

            command.Execute();
        }
    }
}
