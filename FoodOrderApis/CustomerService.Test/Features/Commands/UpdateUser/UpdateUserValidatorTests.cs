using CustomerService.Data.Requests;
using CustomerService.Features.Commands.UserInfoCommands.UpdateUser;
using FluentAssertions;
using FoodOrderApis.Common.Helpers;

namespace CustomerService.Test.Features.Commands.UpdateUser;

[TestFixture]
public class UpdateUserValidatorTests
{
    private readonly UpdateUserValidator _validator;
    private UpdateUserInfoInput _validRequest = new();

    public UpdateUserValidatorTests()
    {
        _validator = new UpdateUserValidator();
    }

    [SetUp]
    public void Setup()
    {
        _validRequest = new UpdateUserInfoInput
        {
            Id = "a",
            DisplayName = "Yamato",
            PhoneNumber = "0123456789"
        };
    }

    #region Setup Test Cases

    private static IEnumerable<TestCaseData> InvalidIdTestCases()
    {
        yield return new TestCaseData(null)
            .SetName("Id field is required");
        yield return new TestCaseData(string.Empty)
            .SetName("Id field is required");
    }

    private static IEnumerable<TestCaseData> InvalidDisplayNameTestCases()
    {
        yield return new TestCaseData(null)
            .SetName("DisplayName field is required");
        yield return new TestCaseData(string.Empty)
            .SetName("DisplayName field is required");
        var displaynameUnderMinLength = RandomGenerator.RandomString(2);
        yield return new TestCaseData(displaynameUnderMinLength)
            .SetName("DisplayName must be a string with a minimum length of 3 characters");
        var displaynameOverMaxLength = RandomGenerator.RandomString(101);
        yield return new TestCaseData(displaynameOverMaxLength)
            .SetName("DisplayName must be a string with a maximum length of 100 characters");
    }

    private static IEnumerable<TestCaseData> InvalidPhoneNumberTestCases()
    {
        yield return new TestCaseData(string.Empty)
            .SetName("PhoneNumber field is required");
        yield return new TestCaseData("g090876765")
            .SetName("Phone invalid");
        yield return new TestCaseData("09908767658")
            .SetName("Phone invalid");
    }
    #endregion

    #region Setup Tests

    [Test]
    public async Task Validate_ShouldBeValid_WhenGivenValidRequest()
    {
        var command = new UpdateUserCommand
        {
            Payload = _validRequest
        };

        var actual = await _validator.ValidateAsync(command);
        Console.WriteLine(actual.ToString("~"));
        actual.IsValid.Should().BeTrue();
    }

    [Test, TestCaseSource(nameof(InvalidIdTestCases))]
    public async Task Validate_ShouldBeInvalid_WhenGiveInvalid_Id(string invalidId)
    {
        var request = new UpdateUserInfoInput
        {
            Id = invalidId,
            DisplayName = _validRequest.DisplayName,
            PhoneNumber = _validRequest.PhoneNumber
        };
        var command = new UpdateUserCommand
        {
            Payload = request
        };
        var actual = await _validator.ValidateAsync(command);
        actual.IsValid.Should().BeFalse();
    }

    [Test, TestCaseSource(nameof(InvalidDisplayNameTestCases))]
    public async Task Validate_ShouldBeInvalid_WhenGiveInvalid_DisplayName(string invalidDisplayName)
    {
        var request = new UpdateUserInfoInput
        {
            Id = _validRequest.Id,
            DisplayName = invalidDisplayName,
            PhoneNumber = _validRequest.PhoneNumber
        };
        var command = new UpdateUserCommand
        {
            Payload = request
        };
        var actual = await _validator.ValidateAsync(command);
        actual.IsValid.Should().BeFalse();
    }

    [Test, TestCaseSource(nameof(InvalidPhoneNumberTestCases))]
    public async Task Validate_ShouldBeInvalid_WhenGiveInvalid_PhoneNumber(string invalidPhoneNumber)
    {
        var request = new UpdateUserInfoInput
        {
            Id = _validRequest.Id,
            DisplayName = _validRequest.DisplayName,
            PhoneNumber = invalidPhoneNumber
        };
        var command = new UpdateUserCommand
        {
            Payload = request
        };
        var actual = await _validator.ValidateAsync(command);
        actual.IsValid.Should().BeFalse();
    }
    #endregion
}
