using FoodOrderApis.Common.Helpers;
using MediatR;
using OrderService.Data.Models;
using OrderService.Data.Responses;
using OrderService.Enums;
using OrderService.Repositories;

namespace OrderService.Features.Commands.OrderCommands.UpdateOrder;

public class UpdateOrderHandler : IRequestHandler<UpdateOrderCommand, UpdateOrderResponse>
{
    private readonly IUnitOfRepository _unitOfRepository;
    private readonly ILogger<UpdateOrderHandler> _logger;

    public UpdateOrderHandler(IUnitOfRepository unitOfRepository, ILogger<UpdateOrderHandler> logger)
    {
        _unitOfRepository = unitOfRepository;
        _logger = logger;
    }
    public async Task<UpdateOrderResponse> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var functionName = nameof(UpdateOrderHandler);
        var response = new UpdateOrderResponse(){StatusCode = (int)ResponseStatusCode.BadRequest};
        try
        {
            _logger.LogInformation($"{functionName} - Start");
            var payload = request.Payload;
            var order = await _unitOfRepository.Order.GetById(payload.OrderId);
            if (order == null)
            {
                _logger.LogError($"{functionName} - Order not found");
                response.StatusCode = (int)ResponseStatusCode.NotFound;
                response.StatusText = $"Order with id {payload.OrderId} does not exist";
                return response;
            }
            order.OrderStatus = payload.OrderStatus;
            if (payload.OrderStatus == (int)OrderStatus.Preparing)
            {
                order.OrderedDate = DateTime.Now;
            }

            var saveResult = _unitOfRepository.Order.Update(order);
            if (saveResult)
            {
                await _unitOfRepository.CompleteAsync();
                response.StatusCode = (int)ResponseStatusCode.OK;
                response.StatusText = "Order successfully updated";
            }
            else
            {
                response.StatusText = "Order not updated";
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
