using FoodOrderApis.Common.Helpers;
using MediatR;
using OrderService.Data.Models;
using OrderService.Data.Responses;
using OrderService.Repositories;

namespace OrderService.Features.Commands.OrderDetailCommands.CreateOrderDetail;

public class CreateOrderDetailHandler : IRequestHandler<CreateOrderDetailCommand, CreateOrderDetailResponse >
{
    private readonly IUnitOfRepository _unitOfRepository;
    private readonly ILogger<CreateOrderDetailHandler> _logger;

    public CreateOrderDetailHandler(IUnitOfRepository unitOfRepository, ILogger<CreateOrderDetailHandler> logger)
    {
        _unitOfRepository = unitOfRepository;
        _logger = logger;
    }
    public async Task<CreateOrderDetailResponse> Handle(CreateOrderDetailCommand request, CancellationToken cancellationToken)
    {
        var functionName = nameof(CreateOrderDetailHandler);
        var response = new CreateOrderDetailResponse(){StatusCode = (int)ResponseStatusCode.BadRequest};
        try
        {
            _logger.LogInformation($"{functionName} - Start");
            var validator = new CreateOrderDetailValidator();
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                _logger.LogError($"{functionName} => Invalid request : Message = {validationResult.ToString("-")}");
                response.StatusText = validationResult.ToString("~");
                return response;
            }
            var payload = request.Payload;
            var order = await _unitOfRepository.Order.GetById(payload.OrderId);
            if (order == null)
            {
                _logger.LogError($"{functionName} - Order not found");
                response.StatusCode = (int)ResponseStatusCode.NotFound;
                response.StatusText = $"Order not found";
                return response;
            }
            var food = await _unitOfRepository.Food.GetById(payload.FoodId);
            if (food == null)
            {
                _logger.LogError($"{functionName} - Food not found");
                response.StatusCode = (int)ResponseStatusCode.NotFound;
                response.StatusText = $"Food not found";
                return response;
            }
            var orderDetail = new OrderDetail
            {
                FoodId = payload.FoodId,
                OrderId = payload.OrderId,
                Quantity = payload.Quantity,
                Price = payload.Price,
            };
            var createResult = await _unitOfRepository.OrderDetail.Add(orderDetail);
            if (createResult != null)
            {
                
                await _unitOfRepository.CompleteAsync();
                response.StatusCode = (int)ResponseStatusCode.Created;
                response.StatusText = "Order Detail Created";
            }
            else
            {
                response.StatusText = "Order Detail Not Created";
            }
            _logger.LogInformation($"{functionName} - End");
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError($"{functionName} => Has error : Message = {ex.Message}");
            response.StatusCode = (int)ResponseStatusCode.InternalServerError;
            response.StatusText = "Internal Server Error";
            response.ErrorMessage = ex.Message;
            return response;
        }
    }
}