using AuthServer.Data.Models;
using AuthServer.Data.Requests;
using AuthServer.Features.Commands.Login;
using AuthServer.Repositories;
using AuthServer.Test.Extensions;
using AutoFixture;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MockQueryable;
using Moq;

namespace AuthServer.Test.Features.Commands.Login;

public class LoginHandlerTests
{
    private readonly Mock<IUnitOfRepository> _unitOfRepositoryMock;
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Fixture _fixture;
    private readonly LoginHandler _handler;
    private readonly CancellationToken _cancellationToken;

    public LoginHandlerTests()
    {
        _unitOfRepositoryMock = new Mock<IUnitOfRepository>();
        _fixture = new Fixture().OmitOnRecursionBehavior();
        _cancellationToken = new CancellationToken();
        _userManagerMock  = new Mock<UserManager<User>>(
            new Mock<IUserStore<User>>().Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<IPasswordHasher<User>>().Object,
            new IUserValidator<User>[0],
            new IPasswordValidator<User>[0],
            new Mock<ILookupNormalizer>().Object,
            new Mock<IdentityErrorDescriber>().Object,
            new Mock<IServiceProvider>().Object,
            new Mock<ILogger<UserManager<User>>>().Object);
        _handler = new LoginHandler(_unitOfRepositoryMock.Object, _userManagerMock.Object);
    }

    [Test]
    public async Task Handle_ShouldReturn_StatusOK()
    {
        // Arrange
        var input = new LoginInput
        {
            Username = "yamato",
            Password = "String123@"
        };
        var role = "Admin";
        var users = _fixture.Build<User>().With(u => u.UserName, input.Username).With(u => u.IsActive, true).CreateMany(1).AsQueryable()
            .BuildMock();
        var roles = _fixture.Build<string>().CreateMany(1).ToList();
        var clients = _fixture.Build<Client>().CreateMany(1).AsQueryable().BuildMock();
        

    }
}
