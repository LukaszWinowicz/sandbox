using System;

namespace BridgePattern.AthorizationMethods;

public class Blik : IAthorizationMethod
{
    public void Authorize()
    {
        Console.WriteLine("Autoryzacja za pomocą kodu BLIK");
    }
}

