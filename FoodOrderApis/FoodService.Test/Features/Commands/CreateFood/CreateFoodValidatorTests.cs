using FluentAssertions;
using FoodService.Data.Requests;
using FoodService.Features.Commands.FoodCommands.CreateFoodCommands;
using Microsoft.AspNetCore.Identity.Data;

namespace FoodService.Test.Features.Commands.CreateFood;

[TestFixture]
public class CreateFoodValidatorTests
{
    private readonly CreateFoodValidator _validator;
    private CreateFoodInput _validRequest = new ();

    public CreateFoodValidatorTests()
    {
        _validator = new CreateFoodValidator();
    }

    [SetUp]
    public void Setup()
    {
        _validRequest = new CreateFoodInput
        {
            Name = "Test",
            Describe = "aaa",
            ImageUrl = "aaa",
            Price = 10,
            UserId = "aaa"
        };

    }

    #region Setup Test Cases

    private static IEnumerable<TestCaseData> InvalidNameTestCases()
    {
        yield return new TestCaseData(null)
            .SetName("Name field is required");
        yield return new TestCaseData(string.Empty)
            .SetName("Name field is required");
    }

    private static IEnumerable<TestCaseData> InvalidImageUrlTestCases()
    {
        yield return new TestCaseData(null)
            .SetName("Image url field is required");
        yield return new TestCaseData(string.Empty)
            .SetName("Image url field is required");
    }
    private static IEnumerable<TestCaseData> InvalidPriceTestCases()
    {
        yield return new TestCaseData(null)
            .SetName("Image url field is required");
        yield return new TestCaseData(-1)
            .SetName("Food price must be greater than or equal to 0");
    }

    private static IEnumerable<TestCaseData> InvalidUserIdTestCases()
    {
        yield return new TestCaseData(null)
            .SetName("User id field is required");
        yield return new TestCaseData(string.Empty)
            .SetName("User id field is required");
    }

    #endregion

    #region Setup Tests

    [Test]
    public async Task Validate_ShouldBeValid_WhenGivenValidRequest()
    {
        var command = new CreateFoodCommand
        {
            Payload = _validRequest
        };

        var actual = await _validator.ValidateAsync(command);

        actual.IsValid.Should().BeTrue();
    }

    [Test, TestCaseSource(nameof(InvalidNameTestCases))]
    public async Task Validate_ShouldBeInvalid_WhenGiveInvalid_Name(string invalidName)
    {
        var request = new CreateFoodInput
        {
            Name = invalidName,
            Describe = _validRequest.Describe,
            ImageUrl = _validRequest.ImageUrl,
            Price = _validRequest.Price,
            UserId = _validRequest.UserId
        };
        var command = new CreateFoodCommand
        {
            Payload = request
        };
        var actual = await _validator.ValidateAsync(command);
        actual.IsValid.Should().BeFalse();
    }

    [Test, TestCaseSource(nameof(InvalidImageUrlTestCases))]
    public async Task Validate_ShouldBeInvalid_WhenGiveInvalid_ImageUrl(string invalidImageUrl)
    {
        var request = new CreateFoodInput
        {
            Name = _validRequest.Name,
            Describe = _validRequest.Describe,
            ImageUrl = invalidImageUrl,
            Price = _validRequest.Price,
            UserId = _validRequest.UserId
        };
        var command = new CreateFoodCommand
        {
            Payload = request
        };
        var actual = await _validator.ValidateAsync(command);
        actual.IsValid.Should().BeFalse();
    }

    [Test, TestCaseSource(nameof(InvalidUserIdTestCases))]
    public async Task Validate_ShouldBeInvalid_WhenGiveInvalid_(string invalidUserId)
    {
        var request = new CreateFoodInput
        {
            Name = _validRequest.Name,
            Describe = _validRequest.Describe,
            ImageUrl = _validRequest.ImageUrl,
            Price = _validRequest.Price,
            UserId = invalidUserId
        };
        var command = new CreateFoodCommand
        {
            Payload = request
        };
        var actual = await _validator.ValidateAsync(command);
        actual.IsValid.Should().BeFalse();
    }

    [Test, TestCaseSource(nameof(InvalidPriceTestCases))]
    public async Task Validate_ShouldBeInvalid_WhenGiveInvalid_Price(decimal invalidPrice)
    {
        var request = new CreateFoodInput
        {
            Name = _validRequest.Name,
            Describe = _validRequest.Describe,
            ImageUrl = _validRequest.ImageUrl,
            Price = invalidPrice,
            UserId = _validRequest.UserId
        };
        var command = new CreateFoodCommand
        {
            Payload = request
        };
        var actual = await _validator.ValidateAsync(command);
        Console.WriteLine(actual.ToString("-"));
        actual.IsValid.Should().BeFalse();
    }
    #endregion


}