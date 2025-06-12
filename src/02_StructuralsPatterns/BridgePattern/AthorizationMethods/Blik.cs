using System;

namespace BridgePattern.AthorizationMethods;

// Concrete Implementor
public class Blik : IAthorizationMethod
{
    public void Authorize()
    {
        Console.WriteLine("Autoryzacja za pomocą kodu BLIK");
    }
}

