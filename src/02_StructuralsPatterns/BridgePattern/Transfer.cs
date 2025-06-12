using System;

namespace BridgePattern;

// Abstraction
public interface ITransfer
{
    void MakeTransfer(decimal amount);
}


// Abstraction 1
public abstract class Transfer : ITransfer
{
    // Implementor
    private readonly IAthorizationMethod athorizationMethod;

    protected Transfer(IAthorizationMethod athorizationMethod)
    {
        this.athorizationMethod = athorizationMethod;
    }

    // Operation
    public virtual void MakeTransfer(decimal amount)
    {
        athorizationMethod.Authorize();        
    }
}

