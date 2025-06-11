using CompositePattern;
using Moq;

public class PrinterTests
{
    [Fact]
    public void Print_LogsInfoExactlyOnce()
    {
        // Arrange
        var loggerMock = new Mock<ILogger>();
        var printer = new Printer(loggerMock.Object);

        // Act
        printer.Print("Test page");

        // Assert
        loggerMock.Verify(
            l => l.Info(It.Is<string>(msg => msg.Contains("Test page"))),
            Times.Once);
    }
}
