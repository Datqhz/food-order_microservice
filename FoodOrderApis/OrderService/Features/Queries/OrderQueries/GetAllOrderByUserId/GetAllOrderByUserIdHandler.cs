using System.Diagnostics;
using FoodOrderApis.Common.Enums;
using FoodOrderApis.Common.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderService.Data.Models;
using OrderService.Data.Models.Dtos;
using OrderService.Data.Responses;
using OrderService.Enums;
using OrderService.Extensions;
using OrderService.Repositories;

namespace OrderService.Features.Queries.OrderQueries.GetAllOrderByUserId;

public class GetAllOrderByUserIdHandler : IRequestHandler<GetAllOrderByUserIdQuery, GetAllOrderByUserIdResponse>
{
    private readonly IUnitOfRepository _unitOfRepository;
    private readonly ILogger<GetAllOrderByUserIdHandler> _logger;

    public GetAllOrderByUserIdHandler(IUnitOfRepository unitOfRepository, ILogger<GetAllOrderByUserIdHandler> logger)
    {
        _unitOfRepository = unitOfRepository;
        _logger = logger;
    }
    public async Task<GetAllOrderByUserIdResponse> Handle(GetAllOrderByUserIdQuery request, CancellationToken cancellationToken)
    {
        var functionName = nameof(GetAllOrderByUserIdQuery);
        var response = new GetAllOrderByUserIdResponse() {StatusCode = (int)ResponseStatusCode.BadRequest};
        try
        {
            _logger.LogInformation($"{functionName} - Start");
            var payload = request.Payload;
            var orders = _unitOfRepository.Order
                .Where(o => o.OrderStatus == payload.OrderStatus)
                .Include(o => o.Eater)
                .Include(o => o.Merchant)
                .AsNoTracking();
            if (payload.EaterId != null)
            {
                orders = orders.Where(o => o.EaterId == payload.EaterId);
            }
            else
            {
                orders = orders.Where(o => o.MerchantId == payload.MerchantId);
            }
            switch (payload.SortBy)
            {
                case SortOption.ByDateDescending:
                    orders = orders.OrderByDescending(o => o.OrderedDate);
                        
                    break;
                default:
                    orders = orders.OrderBy(o => o.OrderedDate);
                    break;
            }
            response.StatusCode = (int)ResponseStatusCode.OK;
            response.StatusText = "Get all orders by successfully";
            response.Data = await orders.Select(o => o.AsDto()).ToListAsync(cancellationToken);
            _logger.LogInformation($"{functionName} - End");
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,$"{functionName} => Has error : Message =  {ex.Message}");
            response.StatusCode = (int)ResponseStatusCode.InternalServerError;
            response.StatusText = "Internal Server Error";
            response.ErrorMessage = ex.Message;
            return response;
        }
    }
}