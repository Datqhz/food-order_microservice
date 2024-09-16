using System.Net;
using FoodOrderApis.Common.MassTransit.Contracts;
using FoodOrderApis.Common.MassTransit.Core;
using FoodService.Data.Models;
using FoodService.Data.Responses;
using FoodService.Repositories;
using MassTransit;
using MassTransit.Transports.Fabric;
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
        var payload = request.Payload;
        try
        {
            _logger.LogInformation($"{functionName} - Start");
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
                throw new Exception("Can't create food");
            }
            var message = new CreateFood
            {
                Id = createdFood.Id,
                Name = createdFood.Name,
                Describe = createdFood.Describe,
                ImageUrl = createdFood.ImageUrl,
            };
            await _sendEndpoint.SendMessage<CreateFood>(message, cancellationToken, ExchangeType.Direct);
            response.StatusCode = (int)HttpStatusCode.Created;
            response.StatusText = "Food created successfully";
            _logger.LogInformation($"{functionName} - End");
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"{functionName} => Has error : Message = {ex.Message}");
            response.StatusCode = (int)HttpStatusCode.InternalServerError;
            response.StatusText = "Internal Server Error";
            response.ErrorMessage = ex.Message;
            return response;
        }
    }
}
