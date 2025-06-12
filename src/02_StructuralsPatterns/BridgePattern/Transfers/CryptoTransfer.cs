using System;

namespace BridgePattern.Transfers;

// Refined Abstraction
public abstract class CryptoTransfer : Transfer
{
    protected CryptoTransfer(IAthorizationMethod athorizationMethod) : base(athorizationMethod)
    {
    }

    public override void MakeTransfer(decimal amount)
    {
        base.MakeTransfer(amount);

        Console.WriteLine("Dodatkowa weryfikacja");
    }
}
