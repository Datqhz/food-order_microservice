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
    private readonly ILogger<ModifyMultipleOrderDetailHandler> _logger;

    public ModifyMultipleOrderDetailHandler(IUnitOfRepository unitOfRepository,
        ILogger<ModifyMultipleOrderDetailHandler> logger)
    {
        _unitOfRepository = unitOfRepository;
        _logger = logger;
    }
    public async Task<ModifyMultipleOrderDetailResponse> Handle(ModifyMultipleOrderDetailCommand request, CancellationToken cancellationToken)
    {
        var functionName = nameof(ModifyMultipleOrderDetailHandler);
        var response = new ModifyMultipleOrderDetailResponse() {StatusCode = (int)ResponseStatusCode.BadRequest};
        var payload = request.Payload;
        try
        {
            _logger.LogInformation($"{functionName} - Start");
            var validator = new ModifyMultipleOrderDetailValidator();
            var validationResult = validator.Validate(request);
            if (!validationResult.IsValid)
            {
                _logger.LogError($"{functionName} => Invalid request : Message = {validationResult.ToString("-")}");
                response.StatusText = validationResult.ToString("~");
                return response;
            }

            using (var transaction = _unitOfRepository.OpenTransactionAsync())
            {
                foreach (var item in payload)
                {
                    var orderDetail = await _unitOfRepository.OrderDetail.GetById(item.OrderDetailId);
                    if (orderDetail == null)
                    {
                        await _unitOfRepository.RollbackAsync();
                        response.StatusCode = (int)ResponseStatusCode.NotFound;
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
                await _unitOfRepository.CommitAsync();
                await _unitOfRepository.CompleteAsync();
            }
            response.StatusCode = (int)ResponseStatusCode.OK;
            response.StatusText = $"Order details modified successfully.";
            _logger.LogInformation($"{functionName} - End");
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError($"{functionName} => Has error: Message = {ex.Message}");
            response.StatusCode = (int)ResponseStatusCode.InternalServerError;
            response.StatusText = "Internal Server Error";
            response.ErrorMessage = ex.Message;
            return response;
        }
    }
}