using AuthServer.Features.Commands.DeleteUser;
using FluentAssertions;

namespace AuthServer.Test.Features.Commands.DeleteUser;

public class DeleletUserValidatorTests
{
    private readonly DeleteUserValidator _validator;
    private string validId;

    public DeleletUserValidatorTests()
    {
        _validator = new DeleteUserValidator();
    }

    [SetUp]
    public void Setup()
    {
        validId = "a";
    }

    #region Setup Test Cases

    private static IEnumerable<TestCaseData> InvalidUserIdTestCases()
    {
        yield return new TestCaseData(null)
            .SetName("Password field is required");
        yield return new TestCaseData(string.Empty)
            .SetName("Password field is required");
    }
    #endregion

    [Test]
    public void Validate_ShouldBeValid_WhenGivenValidUserId()
    {
        var command = new DeleteUserCommand { UserId = validId };
        var act = _validator.Validate(command);
        act.IsValid.Should().BeTrue();
    }

    [Test, TestCaseSource(nameof(InvalidUserIdTestCases))]
    public void Validate_ShouldBeInvalid_WhenGivenInvalidUserId(string userId)
    {
        var command = new DeleteUserCommand
        {
            UserId = userId
        };
        var act = _validator.Validate(command);
        
        act.IsValid.Should().BeFalse();
    }
    
}
