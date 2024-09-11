using FoodOrderApis.Common.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderService.Data.Responses;
using OrderService.Extensions;
using OrderService.Repositories;

namespace OrderService.Features.Queries.OrderQueries.GetOrderById;

public class GetOrderByIdHandler : IRequestHandler<GetOrderByIdQuery, GetOrderByIdResponse>
{
    private readonly IUnitOfRepository _unitOfRepository;
    private readonly ILogger<GetOrderByIdHandler> _logger;

    public GetOrderByIdHandler(IUnitOfRepository unitOfRepository, ILogger<GetOrderByIdHandler> logger)
    {
        _unitOfRepository = unitOfRepository;
        _logger = logger;
    }
    public async Task<GetOrderByIdResponse> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var functionName = nameof(GetOrderByIdHandler);
        var response = new GetOrderByIdResponse(){StatusCode = (int)ResponseStatusCode.BadRequest};
        _logger.LogInformation($"{functionName} - Start");
        try
        {
            var order = await _unitOfRepository.Order
                .Where(o => o.Id == request.Id)
                .AsNoTracking()
                .Include(o => o.Eater)
                .Include(o => o.Merchant)
                .FirstOrDefaultAsync(cancellationToken);
            if (order == null)
            {
                _logger.LogError($"{functionName} => Order not found");
                response.StatusCode = (int)ResponseStatusCode.NotFound;
                response.StatusText = "Order not found";
            }
            else
            {
                response.StatusCode = (int)ResponseStatusCode.OK;
                response.StatusText = "Get successful";
                response.Data = order.AsDto();
            }
            _logger.LogInformation($"{functionName} - End");
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError($"{functionName} => has error : Message = {ex.Message}");
            response.StatusCode = (int)ResponseStatusCode.InternalServerError;
            response.StatusText = "Internal Server Error";
            response.ErrorMessage = ex.Message;
            return response;
        }
    }
}
