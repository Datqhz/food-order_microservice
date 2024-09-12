using FoodOrderApis.Common.Helpers;
using MediatR;
using OrderService.Data.Models;
using OrderService.Data.Responses;
using OrderService.Enums;
using OrderService.Repositories;

namespace OrderService.Features.Commands.OrderDetailCommands.ModifyMultipleOrderDetail;

public class
    ModifyMultipleOrderDetailHandler : IRequestHandler<ModifyMultipleOrderDetailCommand,
    ModifyMultipleOrderDetailResponse>
{
    private readonly ILogger<ModifyMultipleOrderDetailHandler> _logger;
    private readonly IUnitOfRepository _unitOfRepository;

    public ModifyMultipleOrderDetailHandler(IUnitOfRepository unitOfRepository,
        ILogger<ModifyMultipleOrderDetailHandler> logger)
    {
        _unitOfRepository = unitOfRepository;
        _logger = logger;
    }

    public async Task<ModifyMultipleOrderDetailResponse> Handle(ModifyMultipleOrderDetailCommand request,
        CancellationToken cancellationToken)
    {
        var functionName = nameof(ModifyMultipleOrderDetailHandler);
        var response = new ModifyMultipleOrderDetailResponse { StatusCode = (int)ResponseStatusCode.BadRequest };
        var payload = request.Payload;
        await using (var transaction = await _unitOfRepository.OpenTransactionAsync())
        {
            try
            {
                _logger.LogInformation($"{functionName} - Start");

                foreach (var item in payload)
                    if (item.OrderDetailId != null)
                    {
                        var orderDetail = await _unitOfRepository.OrderDetail.GetById(item.OrderDetailId);
                        if (item.Feature == (int)ModifyFeature.Update)
                        {
                            orderDetail.Quantity = item.Quantity;
                            orderDetail.Price = item.Price;
                            _unitOfRepository.OrderDetail.Update(orderDetail);
                        }
                        else if (item.Feature == (int)ModifyFeature.Delete)
                        {
                            _unitOfRepository.OrderDetail.Delete(orderDetail);
                        }
                    }
                    else
                    {
                        if (item.Feature == (int)ModifyFeature.Create)
                        {
                            var newOrderDetail = new OrderDetail
                            {
                                FoodId = item.FoodId ?? 0,
                                OrderId = item.OrderId ?? 0,
                                Price = item.Price,
                                Quantity = item.Quantity
                            };
                            await _unitOfRepository.OrderDetail.Add(newOrderDetail);
                        }
                    }

                await _unitOfRepository.CompleteAsync();
                await _unitOfRepository.CommitAsync();
                
                response.StatusCode = (int)ResponseStatusCode.OK;
                response.StatusText = "Order details modified successfully.";
                _logger.LogInformation($"{functionName} - End");
                return response;
            }
            catch (Exception ex)
            {
                await _unitOfRepository.RollbackAsync();
                _logger.LogError(ex, $"{functionName} => Has error: Message = {ex.Message}");
                response.StatusCode = (int)ResponseStatusCode.InternalServerError;
                response.StatusText = "Internal Server Error";
                response.ErrorMessage = ex.Message;
                return response;
            }
        }
    }
}