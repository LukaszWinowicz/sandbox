using System;

namespace BridgePattern.AthorizationMethods;

public class Face : IAthorizationMethod
{
    public void Authorize()
    {
        Console.WriteLine("Autoryzacja za pomocą twarzy");
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