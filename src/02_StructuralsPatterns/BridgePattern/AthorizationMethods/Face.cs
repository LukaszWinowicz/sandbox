using System;

namespace BridgePattern.AthorizationMethods;

// Concrete Implementor
public class Face : IAthorizationMethod
{
    public void Authorize()
    {
        Console.WriteLine("Autoryzacja za pomocą twarzy");
    }
}

