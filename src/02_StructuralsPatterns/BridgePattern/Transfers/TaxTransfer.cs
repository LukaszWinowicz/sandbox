using System;

namespace BridgePattern.Transfers;

public class TaxTransfer : Transfer
{
    public TaxTransfer(IAthorizationMethod athorizationMethod) : base(athorizationMethod)
    {
    }

    public override void MakeTransfer(decimal amount)
    {
        base.MakeTransfer(amount);

        Console.WriteLine($"Przelew podatkowy {amount}");
    }
}



//public abstract class CryptoTransfer : Transfer
//{


//}

//public class BitcoinTransfer : CryptoTransfer
//{

//}

//public class EtheriumTransfer : CryptoTransfer
//{

//}