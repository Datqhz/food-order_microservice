using System.Linq.Expressions;
using AutoFixture;
using FoodOrderApis.Common.Helpers;
using Microsoft.Extensions.Logging;
using MockQueryable;
using Moq;
using OrderService.Data.Models;
using OrderService.Data.Requests;
using OrderService.Data.Responses;
using OrderService.Enums;
using OrderService.Features.Queries.OrderQueries.GetInitialOrderByEaterAndMerchant;
using OrderService.Repositories;
using OrderService.Test.Extensions;

namespace OrderService.Test.Features.Queries.GetInitialOrderByEaterAndMerchant;

[TestFixture]
public class GetInitialOrderByEaterAndMerchantHandlerTests
{
    private readonly Mock<IUnitOfRepository> _unitOfRepositoryMock;
    private readonly Mock<ILogger<GetInitialOrderByEaterAndMerchantHandler>> _loggerMock;
    private readonly CancellationToken _cancellationToken;
    private readonly Fixture _fixture;
    private readonly GetInitialOrderByEaterAndMerchantHandler _handler;

    public GetInitialOrderByEaterAndMerchantHandlerTests()
    {
        _unitOfRepositoryMock = new Mock<IUnitOfRepository>();
        _loggerMock = new Mock<ILogger<GetInitialOrderByEaterAndMerchantHandler>>();
        _cancellationToken = new CancellationToken();
        _fixture = new Fixture().OmitOnRecursionBehavior();
        _handler = new GetInitialOrderByEaterAndMerchantHandler(_unitOfRepositoryMock.Object, _loggerMock.Object);
    }

    [Test]
    public async Task Handle_ShouldReturn_StatusOK_WhenOrderExists()
    {
        // Arrange
        var eater = _fixture.Build<User>()
            .Create();
        var merchant = _fixture.Build<User>()
            
            .Create();
        var order = _fixture.Build<Order>()
            .With(x => x.OrderStatus, (int)OrderStatus.Initialize)
            .With(x => x.EaterId, eater.UserId)
            .With(x => x.MerchantId, merchant.UserId)
            .CreateMany(1).AsQueryable()
            .BuildMock();
        
        _unitOfRepositoryMock.Setup(p => p.User.GetById(eater.UserId)).ReturnsAsync(eater);
        _unitOfRepositoryMock.Setup(p => p.User.GetById(merchant.UserId)).ReturnsAsync(eater);
        _unitOfRepositoryMock.Setup(p => p.Order.Where(It.IsAny<Expression<Func<Order, bool>>>())).Returns(order);

        var input = new GetInitialOrderByEaterAndMerchantInput
        {
            EaterId = eater.UserId,
            MerchantId = merchant.UserId,
        };
        var query = new GetInitialOrderByEaterAndMerchantQuery
        {
            Payload = input
        };
        // Act
        var result = await _handler.Handle(query, _cancellationToken);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo((int)ResponseStatusCode.OK));
        
    }
    
    [Test]
    public async Task Handle_ShouldReturn_StatusOK_WhenOrderDoesNotExists()
    {
        // Arrange
        var eater = _fixture.Build<User>()
            .Create();
        var merchant = _fixture.Build<User>()
            .Create();
        var orders = _fixture.Build<Order>()
            .With(x => x.OrderStatus, (int)OrderStatus.Initialize)
            .With(x => x.EaterId, eater.UserId)
            .With(x => x.MerchantId, merchant.UserId)
            .CreateMany(0).AsQueryable()
            .BuildMock();
        
        _unitOfRepositoryMock.Setup(p => p.User.GetById(eater.UserId)).ReturnsAsync(eater);
        _unitOfRepositoryMock.Setup(p => p.User.GetById(merchant.UserId)).ReturnsAsync(eater);
        _unitOfRepositoryMock.Setup(p => p.Order.Where(It.IsAny<Expression<Func<Order, bool>>>())).Returns(orders);
        _unitOfRepositoryMock.Setup(p => p.Order.Add(It.IsAny<Order>())).ReturnsAsync(new Order());

        var input = new GetInitialOrderByEaterAndMerchantInput
        {
            EaterId = eater.UserId,
            MerchantId = merchant.UserId,
        };
        var query = new GetInitialOrderByEaterAndMerchantQuery
        {
            Payload = input
        };
        // Act
        var result = await _handler.Handle(query, _cancellationToken);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo((int)ResponseStatusCode.OK));
    }
    
    [Test]
    public async Task Handle_ShouldReturn_StatusBadRequest_WhenEaterDoesNotExists()
    {
        // Arrange
        
        _unitOfRepositoryMock.Setup(p => p.User.GetById(It.IsAny<string>())).ReturnsAsync((User) null);

        var input = new GetInitialOrderByEaterAndMerchantInput
        {
            EaterId = "aaaa",
            MerchantId = "dddd",
        };
        var query = new GetInitialOrderByEaterAndMerchantQuery
        {
            Payload = input
        };
        // Act
        var result = await _handler.Handle(query, _cancellationToken);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo((int)ResponseStatusCode.BadRequest));
    }
    
    [Test]
    public async Task Handle_ShouldReturn_StatusBadRequest_WhenMerchantDoesNotExists()
    {
        // Arrange
        var eater = _fixture.Build<User>()
            .Create();
        
        _unitOfRepositoryMock.Setup(p => p.User.GetById(eater.UserId)).ReturnsAsync(eater);
        _unitOfRepositoryMock.Setup(p => p.User.GetById(It.IsAny<string>())).ReturnsAsync((User) null);

        var input = new GetInitialOrderByEaterAndMerchantInput
        {
            EaterId = eater.UserId,
            MerchantId = "dddd",
        };
        var query = new GetInitialOrderByEaterAndMerchantQuery
        {
            Payload = input
        };
        // Act
        var result = await _handler.Handle(query, _cancellationToken);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo((int)ResponseStatusCode.BadRequest));
    }
    
    [Test]
    public async Task Handle_ShouldReturn_StatusInternalServerError()
    {
        // Arrange
        
        _unitOfRepositoryMock.Setup(p => p.User.GetById(It.IsAny<string>())).ThrowsAsync(new Exception());

        var input = new GetInitialOrderByEaterAndMerchantInput
        {
            EaterId = "aaaa",
            MerchantId = "dddd",
        };
        var query = new GetInitialOrderByEaterAndMerchantQuery
        {
            Payload = input
        };
        // Act
        var result = await _handler.Handle(query, _cancellationToken);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo((int)ResponseStatusCode.InternalServerError));
    }
}
