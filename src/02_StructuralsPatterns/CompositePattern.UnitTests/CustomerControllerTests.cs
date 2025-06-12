using Moq;

namespace CompositePattern.UnitTests;

public class CustomerControllerTests
{
    [Fact]
    public void Post_AllValid_ShouldReturnCreatedResult()
    {
        // Arrange
        ICustomerValidator validator = new CompositeCustomerValidator(new TaxNumberValidator(), new RegonValidator());
        var controller = new CustomerController(validator);

        var customer = new Customer { Regon = "123456789", TaxNumber = "12345678901" };

        // Act
        var result = controller.Post(customer);

        // Assert
        Assert.IsType<CreatedResult>(result);

    }


    [Fact]
    public void Post_AnyInvalid_ShouldReturnBadRequestObjectResult()
    {
        ICustomerValidator validator = new CompositeCustomerValidator(new TaxNumberValidator(), new RegonValidator());
        var controller = new CustomerController(validator);

        var customer = new Customer { Regon = "123456789", TaxNumber = "1234567890" };

        // Act
        var result = controller.Post(customer);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }
}


