using System.Text.Json;
using FoodOrderApis.Common.Helpers;
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

    public DeleteFoodHandler(IUnitOfRepository unitOfRepository, ISendEndpointCustomProvider sendEndpoint)
    {
        _unitOfRepository = unitOfRepository;
        _sendEndpoint = sendEndpoint;
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
                response.StatusText = validationResult.ToString("~");
                return response;
            }

            var food = await _unitOfRepository.Food.GetById(request.Id);
            if (food == null)
            {
                response.StatusCode = (int)ResponseStatusCode.NotFound;
                response.StatusText = $"Food with ID {request.Id} does not exist";
                return response;
            }
            using (HttpClient client = new HttpClient())
            {
                
                string url = $"http://localhost:5149/api/v1/food/check-used?foodId={request.Id}";
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
                            response.StatusCode = (int)ResponseStatusCode.BadRequest;
                            response.StatusText = "Can't delete this food";
                            return response;
                        }
                    }
                }
                catch (HttpRequestException e)
                {
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
