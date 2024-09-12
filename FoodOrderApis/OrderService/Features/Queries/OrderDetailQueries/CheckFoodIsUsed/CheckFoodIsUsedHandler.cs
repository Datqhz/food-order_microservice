using FoodOrderApis.Common.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderService.Data.Responses;
using OrderService.Repositories;

namespace OrderService.Features.Queries.OrderDetailQueries.CheckFoodIsUsed;

public class CheckFoodIsUsedHandler : IRequestHandler<CheckFoodIsUsedQuery, CheckFoodIsUsedResponse>
{
    private readonly IUnitOfRepository _unitOfRepository;
    private readonly ILogger<CheckFoodIsUsedHandler> _logger;

    public CheckFoodIsUsedHandler(IUnitOfRepository unitOfRepository, ILogger<CheckFoodIsUsedHandler> logger)
    {
        _unitOfRepository = unitOfRepository;
        _logger = logger;
    }
    public async Task<CheckFoodIsUsedResponse> Handle(CheckFoodIsUsedQuery request, CancellationToken cancellationToken)
    {
        var functionName = nameof(CheckFoodIsUsedHandler);
        var response  = new CheckFoodIsUsedResponse(){StatusCode = (int)ResponseStatusCode.InternalServerError};
        try
        {
            _logger.LogInformation($"{functionName} - Start");
            var details = await _unitOfRepository.OrderDetail.Where(d => d.FoodId == request.FoodId).AsNoTracking()
                .ToListAsync(cancellationToken);
            response.StatusCode = (int)ResponseStatusCode.OK;
            response.StatusText = "Check successfully";
            if (details is null || details.Count == 0)
            {
                response.Data = false;
            }
            else
            {
                response.Data = true;
            }
            _logger.LogInformation($"{functionName} - End");
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,$"{functionName} => Has error : Message = {ex.Message}");
            response.StatusText = "Internal Server Error";
            response.ErrorMessage = ex.Message;
            return response;
        }
    }
}
