using AuthServer.Controllers.v1;
using AuthServer.Data.Requests;
using AuthServer.Repositories;
using AuthServer.Test.Extensions;
using AutoFixture;
using Moq;

namespace AuthServer.Test.Controllers;

[TestFixture]
public class AuthControllerTest
{
    private readonly Mock<IUnitOfRepository> _unitOfRepository;
    private readonly Fixture _fixture;
    private AuthController _authController;

    public AuthControllerTest()
    {
        _unitOfRepository = new Mock<IUnitOfRepository>();
        _fixture = new Fixture().OmitOnRecursionBehavior();
    }

    [Test]
    public async Task Login_ReturnOk()
    {
        #region Arrange
        var loginRequest = _fixture.Create<LoginRequest>();
        /*_unitOfRepository.Setup()*/
        

        #endregion
    }
}

