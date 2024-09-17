using System.Linq.Expressions;
using AutoFixture;
using CustomerService.Data.Models;
using CustomerService.Data.Requests;
using CustomerService.Features.Queries.UserInfoQueries.GetAllMerchant;
using CustomerService.Repositories;
using CustomerService.Test.Extensions;
using FoodOrderApis.Common.Helpers;
using Microsoft.Extensions.Logging;
using MockQueryable;
using Moq;

namespace CustomerService.Test.Features.Queries.GetAllMerchant;

[TestFixture]
public class GetAllMerchantHandlerTests
{
    private readonly Mock<IUnitOfRepository> _unitOfRepositoryMock; 
    private readonly Mock<ILogger<GetAllMerchantHandler>> _loggerMock;
    private readonly Fixture _fixture;
    private readonly GetAllMerchantHandler _handler;
    private readonly CancellationToken _cancellationToken;

    public GetAllMerchantHandlerTests()
    {
        _unitOfRepositoryMock = new Mock<IUnitOfRepository>();
        _loggerMock = new Mock<ILogger<GetAllMerchantHandler>>();
        _fixture = new Fixture().OmitOnRecursionBehavior();
        _cancellationToken = new CancellationToken();
        _handler = new GetAllMerchantHandler(_unitOfRepositoryMock.Object, _loggerMock.Object);
    }

    [Test]
    public async Task Handle_ShouldReturn_StatusOK()
    {
        // Arrange
        var merchants = _fixture.Build<UserInfo>()
            .With(u => u.IsActive, true)
            .CreateMany(10)
            .AsQueryable()
            .BuildMock();

        _unitOfRepositoryMock.Setup(p => p.User.Where(It.IsAny<Expression<Func<UserInfo, bool>>>())).Returns(merchants);
        
        var query = new GetAllMerchantQuery()
        {
            Payload = new GetAllMerchantRequest()
        };
        // Act
        var result = await _handler.Handle(query, _cancellationToken);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo((int)ResponseStatusCode.OK));
    }
    
    [Test]
    public async Task Handle_ShouldReturn_StatusInternalServerError()
    {
        // Arrange

        _unitOfRepositoryMock.Setup(p => p.User.Where(It.IsAny<Expression<Func<UserInfo, bool>>>())).Throws(new Exception());
        
        var query = new GetAllMerchantQuery()
        {
            Payload = new GetAllMerchantRequest()
        };
        // Act
        var result = await _handler.Handle(query, _cancellationToken);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo((int)ResponseStatusCode.InternalServerError));
    }
}
