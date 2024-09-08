using FluentAssertions;
using OrderService.Data.Requests;
using OrderService.Features.Commands.OrderDetailCommands.UpdateOrderDetail;

namespace OrderService.Test.Features.Commands.UpdateOrderDetail;

[TestFixture]
public class UpdateOrderDetailValidatorTests
{
    private readonly UpdateOrderDetailValidator _validator;
    private UpdateOrderDetailInput _validInput;

    public UpdateOrderDetailValidatorTests()
    {
        _validator = new UpdateOrderDetailValidator();
    }

    [SetUp]
    public void Setup()
    {
        _validInput = new UpdateOrderDetailInput
        {
            OrderDetailId = 1,
            Price = 10000,
            Quantity = 10,
            Feature = 1
        };
    }

    #region Setup Test Cases

    /*private static IEnumerable<TestCaseData> InvalidOrderDetailIdCases()
    {
        yield return new TestCaseData(null)
            .SetName("OrderDetailId cannot be null");
    }
    private static IEnumerable<TestCaseData> InvalidFoodIdCases()
    {
        yield return new TestCaseData()
            .SetName("FoodId cannot be null");
    }*/
    private static IEnumerable<TestCaseData> InvalidPriceCases()
    {
        yield return new TestCaseData((decimal)-1)
            .SetName("Price must be greater than or equal to 0");
    }
    
    private static IEnumerable<TestCaseData> InvalidQuantityCases()
    {
        yield return new TestCaseData(0)
            .SetName("Quantity must be greater than 0");
    }

    #endregion
    
    #region Setup Tests

    [Test]
    public void Validate_ShouldBeValid_WhenGivenValidInput()
    {
        var command = new UpdateOrderDetailCommand
        {
            Payload = _validInput,
        };
        var actual = _validator.Validate(command);
        
        actual.IsValid.Should().BeTrue();
    }
    
    
    /*
    [Test, TestCaseSource(nameof(InvalidOrderDetailIdCases))]
    public void Validate_ShouldBeInValid_WhenGivenInvalidOrderId(int invalidOrderDetailId)
    {
        Console.WriteLine(invalidOrderDetailId);
        var command = new UpdateOrderDetailCommand
        {
            Payload = new UpdateOrderDetailInput
            {
                OrderDetailId = invalidOrderDetailId,
                Price = _validInput.Price,
                Quantity = _validInput.Quantity
            },
        };
        var actual = _validator.Validate(command);
        
        actual.IsValid.Should().BeFalse();
    }
    */

    [Test, TestCaseSource(nameof(InvalidPriceCases))]
    public void Validate_ShouldBeInvalid_WhenGivenInvalidPrice(decimal invalidPrice)
    {
        Console.WriteLine(invalidPrice);
        var command = new UpdateOrderDetailCommand
        {
            Payload = new UpdateOrderDetailInput
            {
                OrderDetailId = _validInput.OrderDetailId,
                Price = invalidPrice,
                Quantity = _validInput.Quantity
            },
        };
        var actual = _validator.Validate(command);
        
        actual.IsValid.Should().BeFalse();
    }
    
    [Test, TestCaseSource(nameof(InvalidQuantityCases))]
    public void Validate_ShouldBeInvalid_WhenGivenInvalidQuantity(int invalidQuantity)
    {
        var command = new UpdateOrderDetailCommand
        {
            Payload = new UpdateOrderDetailInput
            {
                OrderDetailId = _validInput.OrderDetailId,
                Price = _validInput.Price,
                Quantity = invalidQuantity
            },
        };
        var actual = _validator.Validate(command);
        
        actual.IsValid.Should().BeFalse();
    }
    #endregion
}
