using System;

namespace BridgePattern.AthorizationMethods;

// Concrete Implementor
public class LoginAndPassword : IAthorizationMethod
{
    public void Authorize()
    {
        Console.WriteLine("Autoryzacja za pomocą login i hasła.");
    }
}

