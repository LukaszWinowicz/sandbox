using Xunit;

namespace DecoratorPattern.UnitTests;

public class LeaveCalculatorTests
{

    [Fact]
    public void CalculateLeaveDays_SeniorityYearsBelow5_ShouldReturns20Days()
    {
        // Arrange
        ILeaveCalculator calculator = new SeniorityYearsDecorator(
                                        new BaseLeave(20), 2);

        // Act
        var result = calculator.CalculateLeaveDays();

        // Assert
        Assert.Equal(20, result );
    }

    [Fact]
    public void CalculateLeaveDays_SeniorityYearsAbove5_ShouldReturns25Days()
    {
        // Arrange
        ILeaveCalculator calculator = new SeniorityYearsDecorator(
                                        new BaseLeave(20), 6);

        // Act
        var result = calculator.CalculateLeaveDays();

        // Assert
        Assert.Equal(25, result);
    }

    [Fact]
    public void CalculateLeaveDays_SeniorityYearsAbove5AndCompletedTraining_ShouldReturns28Days()
    {
        // Arrange
        ILeaveCalculator calculator = new CompletedTrainingDecorator(
                                        new SeniorityYearsDecorator(
                                            new BaseLeave(20), 6), true);

        // Act
        var result = calculator.CalculateLeaveDays();

        // Assert
        Assert.Equal(28, result);
    }

    [Fact]
    public void CalculateLeaveDays_SeniorityYearsAbove5AndNotCompletedTraining_ShouldReturns25Days()
    {
        // Arrange
        ILeaveCalculator calculator = new CompletedTrainingDecorator(
                                        new SeniorityYearsDecorator(
                                            new BaseLeave(20), 6), false);

        // Act
        var result = calculator.CalculateLeaveDays();

        // Assert
        Assert.Equal(25, result);
    }
}
