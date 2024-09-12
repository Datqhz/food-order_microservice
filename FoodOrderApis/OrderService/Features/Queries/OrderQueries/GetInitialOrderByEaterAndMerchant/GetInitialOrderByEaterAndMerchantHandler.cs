using FoodOrderApis.Common.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderService.Data.Models;
using OrderService.Data.Responses;
using OrderService.Enums;
using OrderService.Extensions;
using OrderService.Repositories;

namespace OrderService.Features.Queries.OrderQueries.GetInitialOrderByEaterAndMerchant;

public class GetInitialOrderByEaterAndMerchantHandler : IRequestHandler<GetInitialOrderByEaterAndMerchantQuery, GetInitialOrderByEaterAndMerchantResponse>
{

    private readonly IUnitOfRepository _unitOfRepository;
    private readonly ILogger<GetInitialOrderByEaterAndMerchantHandler> _logger;

    public GetInitialOrderByEaterAndMerchantHandler(IUnitOfRepository unitOfRepository,
        ILogger<GetInitialOrderByEaterAndMerchantHandler> logger)
    {
        _unitOfRepository = unitOfRepository;
        _logger = logger;
    }
    
    public async Task<GetInitialOrderByEaterAndMerchantResponse> Handle(GetInitialOrderByEaterAndMerchantQuery request, CancellationToken cancellationToken)
    {
        var functionName = nameof(GetInitialOrderByEaterAndMerchantQuery);
        _logger.LogInformation($"{functionName} - Start");
        var payload = request.Payload;
        var response = new GetInitialOrderByEaterAndMerchantResponse(){StatusCode = (int)ResponseStatusCode.BadRequest};
        try
        {

            var eater = await _unitOfRepository.User.GetById(payload.EaterId);
            if (eater == null)
            {
                _logger.LogError($"{functionName} => Eater not found");
                response.StatusText = "Eater doesn't exist";
                return response;
            }

            var merchant = await _unitOfRepository.User.GetById(payload.MerchantId);
            if (merchant == null)
            {
                _logger.LogError($"{functionName} => Eater not found");
                response.StatusText = "Merchant doesn't exist";
                return response;
            }
            var order = await _unitOfRepository.Order
                .Where(o => o.MerchantId == payload.MerchantId && o.EaterId == payload.EaterId &&
                            o.OrderStatus == (int)OrderStatus.Initialize).AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);
            if (order == null)
            {
                order = new Order
                {
                    OrderStatus = (int)OrderStatus.Initialize,
                    EaterId = payload.EaterId,
                    MerchantId = payload.MerchantId,
                    OrderedDate = DateTime.Now,

                };
                var createdOrder = await _unitOfRepository.Order.Add(order);
                if (createdOrder != null)
                {
                    await _unitOfRepository.CompleteAsync();
                    response.StatusCode = (int)ResponseStatusCode.OK;
                    response.StatusText = "Get order successfully";
                    response.Data = createdOrder.AsDto();
                }
                else
                {
                    response.StatusText = "Get order failed";
                }
            }
            else
            {
                response.StatusCode = (int)ResponseStatusCode.OK;
                response.StatusText = "Get order successfully";
                response.Data = order.AsDto();
            }
            _logger.LogInformation($"{functionName} - End");
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,$"{functionName} - Error: {ex.Message}");
            response.StatusText = "Internal Server Error";
            response.StatusCode = (int)ResponseStatusCode.InternalServerError;
            response.ErrorMessage = ex.Message;
            return response;
        }
    }
}
