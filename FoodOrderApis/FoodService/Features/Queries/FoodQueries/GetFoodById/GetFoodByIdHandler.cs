using FoodOrderApis.Common.Helpers;
using FoodService.Data.Responses;
using FoodService.Extensions;
using FoodService.Repositories;
using MediatR;

namespace FoodService.Features.Queries.FoodQueries.GetFoodById;

public class GetFoodByIdHandler : IRequestHandler<GetFoodByIdQuery, GetFoodByIdResponse>
{
    private readonly IUnitOfRepository _unitOfRepository;

    public GetFoodByIdHandler(IUnitOfRepository unitOfRepository)
    {
        _unitOfRepository = unitOfRepository;
    }
    public async Task<GetFoodByIdResponse> Handle(GetFoodByIdQuery request, CancellationToken cancellationToken)
    {
        var response = new GetFoodByIdResponse() {StatusCode = (int)ResponseStatusCode.NotFound};
        try
        {
            var food = await _unitOfRepository.Food.GetById(request.Id);
            if (food == null)
            {
                response.StatusText = "Food not found";
            }
            else
            {
                response.StatusCode = (int)ResponseStatusCode.OK;
                response.StatusText = "Food retrieved";
                response.Data = food.AsDto();
            }
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
