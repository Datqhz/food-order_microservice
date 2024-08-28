using FoodOrderApis.Common.Helpers;
using FoodService.Data.Responses;
using FoodService.Repositories;
using MediatR;

namespace FoodService.Features.Commands.FoodCommands.UpdateFood;

public class UpdateFoodHandler : IRequestHandler<UpdateFoodCommand, UpdateFoodResponse>
{
    private readonly IUnitOfRepository _unitOfRepository;

    public UpdateFoodHandler(IUnitOfRepository unitOfRepository)
    {
        _unitOfRepository = unitOfRepository;
    }
    public async Task<UpdateFoodResponse> Handle(UpdateFoodCommand request, CancellationToken cancellationToken)
    {
        var response = new UpdateFoodResponse() {StatusCode = (int)ResponseStatusCode.BadRequest};
        try
        {
            var validator = new UpdateFoodValidator();
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                response.StatusText = validationResult.Errors[0].ErrorMessage;
                return response;
            }

            var payload = request.Payload;
            var food = await _unitOfRepository.Food.GetById(payload.Id);
            if (food == null)
            {
                response.StatusCode = (int)ResponseStatusCode.NotFound;
                response.StatusText = "Food does not exist";
                return response;
            }

            food.Name = payload.Name;
            food.Price = payload.Price;
            food.Describe = payload.Describe;
            food.ImageUrl = payload.ImageUrl;
            var updateResult = _unitOfRepository.Food.Update(food);
            await _unitOfRepository.CompleteAsync();
            if (updateResult)
            {
                response.StatusCode = (int)ResponseStatusCode.OK;
                response.StatusText = "Food updated";
                return response;
            }
            else
            {
                response.StatusCode = (int)ResponseStatusCode.BadRequest;
                response.StatusText = "Failed to update food";
                return response;
            }
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
