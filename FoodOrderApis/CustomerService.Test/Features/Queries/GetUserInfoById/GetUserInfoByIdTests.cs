using AutoFixture;
using CustomerService.Data.Models;
using CustomerService.Features.Queries.UserInfoQueries.GetUserInfoById;
using CustomerService.Repositories;
using CustomerService.Test.Extensions;
using FoodOrderApis.Common.Helpers;
using MockQueryable;
using Moq;

namespace CustomerService.Test.Features.Queries.GetUserInfoById;

public class GetUserInfoByIdTests
{
    private readonly Mock<IUnitOfRepository> _unitOfRepositoryMock;
    private readonly Fixture _fixture;
    private readonly CancellationToken _cancellationToken;
    private readonly GetUserByIdHandler _handler;

    public GetUserInfoByIdTests()
    {
        _unitOfRepositoryMock = new Mock<IUnitOfRepository>();
        _fixture = new Fixture().OmitOnRecursionBehavior();
        _cancellationToken = new CancellationToken();
        _handler = new GetUserByIdHandler(_unitOfRepositoryMock.Object);
    }

    [Test]
    public async Task Hanlde_ShoutReturn_StatusOK()
    {
        var userId = _fixture.Create<Guid>().ToString();
        var user = _fixture.Build<UserInfo>().With(x => x.Id, userId).Create();

        _unitOfRepositoryMock.Setup(p => p.User.GetById(It.IsAny<string>())).ReturnsAsync(user);
        
        var query = new GetUserByIdQuery{ UserId = userId };
        // Act
        var result = await _handler.Handle(query, _cancellationToken);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Data, Is.Not.Null);
    }
    
    [Test]
    public async Task Hanlde_ShoutReturn_StatusNotFound()
    {
        var userId = _fixture.Create<Guid>().ToString();

        _unitOfRepositoryMock.Setup(p => p.User.GetById(It.IsAny<string>())).ReturnsAsync((UserInfo)null);
        
        var query = new GetUserByIdQuery{ UserId = userId };
        // Act
        var result = await _handler.Handle(query, _cancellationToken);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo((int)ResponseStatusCode.NotFound));
    }
    [Test]
    public async Task Hanlde_ShoutReturn_StatusInternalServerError()
    {
        var userId = _fixture.Create<Guid>().ToString();
        var user = _fixture.Build<UserInfo>().With(x => x.Id, userId).Create();

        _unitOfRepositoryMock.Setup(p => p.User.GetById(It.IsAny<string>())).ThrowsAsync(new Exception());
        
        var query = new GetUserByIdQuery{ UserId = userId };
        // Act
        var result = await _handler.Handle(query, _cancellationToken);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo((int)ResponseStatusCode.InternalServerError));
    }
}
