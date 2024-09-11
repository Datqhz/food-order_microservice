using System.Security.Claims;
using System.Text.Json;
using FoodOrderApis.Common.Helpers;
using FoodOrderApis.Common.HttpContextCustom;
using FoodOrderApis.Common.MassTransit.Contracts;
using FoodOrderApis.Common.MassTransit.Core;
using FoodService.Data.InternalResponse;
using FoodService.Data.Responses;
using FoodService.Repositories;
using MediatR;

namespace FoodService.Features.Commands.FoodCommands.DeleteFoodCommands;

public class DeleteFoodHandler : IRequestHandler<DeleteFoodCommand, DeleteFoodResponse>
{
    private readonly IUnitOfRepository _unitOfRepository;
    private readonly ISendEndpointCustomProvider _sendEndpoint;
    private readonly ILogger<DeleteFoodHandler> _logger;
    private readonly ICustomHttpContextAccessor _httpContextAccessor;

    public DeleteFoodHandler(
        IUnitOfRepository unitOfRepository,
        ISendEndpointCustomProvider sendEndpoint, ILogger<DeleteFoodHandler> logger,
        ICustomHttpContextAccessor httpContextAccessor)
    {
        _unitOfRepository = unitOfRepository;
        _sendEndpoint = sendEndpoint;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task<DeleteFoodResponse> Handle(DeleteFoodCommand request, CancellationToken cancellationToken)
    {
        var functionName = nameof(DeleteFoodHandler);
        var response = new DeleteFoodResponse(){StatusCode = (int)ResponseStatusCode.BadRequest};
        try
        {
            _logger.LogInformation($"{functionName} - Start");
            var food = await _unitOfRepository.Food.GetById(request.Id);
            if (food == null)
            {
                _logger.LogError($"{functionName} - Food Not Found");
                response.StatusCode = (int)ResponseStatusCode.NotFound;
                response.StatusText = $"Food with ID {request.Id} does not exist";
                return response;
            }
            
            var currentUserId = _httpContextAccessor.GetCurrentUserId();
            if (currentUserId != food.UserId)
            {
                _logger.LogError($"{functionName} => Permission denied");
                response.StatusCode = (int)ResponseStatusCode.Forbidden;
                response.StatusText = $"Permission denied";
                return response;
            }
            using (HttpClient client = new HttpClient())
            {
                
                //string url = $"http://localhost:5149/api/v1/food/check-used?foodId={request.Id}";
                string url = $"http://orderservice:5149/api/v1/food/check-used?foodId={request.Id}";

                try
                {
                    HttpResponseMessage internalResponse = await client.GetAsync(url);
                    if (!internalResponse.IsSuccessStatusCode)
                    {
                        response.StatusCode = (int)internalResponse.StatusCode;
                        response.StatusText = "Something wrong went check condition";
                        return response;
                    }
                    else
                    {
                        var responseBody = await internalResponse.Content.ReadAsStringAsync();
                        var data = JsonSerializer.Deserialize<CheckFoodIsUsedResponse>(responseBody);
                        if (data.Data)
                        {
                            _logger.LogError($"{functionName} => This food is already used");
                            response.StatusCode = (int)ResponseStatusCode.BadRequest;
                            response.StatusText = "Can't delete this food";
                            return response;
                        }
                    }
                }
                catch (HttpRequestException e)
                {
                    _logger.LogError($"{functionName} => Has error : Message = {e.Message}");
                    response.StatusCode = (int)ResponseStatusCode.InternalServerError;
                    response.StatusText = "Something wrong went check condition";
                    return response;
                }
            }
            var deleteResult = _unitOfRepository.Food.Delete(food);
            await _unitOfRepository.CompleteAsync();
            if (deleteResult)
            {
                await _sendEndpoint.SendMessage<DeleteFood>(new DeleteFood
                {
                    FoodId = request.Id
                }, cancellationToken, "order-delete-food");
                response.StatusCode = (int)ResponseStatusCode.OK;
                response.StatusText = $"Food with ID {request.Id} has been deleted";
            }
            else
            {
                response.StatusCode = (int)ResponseStatusCode.BadRequest;
                response.StatusText = $"Food with ID {request.Id} has not been deleted";
            }
            _logger.LogInformation($"{functionName} - End");
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError($"{functionName} => Error : Message = {ex.Message}");
            response.StatusCode = (int)ResponseStatusCode.InternalServerError;
            response.StatusText = "Internal Server Error";
            response.ErrorMessage = ex.Message;
            return response;
        }
    }
}
