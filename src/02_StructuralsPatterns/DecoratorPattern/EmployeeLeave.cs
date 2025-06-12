using System;

namespace DecoratorPattern;

// Abstract Component

public interface ILeaveCalculator
{
    int CalculateLeaveDays();
}

// Concrete Component A
public class BaseLeave : ILeaveCalculator
{
    public int BaseLeaveDays { get; private set; }

    public BaseLeave(int baseLeaveDays)
    {
        BaseLeaveDays = baseLeaveDays;
    }

    public int CalculateLeaveDays()
    {
        return BaseLeaveDays;
    }
}

// Abstract Decorator
public abstract class LeaveDecorator : ILeaveCalculator
{
    // Decorated
    protected ILeaveCalculator leaveCalculator;

    protected LeaveDecorator(ILeaveCalculator leaveCalculator)
    {
        this.leaveCalculator = leaveCalculator;
    }

    public abstract int CalculateLeaveDays();
}


// Concrete Decorator A
public class SeniorityYearsDecorator : LeaveDecorator
{
    public int SeniorityYears { get; private set; }

    public SeniorityYearsDecorator(ILeaveCalculator leaveCalculator, int seniorityYears) : base(leaveCalculator)
    {
        SeniorityYears = seniorityYears;
    }

    public override int CalculateLeaveDays()
    {
        // Dodanie dni za staż pracy
        if (SeniorityYears >= 5)
        {
            return leaveCalculator.CalculateLeaveDays() + 5;
        }

        return leaveCalculator.CalculateLeaveDays();
    }
}

// Concrete Decorator B
public class CompletedTrainingDecorator : LeaveDecorator
{
    public bool HasCompletedTraining { get; private set; }

    public CompletedTrainingDecorator(ILeaveCalculator leaveCalculator, bool hasCompletedTraining) : base(leaveCalculator)
    {
        HasCompletedTraining = hasCompletedTraining;
    }

    public override int CalculateLeaveDays()
    {
        // Dodanie dni za ukończone szkolenia
        if (HasCompletedTraining)
        {
            return leaveCalculator.CalculateLeaveDays() + 3;
        }

        return leaveCalculator.CalculateLeaveDays();
    }
}


// Concrete Decorator C
public class SpecialBenefitsDecorator : LeaveDecorator
{
    public bool HasSpecialBenefits { get; private set; }

    public SpecialBenefitsDecorator(ILeaveCalculator leaveCalculator, bool hasSpecialBenefits) : base(leaveCalculator)
    {
        HasSpecialBenefits = hasSpecialBenefits;
    }

    public override int CalculateLeaveDays()
    {
        // Dodanie dni za specjalne świadczenia
        if (HasSpecialBenefits)
        {
            return leaveCalculator.CalculateLeaveDays() + 2;
        }

        return leaveCalculator.CalculateLeaveDays();
    }
}



// Urlop pracowniczy
public class EmployeeLeave
{
    
    public EmployeeLeave(int baseLeaveDays, int seniorityYears, bool hasCompletedTraining, bool hasSpecialBenefits)
    {
    }

    // Oblicza ilość dni urlopu
    public int CalculateLeaveDays()
    {
        throw new NotImplementedException();
    }

}
