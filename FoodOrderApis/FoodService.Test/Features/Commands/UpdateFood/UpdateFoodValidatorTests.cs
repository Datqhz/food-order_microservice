using FluentAssertions;
using FoodOrderApis.Common.Helpers;
using FoodService.Data.Requests;
using FoodService.Features.Commands.FoodCommands.UpdateFoodCommands;

namespace FoodService.Test.Features.Commands.UpdateFood;

public class UpdateFoodValidatorTests
{
 private readonly UpdateFoodValidator _validator;
    private UpdateFoodInput _validRequest = new();

    public UpdateFoodValidatorTests()
    {
        _validator = new UpdateFoodValidator();
    }

    [SetUp]
    public void Setup()
    {
        _validRequest = new UpdateFoodInput
        {
            Id = 1,
            Name = "Name",
            Describe = "Describe",
            ImageUrl = "ImageUrl",
            Price = 10000
        };
    }

    #region Setup Test Cases

    private static IEnumerable<TestCaseData> InvalidIdTestCases()
    {
        yield return new TestCaseData(null)
            .SetName("Foodd field is required");
    }

    private static IEnumerable<TestCaseData> InvalidNameTestCases()
    {
        yield return new TestCaseData(null)
            .SetName("Name field is required");
        yield return new TestCaseData(string.Empty)
            .SetName("Name field is required");
    }

    private static IEnumerable<TestCaseData> InvalidPriceTestCases()
    {
        yield return new TestCaseData((decimal)-1000)
            .SetName("Food price must be greater than or equal to 0");
    }
    
    private static IEnumerable<TestCaseData> InvalidImageUrlTestCases()
    {
        yield return new TestCaseData(null)
            .SetName("Image field is required");
        yield return new TestCaseData(string.Empty)
            .SetName("Image field is required");
    }
    
    #endregion

    #region Setup Tests

    [Test]
    public async Task Validate_ShouldBeValid_WhenGivenValidRequest()
    {
        var command = new UpdateFoodCommand
        {
            Payload = _validRequest
        };

        var actual = await _validator.ValidateAsync(command);
        Console.WriteLine(actual.ToString("~"));
        actual.IsValid.Should().BeTrue();
    }

    [Test, TestCaseSource(nameof(InvalidIdTestCases))]
    public async Task Validate_ShouldBeInvalid_WhenGiveInvalidId(int invalidId)
    {
        var request = new UpdateFoodInput
        {
            Id = invalidId,
            Name = _validRequest.Name,
            Describe = _validRequest.Describe,
            ImageUrl = _validRequest.ImageUrl,
            Price = _validRequest.Price,
        };
        var command = new UpdateFoodCommand
        {
            Payload = request
        };
        var actual = await _validator.ValidateAsync(command);
        actual.IsValid.Should().BeFalse();
    }

    [Test, TestCaseSource(nameof(InvalidNameTestCases))]
    public async Task Validate_ShouldBeInvalid_WhenGiveInvalidName(string invalidName)
    {
        var request = new UpdateFoodInput
        {
            Id = _validRequest.Id,
            Name = invalidName,
            Describe = _validRequest.Describe,
            ImageUrl = _validRequest.ImageUrl,
            Price = _validRequest.Price,
        };
        var command = new UpdateFoodCommand
        {
            Payload = request
        };
        var actual = await _validator.ValidateAsync(command);
        actual.IsValid.Should().BeFalse();
    }

    [Test, TestCaseSource(nameof(InvalidPriceTestCases))]
    public async Task Validate_ShouldBeInvalid_WhenGiveInvalidPrice(decimal invalidPrice)
    {
        var request = new UpdateFoodInput
        {
            Id = _validRequest.Id,
            Name = _validRequest.Name,
            Describe = _validRequest.Describe,
            ImageUrl = _validRequest.ImageUrl,
            Price = invalidPrice,
        };
        var command = new UpdateFoodCommand
        {
            Payload = request
        };
        var actual = await _validator.ValidateAsync(command);
        actual.IsValid.Should().BeFalse();
    }
    [Test, TestCaseSource(nameof(InvalidImageUrlTestCases))]
    public async Task Validate_ShouldBeInvalid_WhenGiveInvalidImageUrl(string invalidImageUrl)
    {
        var request = new UpdateFoodInput
        {
            Id = _validRequest.Id,
            Name = _validRequest.Name,
            Describe = _validRequest.Describe,
            ImageUrl = invalidImageUrl,
            Price = _validRequest.Price,
        };
        var command = new UpdateFoodCommand
        {
            Payload = request
        };
        var actual = await _validator.ValidateAsync(command);
        actual.IsValid.Should().BeFalse();
    }
    #endregion   
}


