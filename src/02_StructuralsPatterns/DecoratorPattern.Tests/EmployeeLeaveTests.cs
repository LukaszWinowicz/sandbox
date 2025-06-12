using Xunit;

namespace DecoratorPattern.UnitTests;

public class PricingRepositoryTests()
{

    [Fact]
    public void GetPrice_SymbolA_ShouldReturnPrice10()
    {
        // Arrange
        IPricingRepository repository = new FakePricingRepository();

        // Act
        var result = repository.GetPrice("a");

        // Assert
        Assert.Equal(10, result);
    }

    [Fact]
    public void GetPrice_SymbolADiscounted10Percentage_ShouldReturnPrice9()
    {
        // Arrange
        IPricingRepository repository = new CurrencyPricingDecorator(
            new DiscountedPricingDecorator(
                new FakePricingRepository(), 0.1m), 
           currencyService: new FakeCurrencyService() );

        // Act
        var result = repository.GetPrice("a");

        // Assert
        Assert.Equal(36.09m, result);
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
