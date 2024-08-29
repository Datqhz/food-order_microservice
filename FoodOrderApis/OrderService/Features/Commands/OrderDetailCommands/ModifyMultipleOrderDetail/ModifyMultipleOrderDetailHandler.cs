using FoodOrderApis.Common.Helpers;
using MediatR;
using OrderService.Data.Models;
using OrderService.Data.Responses;
using OrderService.Enums;
using OrderService.Repositories;

namespace OrderService.Features.Commands.OrderDetailCommands.ModifyMultipleOrderDetail;

public class ModifyMultipleOrderDetailHandler :IRequestHandler<ModifyMultipleOrderDetailCommand, ModifyMultipleOrderDetailResponse>
{
    private readonly IUnitOfRepository _unitOfRepository;
    public async Task<ModifyMultipleOrderDetailResponse> Handle(ModifyMultipleOrderDetailCommand request, CancellationToken cancellationToken)
    {
        var response = new ModifyMultipleOrderDetailResponse() {StatusCode = (int)ResponseStatusCode.BadRequest};
        var payload = request.Payload;
        try
        {
            var validator = new ModifyMultipleOrderDetailValidator();
            var validationResult = validator.Validate(request);
            if (!validationResult.IsValid)
            {
                response.StatusText = validationResult.ToString("~");
                return response;
            }

            foreach (var item in payload)
            {
                var orderDetail = await _unitOfRepository.OrderDetail.GetById(item.OrderDetailId);
                if (orderDetail == null)
                {
                    response.StatusText += $"Order detail with id {item.OrderDetailId} does not exist.\n";
                    return response;
                }
                if (item.Feature == (int)ModifyFeature.Update)
                {
                        orderDetail.Quantity = item.Quantity;
                        orderDetail.Price = item.Price;
                        _unitOfRepository.OrderDetail.Update(orderDetail);
                }else if (item.Feature == (int)ModifyFeature.Delete)
                {
                    _unitOfRepository.OrderDetail.Delete(orderDetail);
                }
            }

            await _unitOfRepository.CompleteAsync();
            response.StatusCode = (int)ResponseStatusCode.OK;
            response.StatusText = $"Order detail modified successfully.";
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