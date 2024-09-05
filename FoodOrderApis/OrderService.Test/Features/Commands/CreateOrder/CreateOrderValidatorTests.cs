using FluentAssertions;
using OrderService.Data.Requests;
using OrderService.Features.Commands.OrderCommands.CreateOrder;

namespace OrderService.Test.Features.Commands.CreateOrder;

public class CreateOrderValidatorTests
{
    private readonly CreateOrderValidator _validator;
    private CreateOrderInput _validInput;

    public CreateOrderValidatorTests()
    {
        _validator = new CreateOrderValidator();
    }

    [SetUp]
    public void Setup()
    {
        _validInput = new CreateOrderInput
        {
            EaterId = "bb",
            MerchantId = "aa"
        };
    }
    
    #region Setup Test Cases
    
    private static IEnumerable<TestCaseData> InvalidUserIdTestCases()
    {
        yield return new TestCaseData(null)
            .SetName("Id field is required");
        yield return new TestCaseData(string.Empty)
            .SetName("Id field is required");
    }
    
    #endregion

    #region Setup Tests

    [Test]
    public async Task Validate_ShouldBeValid_WhenGivenValidInput()
    {
        var command = new CreateOrderCommand
        {
            Payload = _validInput
        };
        
        var actual = await _validator.ValidateAsync(command);
        
        actual.IsValid.Should().BeTrue();
    }
    
    [Test, TestCaseSource(nameof(InvalidUserIdTestCases))]
    public async Task Validate_ShouldBeInvalid_WhenGiveInvalid_EaterId(string invalidEaterId)
    {
        var request = new CreateOrderInput
        {
            EaterId = invalidEaterId,
            MerchantId = _validInput.MerchantId
        };
        var command = new CreateOrderCommand
        {
            Payload = request
        };
        var actual = await _validator.ValidateAsync(command);
        actual.IsValid.Should().BeFalse();
    }
    
    [Test, TestCaseSource(nameof(InvalidUserIdTestCases))]
    public async Task Validate_ShouldBeInvalid_WhenGiveInvalid_MerchantId(string invalidMerchantId)
    {
        var request = new CreateOrderInput
        {
            EaterId = _validInput.EaterId,
            MerchantId = invalidMerchantId
        };
        var command = new CreateOrderCommand
        {
            Payload = request
        };
        var actual = await _validator.ValidateAsync(command);
        actual.IsValid.Should().BeFalse();
    }

    #endregion
    
    
}
