using AuthServer.Data.Dtos.Responses;
using AuthServer.Data.Models;
using AuthServer.Repositories;
using FoodOrderApis.Common.Helpers;
using FoodOrderApis.Common.MassTransit;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthServer.Features.Commands.RegisterCommands;

public class RegisterHandler : IRequestHandler<RegisterCommand, RegisterResponse>
{
    private readonly IUnitOfRepository _unitOfRepository;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly UserManager<User> _userManager;
    private readonly IBusControl _busControl;

    public RegisterHandler(IUnitOfRepository unitOfRepository, IPublishEndpoint publishEndpoint, UserManager<User> userManager, IBusControl busControl)
    {
        _unitOfRepository = unitOfRepository;
        _publishEndpoint = publishEndpoint;
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
                response.ErrorMessage = "Invalid information";
                return response;
            }

            var clientId = await _unitOfRepository.Client
                .Where(c => c.ClientId == payload.ClientId)
                .AsNoTracking()
                .Select(_ => _.Id).FirstOrDefaultAsync(cancellationToken);
            if (clientId == 0)
            {
                response.StatusText = "Bad Request";
                response.ErrorMessage = "Invalid information";
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
            if (!createResult.Succeeded)
            {
                response.StatusCode = (int)ResponseStatusCode.InternalServerError;
                response.StatusText = "Internal server error";
                response.ErrorMessage = createResult.Errors.ToString();
            }
            else
            {
                await _publishEndpoint.Publish(new CreateUserInfo
                {
                    UserId = userId,
                    UserName= payload.Username,
                    CreatedDate = DateTime.Now,
                    IsActive = true,
                    ClientId = clientId,
                    DisplayName = payload.Displayname,
                    PhoneNumber = payload.PhoneNumber,
                });
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
