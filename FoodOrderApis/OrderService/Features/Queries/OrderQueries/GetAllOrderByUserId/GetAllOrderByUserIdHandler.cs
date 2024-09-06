using FoodOrderApis.Common.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderService.Data.Models;
using OrderService.Data.Models.Dtos;
using OrderService.Data.Responses;
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
            var validator = new GetAllOrderByUserIdValidator();
            var validationResult = validator.Validate(request);
            if (!validationResult.IsValid)
            {
                _logger.LogError($"{functionName} => Invalid request : Message = {validationResult.ToString("-")}");
                response.StatusText = validationResult.ToString("~");
                return response;
            }
            List<Order> orders;
            if (request.EaterId != null)
            {
                orders = _unitOfRepository.Order.Where(o => o.EaterId == request.EaterId).AsNoTracking().ToList();
            }
            else if (request.MerchantId != null)
            {
                orders = _unitOfRepository.Order.Where(o => o.MerchantId == request.MerchantId).AsNoTracking().ToList();
            }
            else
            {
                orders = _unitOfRepository.Order.Where(o => o.MerchantId == request.MerchantId || o.EaterId == request.EaterId).AsNoTracking().ToList();
            }
            response.StatusCode = (int)ResponseStatusCode.OK;
            response.StatusText = "Get all orders by eaterid successfully";
            response.Data = orders.Select(o => o.AsDto()).ToList();
            _logger.LogInformation($"{functionName} - End");
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError($"{functionName} => Has error : Message =  {ex.Message}");
            response.StatusCode = (int)ResponseStatusCode.InternalServerError;
            response.StatusText = "Internal Server Error";
            response.ErrorMessage = ex.Message;
            return response;
        }
    }
}