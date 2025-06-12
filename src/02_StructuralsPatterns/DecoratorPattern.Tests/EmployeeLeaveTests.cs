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

public class EmployeeLeaveTests
{
    [Fact]
    public void CalculateLeaveDays_WhenNoAdditionalConditions_ShouldReturnBaseLeave()
    {
        // Arrange
        var employeeLeave = new EmployeeLeave(baseLeaveDays: 20, seniorityYears: 2, hasCompletedTraining: false, hasSpecialBenefits: false);

        // Act
        int result = employeeLeave.CalculateLeaveDays();

        // Assert
        Assert.Equal(20, result);
    }

    [Fact]
    public void CalculateLeaveDays_ForSeniorityOf5OrMoreYears_ShouldAdd5Days()
    {
        // Arrange
        var employeeLeave = new EmployeeLeave(baseLeaveDays: 20, seniorityYears: 5, hasCompletedTraining: false, hasSpecialBenefits: false);

        // Act
        int result = employeeLeave.CalculateLeaveDays();

        // Assert
        Assert.Equal(25, result);
    }

    [Fact]
    public void CalculateLeaveDays_ForCompletedTraining_ShouldAdd3Days()
    {
        // Arrange
        var employeeLeave = new EmployeeLeave(baseLeaveDays: 20, seniorityYears: 2, hasCompletedTraining: true, hasSpecialBenefits: false);

        // Act
        int result = employeeLeave.CalculateLeaveDays();

        // Assert
        Assert.Equal(23, result);
    }

    [Fact]
    public void CalculateLeaveDays_ForSpecialBenefits_ShouldAdd2Days()
    {
        // Arrange
        var employeeLeave = new EmployeeLeave(baseLeaveDays: 20, seniorityYears: 2, hasCompletedTraining: false, hasSpecialBenefits: true);

        // Act
        int result = employeeLeave.CalculateLeaveDays();

        // Assert
        Assert.Equal(22, result);
    }

    [Fact]
    public void CalculateLeaveDays_ForAllConditionsMet_ShouldReturn30()
    {
        // Arrange
        var employeeLeave = new EmployeeLeave(baseLeaveDays: 20, seniorityYears: 5, hasCompletedTraining: true, hasSpecialBenefits: true);

        // Act
        int result = employeeLeave.CalculateLeaveDays();

        // Assert
        Assert.Equal(30, result);
    }

    [Theory]
    [InlineData(20, 4, false, false, 20)] // Bez dodatkowych warunków
    [InlineData(20, 5, false, false, 25)] // Dodatkowe dni za staż
    [InlineData(20, 5, true, false, 28)] // Staż + szkolenie
    [InlineData(20, 5, true, true, 30)] // Wszystkie warunki spełnione
    [InlineData(20, 2, true, true, 25)] // Szkolenie + świadczenia, brak stażu
    public void CalculateLeaveDays_ShouldReturnExpectedDays(int baseDays, int seniority, bool training, bool benefits, int expected)
    {
        // Arrange
        var employeeLeave = new EmployeeLeave(baseDays, seniority, training, benefits);

        // Act
        int result = employeeLeave.CalculateLeaveDays();

        // Assert
        Assert.Equal(expected, result);
    }


}
