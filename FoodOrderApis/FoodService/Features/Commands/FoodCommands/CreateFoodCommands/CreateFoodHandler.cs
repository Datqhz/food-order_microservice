using System.Net;
using FoodOrderApis.Common.MassTransit.Contracts;
using FoodService.Data.Models;
using FoodService.Data.Responses;
using FoodService.Repositories;
using MassTransit;
using MediatR;

namespace FoodService.Features.Commands.FoodCommands.CreateFoodCommands;

public class CreateFoodHandler : IRequestHandler<CreateFoodCommand, CreateFoodResponse>
{
    private readonly IUnitOfRepository _unitOfRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    public CreateFoodHandler(IUnitOfRepository unitOfRepository, IPublishEndpoint publishEndpoint)
    {
        _unitOfRepository = unitOfRepository;
        _publishEndpoint = publishEndpoint;
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
                response.StatusText = validationResult.ToString("~");
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
            }
            else
            {
                var message = new CreateFood
                {
                    Id = createdFood.Id,
                    Name = createdFood.Name,
                    Describe = createdFood.Describe,
                    ImageUrl = createdFood.ImageUrl,
                };
                await _publishEndpoint.Publish(message);
                response.StatusCode = (int)HttpStatusCode.Created;
                response.StatusText = "Food created successfully";
            }
            return response;
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
