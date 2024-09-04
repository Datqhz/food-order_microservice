using FoodOrderApis.Common.Helpers;
using FoodOrderApis.Common.MassTransit.Contracts;
using FoodOrderApis.Common.MassTransit.Core;
using FoodService.Data.Responses;
using FoodService.Repositories;
using MassTransit;
using MediatR;

namespace FoodService.Features.Commands.FoodCommands.UpdateFoodCommands;

public class UpdateFoodHandler : IRequestHandler<UpdateFoodCommand, UpdateFoodResponse>
{
    private readonly IUnitOfRepository _unitOfRepository;
    private readonly ISendEndpointCustomProvider _sendEndpoint;

    public UpdateFoodHandler(IUnitOfRepository unitOfRepository, ISendEndpointCustomProvider sendEndpoint)
    {
        _unitOfRepository = unitOfRepository;
        _sendEndpoint = sendEndpoint;
    }
    public async Task<UpdateFoodResponse> Handle(UpdateFoodCommand request, CancellationToken cancellationToken)
    {
        var response = new UpdateFoodResponse() {StatusCode = (int)ResponseStatusCode.BadRequest};
        try
        {
            var validator = new UpdateFoodValidator();
            var validationResult = validator.Validate(request);
            if (!validationResult.IsValid)
            {
                response.StatusText = validationResult.ToString("~");
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
                await _sendEndpoint.SendMessage<UpdateFood>(new UpdateFood
                {
                    Id = food.Id,
                    Name = food.Name,
                    Describe = food.Describe,
                    ImageUrl = food.ImageUrl,
                }, cancellationToken, "order-update-food");
                
                response.StatusCode = (int)ResponseStatusCode.OK;
                response.StatusText = "Food updated";
            }
            else
            {
                response.StatusCode = (int)ResponseStatusCode.BadRequest;
                response.StatusText = "Failed to update food";
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
