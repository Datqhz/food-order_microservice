using FluentAssertions;
using OrderService.Data.Requests;
using OrderService.Features.Commands.OrderDetailCommands.CreateOrderDetail;

namespace OrderService.Test.Features.Commands.CreateOrderDetail;

[TestFixture]
public class CreateOrderDetailValidatorTests
{
    private readonly CreateOrderDetailValidator _validator;
    private CreateOrderDetailInput _validInput;

    public CreateOrderDetailValidatorTests()
    {
        _validator = new CreateOrderDetailValidator();
    }

    [SetUp]
    public void Setup()
    {
        _validInput = new CreateOrderDetailInput
        {
            OrderId = 1,
            FoodId = 1,
            Price = 10000,
            Quantity = 10
        };
    }

    #region Setup Test Cases

    private static IEnumerable<TestCaseData> InvalidOrderIdCases()
    {
        yield return new TestCaseData()
            .SetName("OrderId cannot be null");
    }
    private static IEnumerable<TestCaseData> InvalidFoodIdCases()
    {
        yield return new TestCaseData()
            .SetName("FoodId cannot be null");
    }
    private static IEnumerable<TestCaseData> InvalidPriceCases()
    {
        yield return new TestCaseData(null)
            .SetName("Price cannot be null");
        yield return new TestCaseData((decimal)-1)
            .SetName("Price must be greater than or equal to 0");
    }
    
    private static IEnumerable<TestCaseData> InvalidQuantityCases()
    {
        yield return new TestCaseData(null)
            .SetName("Quantity cannot be null");
        yield return new TestCaseData(0)
            .SetName("Quantity must be greater than 0");
    }

    #endregion
    
    #region Setup Tests

    [Test]
    public void Validate_ShouldBeValid_WhenGivenValidInput()
    {
        var command = new CreateOrderDetailCommand
        {
            Payload = _validInput,
        };
        var actual = _validator.Validate(command);
        
        actual.IsValid.Should().BeTrue();
    }
    
    
    [Test, TestCaseSource(nameof(InvalidOrderIdCases))]
    public void Validate_ShouldBeInValid_WhenGivenInvalidOrderId(int invalidOrderId)
    {
        Console.WriteLine(invalidOrderId);
        var command = new CreateOrderDetailCommand
        {
            Payload = new CreateOrderDetailInput
            {
                OrderId = invalidOrderId,
                FoodId = _validInput.FoodId,
                Price = _validInput.Price,
                Quantity = _validInput.Quantity
            },
        };
        var actual = _validator.Validate(command);
        
        actual.IsValid.Should().BeFalse();
    }
    [Test, TestCaseSource(nameof(InvalidFoodIdCases))]
    public void Validate_ShouldBeInValid_WhenGivenInvalidFoodId(int invalidFoodId)
    {
        Console.WriteLine(invalidFoodId);
        var command = new CreateOrderDetailCommand
        {
            Payload = new CreateOrderDetailInput
            {
                OrderId = _validInput.OrderId,
                FoodId = invalidFoodId,
                Price = _validInput.Price,
                Quantity = _validInput.Quantity
            },
        };
        var actual = _validator.Validate(command);
        
        actual.IsValid.Should().BeFalse();
    }

    [Test, TestCaseSource(nameof(InvalidPriceCases))]
    public void Validate_ShouldBeInValid_WhenGivenInvalidPrice(int invalidPrice)
    {
        Console.WriteLine(invalidPrice);
        var command = new CreateOrderDetailCommand
        {
            Payload = new CreateOrderDetailInput
            {
                OrderId = _validInput.OrderId,
                FoodId = _validInput.FoodId,
                Price = invalidPrice,
                Quantity = _validInput.Quantity
            },
        };
        var actual = _validator.Validate(command);
        
        actual.IsValid.Should().BeFalse();
    }
    
    [Test, TestCaseSource(nameof(InvalidQuantityCases))]
    public void Validate_ShouldBeInValid_WhenGivenInvalidQuantity(int invalidQuantity)
    {
        var command = new CreateOrderDetailCommand
        {
            Payload = new CreateOrderDetailInput
            {
                OrderId = _validInput.OrderId,
                FoodId = _validInput.FoodId,
                Price = _validInput.Price,
                Quantity = invalidQuantity
            },
        };
        var actual = _validator.Validate(command);
        
        actual.IsValid.Should().BeFalse();
    }
    #endregion
}
