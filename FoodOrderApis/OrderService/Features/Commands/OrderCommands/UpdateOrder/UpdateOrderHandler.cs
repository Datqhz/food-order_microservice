using FoodOrderApis.Common.Helpers;
using FoodOrderApis.Common.HttpContextCustom;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderService.Data.Models;
using OrderService.Data.Responses;
using OrderService.Enums;
using OrderService.Repositories;

namespace OrderService.Features.Commands.OrderCommands.UpdateOrder;

public class UpdateOrderHandler : IRequestHandler<UpdateOrderCommand, UpdateOrderResponse>
{
    private readonly IUnitOfRepository _unitOfRepository;
    private readonly ILogger<UpdateOrderHandler> _logger;
    private readonly ICustomHttpContextAccessor _httpContext;

    public UpdateOrderHandler(IUnitOfRepository unitOfRepository, ILogger<UpdateOrderHandler> logger, ICustomHttpContextAccessor httpContext)
    {
        _unitOfRepository = unitOfRepository;
        _logger = logger;
        _httpContext = httpContext;
    }
    public async Task<UpdateOrderResponse> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var functionName = nameof(UpdateOrderHandler);
        var response = new UpdateOrderResponse(){StatusCode = (int)ResponseStatusCode.BadRequest};
        try
        {
            _logger.LogInformation($"{functionName} - Start");
            var payload = request.Payload;
            var order = await _unitOfRepository.Order
                .Where(o => o.Id == payload.OrderId && 
                            o.OrderStatus != (int)OrderStatus.Cancelled && 
                            o.OrderStatus != (int)OrderStatus.Initialize &&
                            o.OrderStatus != (int)OrderStatus.Received)
                .FirstOrDefaultAsync(cancellationToken);
            if (order == null)
            {
                _logger.LogError($"{functionName} => No matching order found");
                response.StatusCode = (int)ResponseStatusCode.NotFound;
                response.StatusText = $"No order matching found";
                return response;
            }
            var userId = _httpContext.GetCurrentUserId();
            if (payload.Cancellation == true)
            {
                if (order.EaterId != userId)
                {
                    _logger.LogError($"{functionName} => Permission denied");
                    response.StatusCode = (int)ResponseStatusCode.Forbidden;
                    response.StatusText = $"Permission denied";
                    return response;
                }
                order.OrderStatus = (int)OrderStatus.Cancelled;
            }
            else
            {
                if (order.MerchantId != userId)
                {
                    _logger.LogError($"{functionName} => Permission denied");
                    response.StatusCode = (int)ResponseStatusCode.Forbidden;
                    response.StatusText = $"Permission denied";
                    return response;
                }
                order.OrderedDate = DateTime.Now;
                order.OrderStatus += 1;
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
                throw new Exception("Order not updated");
            }
            _logger.LogInformation($"{functionName} - End");
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"{functionName} => Has error : Message = {ex.Message}");
            response.StatusCode = (int)ResponseStatusCode.InternalServerError;
            response.StatusText = "Internal Server Error";
            response.ErrorMessage = ex.Message;
            return response;
        }
    }
}
