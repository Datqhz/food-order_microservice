using FoodOrderApis.Common.Helpers;
using MediatR;
using OrderService.Data.Responses;
using OrderService.Repositories;

namespace OrderService.Features.Commands.OrderDetailCommands.UpdateOrderDetail;

public class UpdateOrderDetailHandler : IRequestHandler<UpdateOrderDetailCommand, UpdateOrderDetailResponse>
{
    private readonly IUnitOfRepository _unitOfRepository;
    public async Task<UpdateOrderDetailResponse> Handle(UpdateOrderDetailCommand request, CancellationToken cancellationToken)
    {
        var response = new UpdateOrderDetailResponse() { StatusCode = (int)ResponseStatusCode.BadRequest };
        try
        {
            var validator = new UpdateOrderDetailValidator();
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                response.StatusText = validationResult.Errors.First().ErrorMessage;
                return response;
            }

            var payload = request.Payload;
            var orderDetail = await _unitOfRepository.OrderDetail.GetById(payload.OrderDetailId);
            if (orderDetail == null)
            {
                response.StatusCode = (int)ResponseStatusCode.NotFound;
                response.StatusText = $"Order detail with id {payload.OrderDetailId} does not exist";
            }
            else
            {   
                // 1 = create
                var modifyResult = false;
                if (payload.Feature == 2) // update
                {
                    orderDetail.Price = payload.Price;
                    orderDetail.Quantity = payload.Quantity;
                    modifyResult = _unitOfRepository.OrderDetail.Update(orderDetail);
                }
                else if(payload.Feature == 3) // delete
                {
                    modifyResult = _unitOfRepository.OrderDetail.Delete(orderDetail);
                }

                if (modifyResult)
                {
                    await _unitOfRepository.CompleteAsync();
                    response.StatusCode = (int)ResponseStatusCode.OK;
                    response.StatusText = $"Order detail successfully updated";
                }
                else
                {
                    response.StatusText = $"Order detail could not be updated";
                }
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