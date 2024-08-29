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

    public UpdateOrderHandler(IUnitOfRepository unitOfRepository)
    {
        _unitOfRepository = unitOfRepository;
    }
    public async Task<UpdateOrderResponse> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var response = new UpdateOrderResponse(){StatusCode = (int)ResponseStatusCode.BadRequest};
        try
        {
            var validator = new UpdateOrderValidator();
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                response.StatusText = validationResult.ToString("~");
                return response;
            }

            var payload = request.Payload;
            var order = await _unitOfRepository.Order.GetById(payload.OrderId);
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
            return response;
        }
        catch (Exception ex)
        {
            response.StatusCode = (int)ResponseStatusCode.InternalServerError;
            response.StatusText = "Internal Server Error";
            response.ErrorMessage = ex.Message;
            return response;
        }
    }
}
