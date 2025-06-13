using System;

namespace StatePattern;


public class LightSwitch
{
    public ILightSwitchState State { get; set; }

    public LightSwitch()
    {
        State = new Off(this);
    }

    public void Push()
    {
        State.Push();
    }
}

// Abstract State
public interface ILightSwitchState
{
    void Push();
}

// Concrete State A
public class On : ILightSwitchState
{
    private readonly LightSwitch lightSwitch;

    public On(LightSwitch lightSwitch)
    {
        this.lightSwitch = lightSwitch;
    }

    public void Push()
    {
        Console.WriteLine("wyłącz przekaźnik");
        
        lightSwitch.State = new Off(lightSwitch);
    }
}

// Concrete State B
public class Off : ILightSwitchState
{
    private readonly LightSwitch lightSwitch;

    public Off(LightSwitch lightSwitch)
    {
        this.lightSwitch = lightSwitch;
    }

    public void Push()
    {
        Console.WriteLine("załącz przekaźnik");

        lightSwitch.State = new On(lightSwitch);
    }
}