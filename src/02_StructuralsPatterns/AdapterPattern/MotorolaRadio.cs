using System;
using System.Linq;

namespace AdapterPattern;

// Abstract Adapter
public interface IRadioAdapter
{
    void SendMessage(Message message, byte channel);
}

public class Message
{
    public string Title { get; set; }
    public string Content { get; set; }
}


// Concrete Adapter A
// Wariant obiektowy
public class MotorolaRadioAdapter : IRadioAdapter
{
    // Adaptee
    private MotorolaRadio radio = new MotorolaRadio();

    public void SendMessage(Message message, byte channel)
    {
        radio.PowerOn("1234");
        radio.SelectChannel(channel);
        radio.Send($"{message.Title} - {message.Content}");
        radio.PowerOff();
    }
}

// Concrete Adapter B
// Wariant obiektowy
public class HyteraRadioAdapter : IRadioAdapter
{
    private HyteraRadio radio;

    public HyteraRadioAdapter(HyteraRadio radio)
    {
        this.radio = radio;
    }

    public void SendMessage(Message message, byte channel)
    {
        radio.Init();
        radio.SendMessage(channel, $"{message.Title} - {message.Content}");
        radio.Release();
    }
}

// Concrete Adapter C
// wariant klasowy
public class PanasonicRadioAdapter : PanasonicRadio, IRadioAdapter
{
    public void SendMessage(Message message, byte channel)
    {
        base.On();

        base.Send(message.Title, message.Content, channel);


        base.Off();
    }

    public new string Encrypt(string content)
    {
        Console.WriteLine($"Send to me: {content}");

        return base.Encrypt(content);
    }
}


// sealder - klasa zaczepiętowana - nie można po niej dziedziczyć
public class PanasonicRadio
{
    private bool enabled = false;

    public string Encrypt(string content)
    {
        return content.ToUpperInvariant();
    }

    public void On()
    {
        enabled = true;
    }

    public void Send(string title, string content, byte channel)
    {
        if (enabled)
            Console.WriteLine($"json: title: {title} content: {Encrypt(content)} channel: {channel}");
        else
            throw new InvalidOperationException();
    }

    public void Off()
    {
        enabled = false;
    }
}



public class MotorolaRadio
{
    private bool enabled;

    private byte? selectedChannel;

    public MotorolaRadio()
    {
        enabled = false;
    }

    public void PowerOn(string pincode)
    {
        if (pincode == "1234")
        {
            enabled = true;
        }
    }

    public void SelectChannel(byte channel)
    {
        this.selectedChannel = channel;
    }

    public void Send(string message)
    {
        if (enabled && selectedChannel!=null)
        {
            Console.WriteLine($"<Xml><Send Channel={selectedChannel}><Message>{message}</Message></xml>");
        }
    }

    public void PowerOff()
    {
        enabled = false;
    }



}

// Fabryka
public class RadioFactory
{
    public IRadioAdapter Create(string producer) => producer switch
    {
        "M" => new MotorolaRadioAdapter(),
        "H" => new HyteraRadioAdapter(new HyteraRadio()),
        "P" => new PanasonicRadioAdapter(),
        _ => throw new NotSupportedException(),
    };
}
