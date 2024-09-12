using CustomerService.Data.Requests;
using CustomerService.Features.Commands.UserInfoCommands.CreateUser;
using FluentAssertions;
using FoodOrderApis.Common.Helpers;

namespace CustomerService.Test.Features.Commands.CreateUser;

public class CreateUserValidatorTests
{
    private readonly CreateUserValidator _validator;
    private CreateUserInfoRequest _validRequest;

    public CreateUserValidatorTests()
    {
        _validator = new CreateUserValidator();
    }

    [SetUp]
    public void Setup()
    {
        _validRequest = new CreateUserInfoRequest
        {
            UserId = "a",
            UserName = "yamato",
            DisplayName = "Yamato",
            CreatedDate = DateTime.Now,
            IsActive = true,
            PhoneNumber = "0323232563",
            Role = "EATER"
        };
    }

    #region Setup Test Cases

    private static IEnumerable<TestCaseData> InvalidUserIdTestCases()
    {
        yield return new TestCaseData(null)
            .SetName("UserId is required");
        yield return new TestCaseData(string.Empty)
            .SetName("UserId is required");
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
    private static IEnumerable<TestCaseData> InvalidUserNameTestCases()
    {
        yield return new TestCaseData(null)
            .SetName("UserName field is required");
        yield return new TestCaseData(string.Empty)
            .SetName("UserName field is required");
        var usernameUnderMinLength = RandomGenerator.RandomString(4);
        yield return new TestCaseData(usernameUnderMinLength)
            .SetName("UserName must be a string with a minimum length of 5 characters");
        var usernameOverMaxLength = RandomGenerator.RandomString(51);
        yield return new TestCaseData(usernameOverMaxLength)
            .SetName("UserName must be a string with a maximum length of 50 characters");
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
    private static IEnumerable<TestCaseData> InvalidRoleTestCases()
    {
        yield return new TestCaseData(null)
            .SetName("Role is required");
        yield return new TestCaseData(string.Empty)
            .SetName("Role is required");
    }
    #endregion

    #region Setup Tests

    [Test]
    public void Validate_ShouldBeValid_WhenGivenValidInput()
    {
        var command = new CreateUserCommand
        {
            Payload = _validRequest
        };
        
        var actual = _validator.Validate(command);

        actual.IsValid.Should().BeTrue();
    }
    
    [Test, TestCaseSource(nameof(InvalidUserIdTestCases))]
    public void Validate_ShouldBeInvalid_WhenGivenInvalidUserId(string invalidUserId)
    {
        var invalidInput = new CreateUserInfoRequest
        {
            UserId = invalidUserId,
            UserName = _validRequest.UserName,
            DisplayName = _validRequest.DisplayName,
            CreatedDate = _validRequest.CreatedDate,
            IsActive = _validRequest.IsActive,
            PhoneNumber = _validRequest.PhoneNumber,
            Role = _validRequest.Role
        };
        var command = new CreateUserCommand
        {
            Payload = invalidInput
        };
        
        var actual = _validator.Validate(command);

        actual.IsValid.Should().BeFalse();
    }
    [Test, TestCaseSource(nameof(InvalidUserNameTestCases))]
    public void Validate_ShouldBeInvalid_WhenGivenInvalidUserName(string invalidUserName)
    {
        var invalidInput = new CreateUserInfoRequest
        {
            UserId = _validRequest.UserId,
            UserName = invalidUserName,
            DisplayName = _validRequest.DisplayName,
            CreatedDate = _validRequest.CreatedDate,
            IsActive = _validRequest.IsActive,
            PhoneNumber = _validRequest.PhoneNumber,
            Role = _validRequest.Role
        };
        var command = new CreateUserCommand
        {
            Payload = invalidInput
        };
        
        var actual = _validator.Validate(command);

        actual.IsValid.Should().BeFalse();
    }
    [Test, TestCaseSource(nameof(InvalidDisplayNameTestCases))]
    public void Validate_ShouldBeInvalid_WhenGivenInvalidDisplayName(string invalidDisplayName)
    {
        var invalidInput = new CreateUserInfoRequest
        {
            UserId = _validRequest.UserId,
            UserName = _validRequest.UserName,
            DisplayName = invalidDisplayName,
            CreatedDate = _validRequest.CreatedDate,
            IsActive = _validRequest.IsActive,
            PhoneNumber = _validRequest.PhoneNumber,
            Role = _validRequest.Role
        };
        var command = new CreateUserCommand
        {
            Payload = invalidInput
        };
        
        var actual = _validator.Validate(command);

        actual.IsValid.Should().BeFalse();
    }
    
    [Test, TestCaseSource(nameof(InvalidPhoneNumberTestCases))]
    public void Validate_ShouldBeInvalid_WhenGivenInvalidPhoneNumber(string invalidPhoneNumber)
    {
        var invalidInput = new CreateUserInfoRequest
        {
            UserId = _validRequest.UserId,
            UserName = _validRequest.UserName,
            DisplayName = _validRequest.DisplayName,
            CreatedDate = _validRequest.CreatedDate,
            IsActive = _validRequest.IsActive,
            PhoneNumber = invalidPhoneNumber,
            Role = _validRequest.Role
        };
        var command = new CreateUserCommand
        {
            Payload = invalidInput
        };
        
        var actual = _validator.Validate(command);

        actual.IsValid.Should().BeFalse();
    }
    [Test, TestCaseSource(nameof(InvalidRoleTestCases))]
    public void Validate_ShouldBeInvalid_WhenGivenInvalidRole(string invalidRole)
    {
        var invalidInput = new CreateUserInfoRequest
        {
            UserId = _validRequest.UserId,
            UserName = _validRequest.UserName,
            DisplayName = _validRequest.DisplayName,
            CreatedDate = _validRequest.CreatedDate,
            IsActive = _validRequest.IsActive,
            PhoneNumber = _validRequest.PhoneNumber,
            Role = invalidRole
        };
        var command = new CreateUserCommand
        {
            Payload = invalidInput
        };
        
        var actual = _validator.Validate(command);

        actual.IsValid.Should().BeFalse();
    }


    #endregion
}
