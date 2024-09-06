using System.Net;
using FoodOrderApis.Common.MassTransit.Contracts;
using FoodOrderApis.Common.MassTransit.Core;
using FoodService.Data.Models;
using FoodService.Data.Responses;
using FoodService.Repositories;
using MassTransit;
using MediatR;

namespace FoodService.Features.Commands.FoodCommands.CreateFoodCommands;

public class CreateFoodHandler : IRequestHandler<CreateFoodCommand, CreateFoodResponse>
{
    private readonly IUnitOfRepository _unitOfRepository;
    private readonly ISendEndpointCustomProvider _sendEndpoint;
    private readonly ILogger<CreateFoodHandler> _logger;

    public CreateFoodHandler(IUnitOfRepository unitOfRepository,  ISendEndpointCustomProvider sendEndpoint, ILogger<CreateFoodHandler> logger)
    {
        _unitOfRepository = unitOfRepository;
        _sendEndpoint = sendEndpoint;
        _logger = logger;
    }
    public async Task<CreateFoodResponse> Handle(CreateFoodCommand request, CancellationToken cancellationToken)
    {
        var functionName = nameof(CreateFoodHandler);
        var response = new CreateFoodResponse() {StatusCode = (int)HttpStatusCode.BadRequest};
        try
        {
            _logger.LogInformation($"{functionName} - Start");
            CreateFoodValidator validator = new CreateFoodValidator();
            var validationResult = validator.Validate(request);
            if (!validationResult.IsValid)
            {
                _logger.LogError($"{functionName} => Invalid request : Message = {validationResult.ToString("-")}");
                response.StatusText = validationResult.ToString("~");
                return response;
            }
            var payload = request.Payload;
            var user = await _unitOfRepository.User.GetById(payload.UserId);
            if (user == null)
            {
                _logger.LogError($"{functionName} - User not found");
                response.StatusCode = (int)HttpStatusCode.NotFound;
                response.StatusText = "User does not exist";
                return response;
            }
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
                await _sendEndpoint.SendMessage<CreateFood>(message, cancellationToken, "order-create-food");
                response.StatusCode = (int)HttpStatusCode.Created;
                response.StatusText = "Food created successfully";
            }
            _logger.LogInformation($"{functionName} - End");
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError($"{functionName} => Has error : Message = {ex.Message}");
            response.StatusCode = (int)HttpStatusCode.InternalServerError;
            response.StatusText = "Internal Server Error";
            response.ErrorMessage = ex.Message;
            return response;
        }
    }
}
