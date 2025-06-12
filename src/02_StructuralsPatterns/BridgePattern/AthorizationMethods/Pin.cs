using System;

namespace BridgePattern.AthorizationMethods;

// Concrete Implementor
public class Pin : IAthorizationMethod
{
    public void Authorize()
    {
        Console.WriteLine("Autoryzacja za pomocą PIN");
    }
}


