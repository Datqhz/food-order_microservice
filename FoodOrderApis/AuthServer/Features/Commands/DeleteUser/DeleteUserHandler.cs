using System.Net;
using System.Security.Claims;
using AuthServer.Data.Responses;
using AuthServer.Repositories;
using FoodOrderApis.Common.Helpers;
using FoodOrderApis.Common.HttpContextCustom;
using FoodOrderApis.Common.MassTransit.Contracts;
using FoodOrderApis.Common.MassTransit.Core;
using MassTransit.Transports.Fabric;
using MediatR;

namespace AuthServer.Features.Commands.DeleteUser;

public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, DeleteUserResponse>
{

    private readonly IUnitOfRepository _unitOfRepository;
    private readonly ICustomHttpContextAccessor _httpContext;
    private readonly ILogger<DeleteUserHandler> _logger;
    private readonly ISendEndpointCustomProvider _sendEndpoint;
    
    public DeleteUserHandler(IUnitOfRepository unitOfRepository, ICustomHttpContextAccessor httpContext, ILogger<DeleteUserHandler> logger, ISendEndpointCustomProvider sendEndpoint)
    {
        _unitOfRepository = unitOfRepository;
        _httpContext = httpContext;
        _logger = logger;
        _sendEndpoint = sendEndpoint;
    }
    public async Task<DeleteUserResponse> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var funcName = nameof(DeleteUserHandler);
        _logger.LogInformation($"{funcName} =>");
        var response = new DeleteUserResponse(){StatusCode = (int)ResponseStatusCode.BadRequest};
        var userId = request.UserId;
        try
        {
            var userIdRequest = _httpContext.GetCurrentUserId();
            if (userIdRequest != userId)
            {
                _logger.LogError($"{funcName} => Permission denied");
                response.StatusCode = (int)ResponseStatusCode.Forbidden;
                response.StatusText = "You don't have permission to delete this user";
                return response;
            }
            var user = await _unitOfRepository.User.GetById(userId);
            if (user == null)
            {
                _logger.LogError($"{funcName} => User {userId} not found");
                response.StatusCode = (int)ResponseStatusCode.NotFound;
                response.StatusText = $"User not found";
                return response;
            }

            if (!user.IsActive)
            {
                _logger.LogError($"{funcName} => User already delete");
                response.StatusText = "This user is already deleted";
            }
            else
            {
                user.IsActive = false;
                var updateResult = _unitOfRepository.User.Update(user);
                if (updateResult)
                {
                    await _unitOfRepository.CompleteAsync();
                    await _sendEndpoint.SendMessage<DeleteUserInfo>(new DeleteUserInfo { UserId = user.Id }, cancellationToken, ExchangeType.Direct);
                    response.StatusCode = (int)ResponseStatusCode.OK;
                    response.StatusText = $"User deleted";
                }
                else
                {
                    throw new Exception("Can't delete user");
                }
            }
            _logger.LogInformation($"{funcName} - End");
            return response;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"{funcName} => Error: Message = {e.Message}");
            response.StatusCode = (int)ResponseStatusCode.InternalServerError;
            response.StatusText = "Interval server error";
            response.ErrorMessage = e.Message;
            return response;
        }
    }
}
