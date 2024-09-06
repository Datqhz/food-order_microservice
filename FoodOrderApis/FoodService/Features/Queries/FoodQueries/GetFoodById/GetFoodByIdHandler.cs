using FoodOrderApis.Common.Helpers;
using FoodService.Data.Responses;
using FoodService.Extensions;
using FoodService.Repositories;
using MediatR;

namespace FoodService.Features.Queries.FoodQueries.GetFoodById;

public class GetFoodByIdHandler : IRequestHandler<GetFoodByIdQuery, GetFoodByIdResponse>
{
    private readonly IUnitOfRepository _unitOfRepository;
    private readonly ILogger<GetFoodByIdHandler> _logger;

    public GetFoodByIdHandler(IUnitOfRepository unitOfRepository, ILogger<GetFoodByIdHandler> logger)
    {
        _unitOfRepository = unitOfRepository;
        _logger = logger;
    }
    public async Task<GetFoodByIdResponse> Handle(GetFoodByIdQuery request, CancellationToken cancellationToken)
    {
        var functionName = nameof(GetFoodByIdHandler);
        var response = new GetFoodByIdResponse() {StatusCode = (int)ResponseStatusCode.NotFound};
        try
        {
            _logger.LogInformation($"{functionName} - Start");
            var food = await _unitOfRepository.Food.GetById(request.Id);
            if (food == null)
            {
                _logger.LogError($"{functionName} => Food not found");
                response.StatusText = "Food not found";
            }
            else
            {
                _logger.LogInformation($"{functionName} - End");
                response.StatusCode = (int)ResponseStatusCode.OK;
                response.StatusText = "Food retrieved";
                response.Data = food.AsDto();
            }
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
