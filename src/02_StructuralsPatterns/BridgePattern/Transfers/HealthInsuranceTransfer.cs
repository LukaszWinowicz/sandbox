using System;

namespace BridgePattern.Transfers;

// Refined Abstraction
public class HealthInsuranceTransfer : Transfer
{
    public HealthInsuranceTransfer(IAthorizationMethod athorizationMethod) : base(athorizationMethod)
    {
    }

    public override void MakeTransfer(decimal amount)
    {
        base.MakeTransfer(amount);

        Console.WriteLine($"Przelew składki zdrowotnej {amount}");
    }
}

