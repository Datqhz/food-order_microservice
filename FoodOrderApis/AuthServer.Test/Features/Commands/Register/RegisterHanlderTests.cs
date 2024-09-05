using System.Linq.Expressions;
using AuthServer.Data.Dtos.Responses;
using AuthServer.Data.Models;
using AuthServer.Data.Requests;
using AuthServer.Features.Commands.Register;
using AuthServer.Repositories;
using AuthServer.Test.Extensions;
using AutoFixture;
using FoodOrderApis.Common.Helpers;
using FoodOrderApis.Common.MassTransit.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MockQueryable;
using Moq;

namespace AuthServer.Test.Features.Commands.Register;

[TestFixture]
public class RegisterHanlderTests
{
    private readonly Mock<IUnitOfRepository> _unitOfRepositoryMock;
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Fixture _fixture;
    private readonly RegisterHandler _handler;
    private readonly Mock<ISendEndpointCustomProvider> _sendEndpointMock;
    private readonly CancellationToken _cancellationToken;

    public RegisterHanlderTests()
    {
        _unitOfRepositoryMock = new Mock<IUnitOfRepository>();
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
        _fixture = new Fixture().OmitOnRecursionBehavior();
        _sendEndpointMock = new Mock<ISendEndpointCustomProvider>();
        _handler = new RegisterHandler(_unitOfRepositoryMock.Object, _sendEndpointMock.Object, _userManagerMock.Object);
        _cancellationToken = new CancellationToken();
    }

    [Test]
    public async Task Handle_ShouldReturn_StatusCreated()
    {
        // Arrange
        var clientId = _fixture.Create<string>();
        var clients = _fixture.Build<Client>().With(c => c.ClientId, clientId).CreateMany(1).AsQueryable().BuildMock();
        var users = _fixture.Build<User>().CreateMany(0).AsQueryable().BuildMock();
        var request = new RegisterInput
        {
            Displayname = "abcd",
            ClientId = clientId,
            Password = "String123@",
            PhoneNumber = "0321232323",
            Role = "EATER",
            Username = "yamato"
        };
        var command = new RegisterCommand
        {
            Payload = request
        };
        _unitOfRepositoryMock.Setup(p => p.Client.Where(It.IsAny<Expression<Func<Client, bool>>>())).Returns(clients);
        _unitOfRepositoryMock.Setup(p => p.User.Where(It.IsAny<Expression<Func<User, bool>>>())).Returns(users);
        _userManagerMock.Setup(p => p.CreateAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Success);
        _userManagerMock.Setup(p => p.AddToRoleAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
        
        // Act
        var result = await _handler.Handle(command, _cancellationToken);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo((int)ResponseStatusCode.Created));
    }
    
    [Test]
    public async Task Handle_ShouldReturn_StatusBadRequest_When_Invalid_ClientId()
    {
        // Arrange
        var clients = _fixture.Build<Client>().CreateMany(0).AsQueryable().BuildMock();
        var request = new RegisterInput
        {
            Displayname = "abcd",
            ClientId = "nbnb",
            Password = "String123@",
            PhoneNumber = "0321232323",
            Role = "EATER",
            Username = "yamato"
        };
        var command = new RegisterCommand
        {
            Payload = request
        };
        _unitOfRepositoryMock.Setup(p => p.Client.Where(It.IsAny<Expression<Func<Client, bool>>>())).Returns(clients);
        
        // Act
        var result = await _handler.Handle(command, _cancellationToken);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo((int)ResponseStatusCode.BadRequest));
    }
    
    [Test]
    public async Task Handle_ShouldReturn_StatusBadRequest_When_UserName_Is_Used()
    {
        // Arrange
        var clientId = _fixture.Create<string>();
        var clients = _fixture.Build<Client>().With(c => c.ClientId, clientId).CreateMany(1).AsQueryable().BuildMock();
        var users = _fixture.Build<User>().CreateMany(1).AsQueryable().BuildMock();
        var request = new RegisterInput
        {
            Displayname = "abcd",
            ClientId = clientId,
            Password = "String123@",
            PhoneNumber = "0321232323",
            Role = "EATER",
            Username = "yamato"
        };
        var command = new RegisterCommand
        {
            Payload = request
        };
        _unitOfRepositoryMock.Setup(p => p.Client.Where(It.IsAny<Expression<Func<Client, bool>>>())).Returns(clients);
        _unitOfRepositoryMock.Setup(p => p.User.Where(It.IsAny<Expression<Func<User, bool>>>())).Returns(users);
        
        // Act
        var result = await _handler.Handle(command, _cancellationToken);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo((int)ResponseStatusCode.BadRequest));
    }
    
    [Test]
    public async Task Handle_ShouldReturn_StatusInternalServerError()
    {
        // Arrange
        var clientId = _fixture.Create<string>();
        var clients = _fixture.Build<Client>().With(c => c.ClientId, clientId).CreateMany(1).AsQueryable().BuildMock();
        var users = _fixture.Build<User>().CreateMany(0).AsQueryable().BuildMock();
        var request = new RegisterInput
        {
            Displayname = "abcd",
            ClientId = clientId,
            Password = "String123@",
            PhoneNumber = "0321232323",
            Role = "EATER",
            Username = "yamato"
        };
        var command = new RegisterCommand
        {
            Payload = request
        };
        _unitOfRepositoryMock.Setup(p => p.Client.Where(It.IsAny<Expression<Func<Client, bool>>>())).Throws(new Exception("Error"));
        
        // Act
        var result = await _handler.Handle(command, _cancellationToken);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo((int)ResponseStatusCode.InternalServerError));
    }
}
