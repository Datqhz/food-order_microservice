using System.Linq.Expressions;
using AuthServer.Data.Dtos.Responses;
using AuthServer.Data.Models;
using AuthServer.Data.Requests;
using AuthServer.Features.Commands.RegisterCommands;
using AuthServer.Repositories;
using AuthServer.Test.Extensions;
using AutoFixture;
using FoodOrderApis.Common.Helpers;
using FoodOrderApis.Common.MassTransit.Core;
using Microsoft.AspNetCore.Identity;
using MockQueryable;
using Moq;

namespace AuthServer.Test.Features.Commands.Register;

[TestFixture]
public class RegisterHanlderTests
{
    private readonly Mock<IUnitOfRepository> _mockUnitOfRepository;
    private readonly Mock<UserManager<User>> _mockUserManager;
    private readonly Fixture _fixture;
    private readonly RegisterHandler _handler;
    private readonly Mock<ISendEndpointCustomProvider> _mockSendEndpoint;
    private readonly CancellationToken _cancellationToken;

    public RegisterHanlderTests()
    {
        var userStoreMock = new Mock<IUserStore<User>>();
        _mockUnitOfRepository = new Mock<IUnitOfRepository>();
        _mockUserManager =
            new Mock<UserManager<User>>(userStoreMock.Object, null, null, null, null, null, null);
        _fixture = new Fixture().OmitOnRecursionBehavior();
        _mockSendEndpoint = new Mock<ISendEndpointCustomProvider>();
        _handler = new RegisterHandler(_mockUnitOfRepository.Object, _mockSendEndpoint.Object, _mockUserManager.Object);
        _cancellationToken = new CancellationToken();
    }

    [Test]
    public async Task Handler_ShouldReturn_StatusCreated()
    {
        // Arrange
        var clientId = _fixture.Create<string>();
        var client = _fixture.Build<Client>().With(c => c.ClientId == clientId).CreateMany(1).AsQueryable().BuildMock();
        var users = _fixture.Build<User>().CreateMany(0).AsQueryable().BuildMock();
        var request = new RegisterInput
        {
            Displayname = "abcd",
            ClientId = clientId,
            Password = "String123@",
            PhoneNumber = "0321232323",
            Username = "yamato"
        };
        var command = new RegisterCommand
        {
            Payload = request
        };
        _mockUnitOfRepository.Setup(p => p.Client.Where(It.IsAny<Expression<Func<Client, bool>>>())).Returns(client);
        _mockUserManager.Setup(p => p.Users.Where(It.IsAny<Expression<Func<User, bool>>>())).Returns(users);
        _mockUserManager.Setup(p => p.CreateAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Success);
        
        // Act
        var result = await _handler.Handle(command, _cancellationToken);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo((int)ResponseStatusCode.Created));
    }
}
