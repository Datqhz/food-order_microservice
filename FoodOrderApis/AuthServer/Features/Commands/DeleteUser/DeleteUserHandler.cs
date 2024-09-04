using System.Net;
using System.Security.Claims;
using AuthServer.Data.Responses;
using AuthServer.Repositories;
using FoodOrderApis.Common.Helpers;
using MediatR;

namespace AuthServer.Features.Commands.DeleteUser;

public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, DeleteUserResponse>
{

    private readonly IUnitOfRepository _unitOfRepository;
    /*private readonly ClaimsPrincipal _user;*/

    public DeleteUserHandler(IUnitOfRepository unitOfRepository)
    {
        _unitOfRepository = unitOfRepository;
        /*_user = user;*/
    }
    public async Task<DeleteUserResponse> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var response = new DeleteUserResponse(){StatusCode = (int)ResponseStatusCode.BadRequest};
        var userId = request.UserId;
        /*var userIdRequest = _user.FindFirst("sub")?.Value;*/
        try
        {
            var validator = new DeleteUserValidator();
            var validationResult = validator.Validate(request);
            if (!validationResult.IsValid)
            {
                response.StatusText = validationResult.ToString("-");
                return response;
            }
            /*if (userIdRequest != userId)
            {
                response.StatusCode = (int)ResponseStatusCode.Forbidden;
                response.StatusText = "You don't have permission to delete this user";
                return response;
            }*/
            var user = await _unitOfRepository.User.GetById(userId);
            if (user == null)
            {
                response.StatusCode = (int)ResponseStatusCode.NotFound;
                response.StatusText = $"User not found";
                return response;
            }

            if (!user.IsActive)
            {
                response.StatusText = "This user is already deleted";
            }

            user.IsActive = false;
            var updateResult = _unitOfRepository.User.Update(user);
            if (updateResult)
            {
                await _unitOfRepository.CompleteAsync();
                response.StatusCode = (int)ResponseStatusCode.OK;
                response.StatusText = $"User updated";
                ;
            }
            else
            {
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.StatusText = "Can't delete user";
            }

            return response;
        }
        catch (Exception e)
        {
            response.StatusCode = (int)ResponseStatusCode.InternalServerError;
            response.StatusText = "Interval server error";
            response.ErrorMessage = e.Message;
            return response;
        }
    }
}
