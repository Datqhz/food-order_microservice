using FoodOrderApis.Common.Helpers;
using FoodOrderApis.Common.HttpContextCustom;
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
    private readonly ICustomHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<UpdateFoodHandler> _logger;

    public UpdateFoodHandler(
        IUnitOfRepository unitOfRepository, 
        ISendEndpointCustomProvider sendEndpoint, 
        ICustomHttpContextAccessor httpContextAccessor,
        ILogger<UpdateFoodHandler> logger)
    {
        _unitOfRepository = unitOfRepository;
        _sendEndpoint = sendEndpoint;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }
    public async Task<UpdateFoodResponse> Handle(UpdateFoodCommand request, CancellationToken cancellationToken)
    {
        var functionName = nameof(UpdateFoodHandler);
        var response = new UpdateFoodResponse() {StatusCode = (int)ResponseStatusCode.BadRequest};
        try
        {
            _logger.LogInformation($"{functionName} - Start");
            var validator = new UpdateFoodValidator();
            var validationResult = validator.Validate(request);
            if (!validationResult.IsValid)
            {
                _logger.LogError($"{functionName} => Invalid request : Message = {validationResult.ToString("-")}");
                response.StatusText = validationResult.ToString("~");
                return response;
            }
            
            var payload = request.Payload;
            var food = await _unitOfRepository.Food.GetById(payload.Id);
            if (food == null)
            {
                _logger.LogError($"{functionName} - No food found");
                response.StatusCode = (int)ResponseStatusCode.NotFound;
                response.StatusText = "Food does not exist";
                return response;
            }
            var currentUserId = _httpContextAccessor.GetCurrentUserId();
            if (food.UserId != currentUserId)
            {
                _logger.LogError($"{functionName} - Permission denied");
                response.StatusCode = (int)ResponseStatusCode.Forbidden;
                response.StatusText = "You don't have permission to update this food";
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
            _logger.LogInformation($"{functionName} - End");
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError($"{functionName} => Has error : Message = {ex.Message}");
            response.StatusCode = (int)ResponseStatusCode.InternalServerError;
            response.StatusText = "Internal Server Error";
            response.ErrorMessage = ex.Message;
            return response;
        }
    }
}
