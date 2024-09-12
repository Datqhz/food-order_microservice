using FoodOrderApis.Common.Helpers;
using MediatR;
using OrderService.Data.Models;
using OrderService.Data.Responses;
using OrderService.Repositories;

namespace OrderService.Features.Commands.OrderCommands.CreateOrder;

public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, CreateOrderResponse>
{
    private readonly IUnitOfRepository _unitOfRepository;
    private readonly ILogger<CreateOrderHandler> _logger;

    public CreateOrderHandler(IUnitOfRepository unitOfRepository, ILogger<CreateOrderHandler> logger)
    {
        _unitOfRepository = unitOfRepository;
        _logger = logger;
    }
    public async Task<CreateOrderResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var functionName = nameof(CreateOrderHandler);
        var response = new CreateOrderResponse() {StatusCode = (int)ResponseStatusCode.BadRequest};
        try
        {
            _logger.LogInformation($"{functionName} - Start");
            var payload = request.Payload;
            var eater = await _unitOfRepository.User.GetById(payload.EaterId);
            if (eater == null)
            {
                _logger.LogError($"{functionName} => Eater not found");
                response.StatusCode = (int)ResponseStatusCode.NotFound;
                response.StatusText = "Eater not found";
                return response;
            }
            var merchant = await _unitOfRepository.User.GetById(payload.MerchantId);
            if (merchant == null)
            {
                _logger.LogError($"{functionName} => Merchant not found");
                response.StatusCode = (int)ResponseStatusCode.NotFound;
                response.StatusText = "Merchant not found";
                return response;
            }
            var newOrder = new Order
            {
                OrderStatus = 1,
                EaterId = payload.EaterId,
                MerchantId = payload.MerchantId,
                OrderedDate = DateTime.Now
            };
            var addResult = await _unitOfRepository.Order.Add(newOrder);
            if (addResult is null)
            {
                response.StatusText = "Order could not be added";
            }
            else
            {
                await _unitOfRepository.CompleteAsync();
                response.StatusCode = (int)ResponseStatusCode.Created;
                response.StatusText = "Order added";
            }
            _logger.LogInformation($"{functionName} - End");
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,$"{functionName} => Has error : Message = {ex.Message}");
            response.StatusCode = (int)ResponseStatusCode.InternalServerError;
            response.StatusText = "Internal Server Error";
            response.ErrorMessage = ex.Message;
            return response;
        }
    }
}
