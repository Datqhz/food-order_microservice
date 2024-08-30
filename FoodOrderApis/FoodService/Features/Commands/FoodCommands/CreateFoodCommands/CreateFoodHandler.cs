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
    private readonly IBusControl _bus;

    public CreateFoodHandler(IUnitOfRepository unitOfRepository, IPublishEndpoint publishEndpoint, IBusControl bus)
    {
        _unitOfRepository = unitOfRepository;
        _publishEndpoint = publishEndpoint;
        _bus = bus;
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
                    Id = 1,
                    Name = createdFood.Name,
                    Describe = createdFood.Describe,
                    ImageUrl = createdFood.ImageUrl,
                };
                // Send message to topic and queue will be bind on it
                //wait _publishEndpoint.Publish(message); // auto create exchange(topic) if it doesn't exist
                var sendEndpoint = await _bus.GetSendEndpoint(new Uri($"queue:create-food"));//create new exchange with name is ...
                await sendEndpoint.Send(message, cancellationToken);
                // Send message to queue directly (queue)
                /*var sendEndpoint = await _bus.GetSendEndpoint(new Uri($"queue:order_service_create_food"));
                await sendEndpoint.Send<CreateFood>(message, cancellationToken);*/
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
