using AuthServer.Data.Requests;
using AuthServer.Features.Commands.UpdateUser;
using FluentAssertions;
using FoodOrderApis.Common.Helpers;

namespace AuthServer.Test.Features.Commands.UpdateUser;

[TestFixture]
public class UpdateUserValidatorTests
{
    private readonly UpdateUserValidator _validator;
    private UpdateUserRequest _validRequest;

    public UpdateUserValidatorTests()
    {
        _validator = new UpdateUserValidator();
    }

    [SetUp]
    public void Setup()
    {
        _validRequest = new UpdateUserRequest()
        { 
            UserId = "a",
            OldPassword = "String123@",
            NewPassword = "String1234@"
        };
    }

    #region Setup TestCases

    private static IEnumerable<TestCaseData> InvalidUserIdTestCases()
    {
        yield return new TestCaseData(null)
            .SetName("UserId field is required");
        yield return new TestCaseData(string.Empty)
            .SetName("UserId field is required"); ;
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
    public void Validate_ShouldBeValid_WhenGivenValidInput()
    {
        var command = new UpdateUserCommand()
        {
            Payload = _validRequest
        };
        
        var act = _validator.Validate(command);

        act.IsValid.Should().BeTrue();
    }
    
    [Test, TestCaseSource(nameof(InvalidUserIdTestCases))]
    public void Validate_ShouldBeInvalid_WhenGivenInvalid_UserId(string invalidUserId)
    {
        var invalidInput = new UpdateUserRequest()
        {
            UserId = invalidUserId,
            OldPassword = _validRequest.OldPassword,
            NewPassword = _validRequest.NewPassword,
        };
        var command = new UpdateUserCommand()
        {
            Payload = invalidInput
        };
        
        var actual = _validator.Validate(command);
        
        actual.IsValid.Should().BeFalse();
    }
    [Test, TestCaseSource(nameof(InvalidUserIdTestCases))]
    public void Validate_ShouldBeInvalid_WhenGivenInvalid_OldPassword(string invalidPassword)
    {
        var invalidInput = new UpdateUserRequest()
        {
            UserId = _validRequest.UserId,
            OldPassword = invalidPassword,
            NewPassword = _validRequest.NewPassword,
        };
        var command = new UpdateUserCommand()
        {
            Payload = invalidInput
        };
        
        var actual = _validator.Validate(command);
        
        actual.IsValid.Should().BeFalse();
    }
    
    [Test, TestCaseSource(nameof(InvalidUserIdTestCases))]
    public void Validate_ShouldBeInvalid_WhenGivenInvalid_NewPassword(string invalidPassword)
    {
        var invalidInput = new UpdateUserRequest()
        {
            UserId = _validRequest.UserId,
            OldPassword = _validRequest.OldPassword,
            NewPassword = invalidPassword,
        };
        var command = new UpdateUserCommand()
        {
            Payload = invalidInput
        };
        
        var actual = _validator.Validate(command);
        
        actual.IsValid.Should().BeFalse();
    }
    
    #endregion
}
