using AuthServer.Controllers.v1;
using AuthServer.Data.Requests;
using AuthServer.Repositories;
using AuthServer.Test.Extensions;
using AutoFixture;
using MediatR;
using Moq;

namespace AuthServer.Test.Controllers;

[TestFixture]
public class AuthControllerTest
{
    private readonly Mock<IUnitOfRepository> _mockUnitOfRepository;
    private readonly Fixture _fixture;
    private readonly Mock<IMediator> _mockMediator;
    private AuthController _controller;

    public AuthControllerTest()
    {
        _mockUnitOfRepository = new Mock<IUnitOfRepository>();
        _fixture = new Fixture().OmitOnRecursionBehavior();
        _mockMediator = new Mock<IMediator>();
    }

    [Test]
    public async Task Login_ReturnOk()
    {
        #region Arrange
        /*
        var
        var loginRequest = _fixture.Create<LoginInput>();
        _mockUnitOfRepository.Setup(p => p.User.Where())
        */
        

        #endregion
    }
}

