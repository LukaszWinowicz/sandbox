using System;

namespace BridgePattern.AthorizationMethods;

public class Pin : IAthorizationMethod
{
    public void Authorize()
    {
        Console.WriteLine("Autoryzacja za pomocą PIN");
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