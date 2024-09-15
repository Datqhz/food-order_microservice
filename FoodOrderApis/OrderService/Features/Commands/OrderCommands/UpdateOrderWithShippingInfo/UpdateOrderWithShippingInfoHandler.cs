using FoodOrderApis.Common.Helpers;
using FoodOrderApis.Common.HttpContextCustom;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderService.Data.Responses;
using OrderService.Enums;
using OrderService.Repositories;

namespace OrderService.Features.Commands.OrderCommands.UpdateOrderWithShippingInfo;

public class UpdateOrderWithShippingInfoHandler :IRequestHandler<UpdateOrderWithShippingInfoCommand, UpdateOrderResponse>
{
    private readonly IUnitOfRepository _unitOfRepository;
    private readonly ILogger<UpdateOrderWithShippingInfoHandler> _logger;
    private readonly ICustomHttpContextAccessor _httpContext;

    public UpdateOrderWithShippingInfoHandler(IUnitOfRepository unitOfRepository,
        ILogger<UpdateOrderWithShippingInfoHandler> logger,
        ICustomHttpContextAccessor httpContext)
    {
        _unitOfRepository = unitOfRepository;
        _logger = logger;
        _httpContext = httpContext;
    }
    
    public async Task<UpdateOrderResponse> Handle(UpdateOrderWithShippingInfoCommand request, CancellationToken cancellationToken)
    {
        var functionName = nameof(UpdateOrderWithShippingInfoHandler);
        _logger.LogInformation($"{functionName} - Start");
        var payload = request.Payload;
        var response = new UpdateOrderResponse(){StatusCode = (int)ResponseStatusCode.BadRequest};
        try
        {
            var order = await _unitOfRepository.Order
                .Where(o => o.Id == payload.OrderId && o.OrderStatus == (int)OrderStatus.Initialize).FirstOrDefaultAsync(cancellationToken);
            
            if (order == null)
            {
                _logger.LogError($"{functionName} => Order with ID {payload.OrderId} has initial status not found");
                response.StatusText = "Invalid Order";
            }

            var currentUid = _httpContext.GetCurrentUserId();
            if (currentUid != order.EaterId)
            {
                _logger.LogError($"{functionName} => Permission denied");
                response.StatusCode = (int)ResponseStatusCode.Forbidden;
                response.StatusText = "Permission Denied";
                return response;
            }
            order.OrderedDate = DateTime.UtcNow;
            order.ShippingAddress = payload.ShippingAddress;
            order.ShippingFee = payload.ShippingFee;
            order.ShippingPhoneNumber = payload.ShippingPhoneNumber;
            order.OrderStatus = (int)OrderStatus.Preparing;
            var updateResult = _unitOfRepository.Order.Update(order);
            if (!updateResult)
            {
                throw new Exception($"Update of Order with ID {payload.OrderId} failed");
            }
            await _unitOfRepository.CompleteAsync();
            response.StatusCode = (int)ResponseStatusCode.OK;
            response.StatusText = $"Order with ID {payload.OrderId} has been updated";
            _logger.LogInformation($"{functionName} - End");
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"{functionName} => has error : Message = {ex.Message}");
            response.StatusCode = (int)ResponseStatusCode.InternalServerError;
            response.StatusText = "Internal Server Error";
            response.ErrorMessage = ex.Message;
            return response;
        }
    }
}