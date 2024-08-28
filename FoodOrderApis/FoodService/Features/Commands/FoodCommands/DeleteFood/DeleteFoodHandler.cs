using FoodOrderApis.Common.Helpers;
using FoodService.Data.Responses;
using FoodService.Repositories;
using MediatR;

namespace FoodService.Features.Commands.FoodCommands.DeleteFood;

public class DeleteFoodHandler : IRequestHandler<DeleteFoodCommand, DeleteFoodResponse>
{
    private readonly IUnitOfRepository _unitOfRepository;

    public DeleteFoodHandler(IUnitOfRepository unitOfRepository)
    {
        _unitOfRepository = unitOfRepository;
    }
    public async Task<DeleteFoodResponse> Handle(DeleteFoodCommand request, CancellationToken cancellationToken)
    {
        var response = new DeleteFoodResponse(){StatusCode = (int)ResponseStatusCode.BadRequest};
        try
        {
            var validator = new DeleteFoodValidator();
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                response.StatusText = validationResult.Errors[0].ErrorMessage;
                return response;
            }

            var food = await _unitOfRepository.Food.GetById(request.Id);
            if (food == null)
            {
                response.StatusCode = (int)ResponseStatusCode.NotFound;
                response.StatusText = $"Food with ID {request.Id} does not exist";
                return response;
            }

            var deleteResult = _unitOfRepository.Food.Delete(food);
            await _unitOfRepository.CompleteAsync();
            if (deleteResult)
            {
                response.StatusCode = (int)ResponseStatusCode.OK;
                response.StatusText = $"Food with ID {request.Id} has been deleted";
                return response;
            }
            else
            {
                response.StatusCode = (int)ResponseStatusCode.BadRequest;
                response.StatusText = $"Food with ID {request.Id} has not been deleted";
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
