using FoodOrderApis.Common.Helpers;
using MediatR;
using OrderService.Data.Responses;
using OrderService.Enums;
using OrderService.Repositories;

namespace OrderService.Features.Commands.OrderDetailCommands.UpdateOrderDetail;

public class UpdateOrderDetailHandler : IRequestHandler<UpdateOrderDetailCommand, UpdateOrderDetailResponse>
{
    private readonly IUnitOfRepository _unitOfRepository;
    private readonly ILogger<UpdateOrderDetailHandler> _logger;

    public UpdateOrderDetailHandler(IUnitOfRepository unitOfRepository, ILogger<UpdateOrderDetailHandler> logger)
    {
        _unitOfRepository = unitOfRepository;
        _logger = logger;
    }
    public async Task<UpdateOrderDetailResponse> Handle(UpdateOrderDetailCommand request, CancellationToken cancellationToken)
    {
        var functionName = nameof(UpdateOrderDetailHandler);
        var response = new UpdateOrderDetailResponse() { StatusCode = (int)ResponseStatusCode.BadRequest };
        try
        {
            _logger.LogInformation($"{functionName} - Start");
            var payload = request.Payload;
            var orderDetail = await _unitOfRepository.OrderDetail.GetById(payload.OrderDetailId);
            if (orderDetail == null)
            {
                _logger.LogError($"{functionName} - Order detail could not be found");
                response.StatusCode = (int)ResponseStatusCode.NotFound;
                response.StatusText = $"Order detail with id {payload.OrderDetailId} does not exist";
            }
            else
            {   
                // 1 = create
                var modifyResult = false;
                if (payload.Feature == (int)ModifyFeature.Update) // update
                {
                    orderDetail.Price = payload.Price;
                    orderDetail.Quantity = payload.Quantity;
                    modifyResult = _unitOfRepository.OrderDetail.Update(orderDetail);
                }
                else if(payload.Feature == (int)ModifyFeature.Delete) // delete
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