using AuthServer.Data.Requests;
using AuthServer.Features.Commands.RegisterCommands;
using FluentAssertions;
using FoodOrderApis.Common.Helpers;

namespace AuthServer.Test.Features.Commands.Register;

[TestFixture]
public class RegisterValidatorTests
{
    private readonly RegisterValidator _validator;
    private RegisterInput _validInput = new ();

    public RegisterValidatorTests()
    {
        _validator = new RegisterValidator();
    }
    [SetUp]
    public void Setup()
    {
        _validInput = new RegisterInput
        {
            Displayname = "Yamato",
            Username = "yamato",
            ClientId = "Eater",
            PhoneNumber = "0123456789",
            Password = "String123@"
        };
    }

    #region  Setup Test Cases
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
    
    private static IEnumerable<TestCaseData> InvalidUsernameTestCases()
    {
        yield return new TestCaseData(null)
            .SetName("Username field is required");
        yield return new TestCaseData(string.Empty)
            .SetName("Username field is required");
        var usernameUnderMinLength = RandomGenerator.RandomString(4);
        yield return new TestCaseData(usernameUnderMinLength)
            .SetName("Username must be a string with a minimum length of 5 characters");
        var usernameOverMaxLength = RandomGenerator.RandomString(51);
        yield return new TestCaseData(usernameOverMaxLength)
            .SetName("Username must be a string with a maximum length of 50 characters");
    }

    private static IEnumerable<TestCaseData> InvalidPasswordTestCases()
    {
        yield return new TestCaseData(null)
            .SetName("Password field is required");
        yield return new TestCaseData(string.Empty)
            .SetName("Password field is required");
        var passwordUnderMinLength = RandomGenerator.RandomString(7);
        yield return new TestCaseData(passwordUnderMinLength)
            .SetName("Password must be a string with a minimum length of 8 characters");
        var passwordOverMaxLength = RandomGenerator.RandomString(17);
        yield return new TestCaseData(passwordOverMaxLength)
            .SetName("Password must be a string with a maximum length of 16 characters");
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
        var command = new RegisterCommand
        {
            Payload = _validInput
        };
        
        var actual = await _validator.ValidateAsync(command);

        actual.IsValid.Should().BeTrue();
    }

    [Test, TestCaseSource(nameof(InvalidDisplayNameTestCases))]
    public async Task Validate_ShouldBeInvalid_WhenGiveInvalid_DisplayName(string invalidDisplayName)
    {
        var request = new RegisterInput
        {
            Displayname = invalidDisplayName,
            Username = _validInput.Username,
            ClientId = _validInput.ClientId,
            PhoneNumber = _validInput.PhoneNumber,
            Password = _validInput.Password,
        };
        var command = new RegisterCommand
        {
            Payload = request
        };
        var actual = await _validator.ValidateAsync(command);
        actual.IsValid.Should().BeFalse();
    }
    
    [Test, TestCaseSource(nameof(InvalidUsernameTestCases))]
    public async Task Validate_ShouldBeInvalid_WhenGiveInvalid_UserName(string invalidUserName)
    {
        var request = new RegisterInput
        {
            Displayname = _validInput.Displayname,
            Username = invalidUserName,
            ClientId = _validInput.ClientId,
            PhoneNumber = _validInput.PhoneNumber,
            Password = _validInput.Password,
        };
        var command = new RegisterCommand
        {
            Payload = request
        };
        var actual = await _validator.ValidateAsync(command);
        actual.IsValid.Should().BeFalse();
    }
    [Test, TestCaseSource(nameof(InvalidPasswordTestCases))]
    public async Task Validate_ShouldBeInvalid_WhenGiveInvalid_Password(string invalidPassword)
    {
        var request = new RegisterInput
        {
            Displayname =  _validInput.Displayname,
            Username = _validInput.Username,
            ClientId = _validInput.ClientId,
            PhoneNumber = _validInput.PhoneNumber,
            Password = invalidPassword,
        };
        var command = new RegisterCommand
        {
            Payload = request
        };
        var actual = await _validator.ValidateAsync(command);
        actual.IsValid.Should().BeFalse();
    }
    [Test, TestCaseSource(nameof(InvalidPhoneNumberTestCases))]
    public async Task Validate_ShouldBeInvalid_WhenGiveInvalid_PhoneNumber(string invalidPhoneNumber)
    {
        var request = new RegisterInput
        {
            Displayname =  _validInput.Displayname,
            Username = _validInput.Username,
            ClientId = _validInput.ClientId,
            PhoneNumber = invalidPhoneNumber,
            Password = _validInput.Password,
        };
        var command = new RegisterCommand
        {
            Payload = request
        };
        var actual = await _validator.ValidateAsync(command);
        actual.IsValid.Should().BeFalse();
    }
    #endregion
}