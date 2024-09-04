using FoodOrderApis.Common.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderService.Data.Responses;
using OrderService.Repositories;

namespace OrderService.Features.Queries.OrderDetailQueries.CheckFoodIsUsed;

public class CheckFoodIsUsedHandler : IRequestHandler<CheckFoodIsUsedQuery, CheckFoodIsUsedResponse>
{
    private readonly IUnitOfRepository _unitOfRepository;

    public CheckFoodIsUsedHandler(IUnitOfRepository unitOfRepository)
    {
        _unitOfRepository = unitOfRepository;
    }
    public async Task<CheckFoodIsUsedResponse> Handle(CheckFoodIsUsedQuery request, CancellationToken cancellationToken)
    {
        var response  = new CheckFoodIsUsedResponse(){StatusCode = (int)ResponseStatusCode.InternalServerError};
        try
        {
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

            return response;
        }
        catch (Exception ex)
        {
            response.StatusText = "Internal Server Error";
            response.ErrorMessage = ex.Message;
            return response;
        }
    }
}
