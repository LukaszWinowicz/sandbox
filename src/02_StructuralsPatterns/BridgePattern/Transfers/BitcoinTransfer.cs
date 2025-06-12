using System;

namespace BridgePattern.Transfers;

// Refined Abstraction
public class BitcoinTransfer : CryptoTransfer
{
    public BitcoinTransfer(IAthorizationMethod athorizationMethod) : base(athorizationMethod)
    {
    }

    public override void MakeTransfer(decimal amount)
    {
        base.MakeTransfer(amount);

        Console.WriteLine("Przelew bitcoin");
    }
}
