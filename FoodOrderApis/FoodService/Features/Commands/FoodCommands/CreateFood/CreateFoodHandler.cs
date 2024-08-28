using System.Net;
using FoodService.Data.Models;
using FoodService.Data.Responses;
using FoodService.Repositories;
using MediatR;

namespace FoodService.Features.Commands.FoodCommands.CreateFood;

public class CreateFoodHandler : IRequestHandler<CreateFoodCommand, CreateFoodResponse>
{
    private readonly IUnitOfRepository _unitOfRepository;

    public CreateFoodHandler(IUnitOfRepository unitOfRepository)
    {
        _unitOfRepository = unitOfRepository;
    }
    public async Task<CreateFoodResponse> Handle(CreateFoodCommand request, CancellationToken cancellationToken)
    {
        var response = new CreateFoodResponse() {StatusCode = (int)HttpStatusCode.BadRequest};
        try
        {
            CreateFoodValidator validator = new CreateFoodValidator();
            var validationResult = validator.Validate(request);
            if (!validationResult.IsValid)
            {
                response.StatusText = validationResult.Errors[0].ErrorMessage;
                return response;
            }

            var payload = request.Payload;
            var food = new Food
            {
                Name = payload.Name,
                ImageUrl = payload.ImageUrl,
                Describe = payload.Describe,
                Price = payload.Price,
                UserId = payload.UserId
            };
            var createdFood = await _unitOfRepository.Food.Add(food);
            await _unitOfRepository.CompleteAsync();
            if (createdFood == null)
            {
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.StatusText = "Internal Server Error";
                return response;
            }
            else
            {
                response.StatusCode = (int)HttpStatusCode.Created;
                response.StatusText = "Food created successfully";
                return response;
            }
        }
        catch (Exception ex)
        {
            response.StatusCode = (int)HttpStatusCode.InternalServerError;
            response.StatusText = "Internal Server Error";
            response.ErrorMessage = ex.Message;
            return response;
        }
    }
}
