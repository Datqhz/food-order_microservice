using FluentAssertions;
using OrderService.Data.Requests;
using OrderService.Features.Commands.OrderDetailCommands.ModifyMultipleOrderDetail;

namespace OrderService.Test.Features.Commands.ModifyMultipleOrderDetails;

[TestFixture]
public class ModifyMultipleOrderDetailsValidatorTests
{
    private readonly ModifyMultipleOrderDetailValidator _validator;
    private List<ModifyOrderDetailInput> _validRequest;
    public ModifyMultipleOrderDetailsValidatorTests()
    {
        _validator = new ModifyMultipleOrderDetailValidator();
    }

    [SetUp]
    public void Setup()
    {
        _validRequest = new List<ModifyOrderDetailInput>()
        {
            new ModifyOrderDetailInput
            {
                OrderDetailId = 1,
                Price = 10000,
                Quantity = 1,
                Feature = 2
            }
        };
    }

    #region Setup Test Cases

    private static IEnumerable<TestCaseData> InvalidPayloadTestCases()
    {
        yield return new TestCaseData(null)
            .SetName("Payload cannot be null");
        yield return new TestCaseData(new List<ModifyOrderDetailInput>())
            .SetName("Payload cannot be empty");
    }
    
    private static IEnumerable<TestCaseData> InvalidFeatureTestCases()
    {
        yield return new TestCaseData(0)
            .SetName("Feature must be between 1 and 3");
        yield return new TestCaseData(4)
            .SetName("Feature must be between 1 and 3");
    }
    
    private static IEnumerable<TestCaseData> InvalidPriceTestCases()
    {
        yield return new TestCaseData((decimal)-1)
            .SetName("Price must be greater than or equal 0");
    }
    
    private static IEnumerable<TestCaseData> InvalidQuantityTestCases()
        {
            yield return new TestCaseData(0)
                .SetName("Quantity must be greater than 0");
        }
    #endregion
    
    #region Setup Tests

    [Test]
    public void Validate_ShouldBeValid()
    {
        var command = new ModifyMultipleOrderDetailCommand()
        {
            Payload = _validRequest,
        };
        var act = _validator.Validate(command);
        
        act.IsValid.Should().BeTrue();
    }

    [Test, TestCaseSource(nameof(InvalidPayloadTestCases))]
    public void Validate_ShouldNotBeValid_WhenGivenInvalidPayload(List<ModifyOrderDetailInput> invalidPayload)
    {
        var command = new ModifyMultipleOrderDetailCommand() { Payload = invalidPayload };
        
        var act =  _validator.Validate(command);
        act.IsValid.Should().BeFalse();
    }

    [Test, TestCaseSource(nameof(InvalidFeatureTestCases))]
    public void Validate_ShouldBeInvalid_WhenGivenInvalidFeature(int invalidFeature)
    {
        List<ModifyOrderDetailInput> request = new List<ModifyOrderDetailInput>()
        {
            new ModifyOrderDetailInput
            {
                OrderDetailId = _validRequest[0].OrderDetailId,
                Feature = invalidFeature,
                Price = _validRequest[0].Price,
                Quantity = _validRequest[0].Quantity,
            } 
        };
        var command = new ModifyMultipleOrderDetailCommand
        {
            Payload = request
        };
        
        var act = _validator.Validate(command);
        
        act.IsValid.Should().BeFalse();
    }
    
    [Test, TestCaseSource(nameof(InvalidPriceTestCases))]
    public void Validate_ShouldBeInvalid_WhenGivenInvalidPrice(decimal invalidPrice)
    {
        List<ModifyOrderDetailInput> request = new List<ModifyOrderDetailInput>()
        {
            new ModifyOrderDetailInput
            {
                OrderDetailId = _validRequest[0].OrderDetailId,
                Feature = _validRequest[0].Feature,
                Price = invalidPrice,
                Quantity = _validRequest[0].Quantity,
            } 
        };
        var command = new ModifyMultipleOrderDetailCommand
        {
            Payload = request
        };
        
        var act = _validator.Validate(command);
        
        act.IsValid.Should().BeFalse();
    }
    [Test, TestCaseSource(nameof(InvalidQuantityTestCases))]
    public void Validate_ShouldBeInvalid_WhenGivenInvalidQuantity(int invalidQuantity)
    {
        List<ModifyOrderDetailInput> request = new List<ModifyOrderDetailInput>()
        {
            new ModifyOrderDetailInput
            {
                OrderDetailId = _validRequest[0].OrderDetailId,
                Feature = _validRequest[0].Feature,
                Price = _validRequest[0].Price,
                Quantity = invalidQuantity,
            } 
        };
        var command = new ModifyMultipleOrderDetailCommand
        {
            Payload = request
        };
        
        var act = _validator.Validate(command);
        
        act.IsValid.Should().BeFalse();
    }
    #endregion
}