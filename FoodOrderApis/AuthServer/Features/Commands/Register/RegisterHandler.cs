using AuthServer.Data.Dtos.Responses;
using AuthServer.Data.Models;
using AuthServer.Repositories;
using FoodOrderApis.Common.Helpers;
using FoodOrderApis.Common.MassTransit.Contracts;
using FoodOrderApis.Common.MassTransit.Core;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthServer.Features.Commands.Register;

public class RegisterHandler : IRequestHandler<RegisterCommand, RegisterResponse>
{
    private readonly IUnitOfRepository _unitOfRepository;
    //private readonly IPublishEndpoint _publishEndpoint;
    private readonly ISendEndpointCustomProvider _sendEndpoint;
    private readonly UserManager<User> _userManager;
    public RegisterHandler(IUnitOfRepository unitOfRepository, ISendEndpointCustomProvider sendEndpoint, UserManager<User> userManager)
    {
        _unitOfRepository = unitOfRepository;
        _sendEndpoint = sendEndpoint;
        _userManager = userManager;
    }
    public async Task<RegisterResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var response = new RegisterResponse(){StatusCode = (int)ResponseStatusCode.BadRequest};
        var payload = request.Payload;
        try
        {
            RegisterValidator validator = new RegisterValidator();
            var validateResult = validator.Validate(request);
            if (!validateResult.IsValid)
            {
                response.StatusText = "Bad Request";
                response.ErrorMessage = validateResult.ToString("~");
                return response;
            }

            var clientId = await _unitOfRepository.Client
                .Where(c => c.ClientId == payload.ClientId)
                .AsNoTracking()
                .Select(_ => _.Id).FirstOrDefaultAsync(cancellationToken);
            if (clientId == 0)
            {
                response.StatusText = "Invalid information";
                return response;
            }

            var user = await _unitOfRepository.User.Where(u => u.UserName.ToLower() == payload.Username.ToLower())
                .AsNoTracking().FirstOrDefaultAsync(cancellationToken);
            if (user != null)
            {
                response.StatusText = "UserName is used";
                return response;
            }
            var userId = Guid.NewGuid().ToString();
            var newUser = new User
            {
                Id = userId,
                ClientId = clientId,
                Displayname = payload.Displayname,
                CreatedDate = DateTime.Now,
                UserName = payload.Username,
                PhoneNumber = payload.PhoneNumber,
            };
            
            var hashedPassword = new PasswordHasher<User>().HashPassword(newUser, payload.Password); 
            newUser.PasswordHash = hashedPassword;
            var createResult = await _userManager.CreateAsync(newUser);
            var createUserRole = await _userManager.AddToRoleAsync(newUser, payload.Role.ToUpper());
            if (!createResult.Succeeded || !createUserRole.Succeeded)
            {
                response.StatusText = "Bad request";
                response.ErrorMessage = createResult.Errors.ToString();
            }
            else
            {
                await _sendEndpoint.SendMessage<CreateUserInfo>(new CreateUserInfo
                {
                    UserId = userId,
                    UserName= payload.Username,
                    CreatedDate = DateTime.Now,
                    IsActive = true,
                    Role = payload.Role.ToUpper(),
                    DisplayName = payload.Displayname,
                    PhoneNumber = payload.PhoneNumber,
                }, cancellationToken, null);
                response.StatusText = "Created";
                response.StatusCode = (int)ResponseStatusCode.Created;
            }
            return response;
        }
        catch (Exception ex)
        {
            response.StatusCode = (int)ResponseStatusCode.InternalServerError;
            response.StatusText = "Internal server error";
            response.ErrorMessage = ex.Message;
            return response;
        }
    }
}
