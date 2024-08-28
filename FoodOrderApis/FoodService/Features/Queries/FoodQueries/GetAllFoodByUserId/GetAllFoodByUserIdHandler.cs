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

    public GetAllFoodByUserIdHandler(IUnitOfRepository unitOfRepository)
    {
        _unitOfRepository = unitOfRepository;
    }
    public async Task<GetAllFoodByUserIdResponse> Handle(GetAllFoodByUserIdQuery request, CancellationToken cancellationToken)
    {
        var response = new GetAllFoodByUserIdResponse(){StatusCode = (int)ResponseStatusCode.BadRequest};
        try
        {
            var validator = new GetAllFoodByUserIdValidator();
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                response.StatusText = validationResult.Errors[0].ErrorMessage;
                return response;
            }

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
            return response;
        }
        catch (Exception ex)
        {
            response.StatusCode = (int)ResponseStatusCode.InternalServerError;
            response.StatusText = "Internal Server Error";
            response.ErrorMessage = ex.Message;
            return response;
        }
    }
}
