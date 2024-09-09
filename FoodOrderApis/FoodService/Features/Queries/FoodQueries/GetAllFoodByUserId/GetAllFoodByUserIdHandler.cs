using FoodOrderApis.Common.Helpers;
using FoodService.Data.Models.Dtos;
using FoodService.Data.Responses;
using FoodService.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FoodService.Features.Queries.FoodQueries.GetAllFoodByUserId;

public class GetAllFoodByUserIdHandler : IRequestHandler<GetAllFoodByUserIdQuery, GetAllFoodByUserIdResponse>
{
    private readonly IUnitOfRepository _unitOfRepository;
    private readonly ILogger<GetAllFoodByUserIdHandler> _logger;

    public GetAllFoodByUserIdHandler(IUnitOfRepository unitOfRepository, ILogger<GetAllFoodByUserIdHandler> logger)
    {
        _unitOfRepository = unitOfRepository;
        _logger = logger;
    }
    public async Task<GetAllFoodByUserIdResponse> Handle(GetAllFoodByUserIdQuery request, CancellationToken cancellationToken)
    {
        var functionName = nameof(GetAllFoodByUserIdHandler);
        var response = new GetAllFoodByUserIdResponse(){StatusCode = (int)ResponseStatusCode.BadRequest};
        try
        {
            _logger.LogInformation($"{functionName} - Start");
            var foods = await _unitOfRepository.Food.Where(f => f.UserId == request.UserId).AsNoTracking().Select(_ =>
                new FoodDto
                {
                    Id = _.Id,
                    Name = _.Name,
                    Describe = _.Describe,
                    ImageUrl = _.ImageUrl,
                    Price = _.Price,
                }).ToListAsync(cancellationToken);
            response.StatusCode = (int)ResponseStatusCode.OK;
            response.StatusText = "Get All Food By UserId";
            response.Data = foods;
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
