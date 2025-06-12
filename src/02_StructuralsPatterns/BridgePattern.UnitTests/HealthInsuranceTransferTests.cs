using BridgePattern.AthorizationMethods;
using BridgePattern.Transfers;
using Xunit;

namespace BridgePattern.UnitTests;



public class HealthInsuranceTransferTests
{
    [Fact]
    public void MakeTransfer_Pin()
    {
        // Arrange
        var transfer = new HealthInsuranceTransfer(new Pin());

        // Act
        transfer.MakeTransfer(100);

        // Assert
    }

    [Fact]
    public void MakeTransfer_Blik()
    {
        // Arrange
        var transfer = new HealthInsuranceTransfer(new Blik());

        // Act
        transfer.MakeTransfer(100);

        // Assert
    }

    [Fact]
    public void MakeTransfer_LoginAndPassword()
    {
        // Arrange
        var transfer = new HealthInsuranceTransfer(new LoginAndPassword());

        // Act
        transfer.MakeTransfer(100);

        // Assert
    }
}


