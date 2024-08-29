using AuthServer.Data.Requests;
using AuthServer.Features.Commands.LoginCommands;
using FoodOrderApis.Common.Helpers;

namespace AuthServer.Test.Features.Commands.Login;

[TestFixture]
public class LoginValidatorTests
{
    private readonly LoginValidator _validator;
    private LoginRequest _validRequest = new ();

    public LoginValidatorTests()
    {
        _validator = new LoginValidator();
    }

    [SetUp]
    public void Setup()
    {
        _validRequest = new LoginRequest
        {
            Username = "yamato",
            Password = "Dat123456@",
            Scope = "food.read",
        };
    }

    #region Setup Test Cases

    private static IEnumerable<TestCaseData> InvalidUsernameTestCases()
    {
        yield return new TestCaseData(null)
            .SetName("Username field is required");
        yield return new TestCaseData(string.Empty)
            .SetName("Username field is required");
        var usernameUnderMinLength = RandomGenerator.RandomString(2);
        yield return new TestCaseData(usernameUnderMinLength)
            .SetName("Username must be a string with a minimum length of 3 characters");
        var usernameOverMaxLength = RandomGenerator.RandomString(101);
        yield return new TestCaseData(usernameOverMaxLength)
            .SetName("");
    }

    /*private static IEnumerable<TestCaseData> InvalidPasswordTestCases()
    {
        
    }*/

    #endregion
    
}
