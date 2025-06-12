namespace CompositePattern;

public interface ICustomerValidator
{
    bool Validate(Customer customer);
}

// Composite
public class CompositeCustomerValidator(params ICustomerValidator[] customerValidators) : ICustomerValidator
{
    public bool Validate(Customer customer) => customerValidators.All(v => v.Validate(customer));
}