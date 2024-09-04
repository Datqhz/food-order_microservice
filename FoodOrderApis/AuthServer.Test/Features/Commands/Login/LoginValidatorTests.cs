using AuthServer.Data.Requests;
using AuthServer.Features.Commands.LoginCommands;
using FluentAssertions;
using FoodOrderApis.Common.Helpers;
using NUnit.Framework;

namespace AuthServer.Test.Features.Commands.Login;

[TestFixture]
public class LoginValidatorTests
{
    private readonly LoginValidator _validator;
    private LoginInput _validInput = new ();

    public LoginValidatorTests()
    {
        _validator = new LoginValidator();
    }

    [SetUp]
    public void Setup()
    {
        _validInput = new LoginInput
        {
            Username = "yamato",
            Password = "String123@",
        };
    }

    #region Setup Test Cases

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

    #endregion


    #region Setup Tests
    [Test]
    public async Task Validate_ShouldBeValid_WhenGivenValidRequest()
    {
        var command = new LoginCommand
        {
            Payload = _validInput
        };
        var actual = await _validator.ValidateAsync(command);
        actual.IsValid.Should().BeTrue();
    }
    [Test, TestCaseSource(nameof(InvalidUsernameTestCases))]
    public async Task Validate_ShouldBeInvalid_WhenGiveInvalid_UserName(string invalidUserName)
    {
        var request = new LoginInput
        {
            Username = invalidUserName,
            Password = _validInput.Password
        };
        var command = new LoginCommand
        {
            Payload = request
        };
        var actual = await _validator.ValidateAsync(command);
        actual.IsValid.Should().BeFalse();
    }
    [Test, TestCaseSource(nameof(InvalidPasswordTestCases))]
    public async Task Validate_ShouldBeInvalid_WhenGiveInvalid_Password(string invalidPassword)
    {
        var request = new LoginInput
        {
            Username = _validInput.Username,
            Password = invalidPassword
        };
        var command = new LoginCommand
        {
            Payload = request
        };
        var actual = await _validator.ValidateAsync(command);
        actual.IsValid.Should().BeFalse();
    }
    #endregion
}
