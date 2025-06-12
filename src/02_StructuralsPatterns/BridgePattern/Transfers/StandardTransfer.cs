using System;

namespace BridgePattern.Transfers;

// Refined Abstraction
public class StandardTransfer : Transfer
{
    public StandardTransfer(IAthorizationMethod athorizationMethod) : base(athorizationMethod)
    {
    }

    public override void MakeTransfer(decimal amount)
    {
        base.MakeTransfer(amount);

        Console.WriteLine($"Przelew standardowy {amount}");
    }
}
