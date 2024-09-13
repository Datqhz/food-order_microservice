using AuthServer.Data.Dtos.Responses;
using AuthServer.Data.Models;
using AuthServer.Repositories;
using FoodOrderApis.Common.Helpers;
using FoodOrderApis.Common.MassTransit.Contracts;
using FoodOrderApis.Common.MassTransit.Core;
using MassTransit;
using MassTransit.Transports.Fabric;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthServer.Features.Commands.Register;

public class RegisterHandler : IRequestHandler<RegisterCommand, RegisterResponse>
{
    private readonly IUnitOfRepository _unitOfRepository;
    private readonly ISendEndpointCustomProvider _sendEndpoint;
    private readonly UserManager<User> _userManager;
    private readonly ILogger<RegisterHandler> _logger;
    public RegisterHandler(IUnitOfRepository unitOfRepository,
        ISendEndpointCustomProvider sendEndpoint,
        UserManager<User> userManager, 
        ILogger<RegisterHandler> logger
        )
    {
        _unitOfRepository = unitOfRepository;
        _sendEndpoint = sendEndpoint;
        _userManager = userManager;
        _logger = logger;
    }
    public async Task<RegisterResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var functionName = nameof(RegisterHandler);
        _logger.LogInformation($"{functionName} - Start");
        var response = new RegisterResponse(){StatusCode = (int)ResponseStatusCode.BadRequest};
        var payload = request.Payload;
        try
        {
            var clientId = await _unitOfRepository.Client
                .Where(c => c.ClientId == payload.ClientId)
                .AsNoTracking()
                .Select(_ => _.Id).FirstOrDefaultAsync(cancellationToken);
            if (clientId == 0)
            {
                _logger.LogError($"{functionName} => Client {payload.ClientId} not found");
                response.StatusText = "Invalid information";
                return response;
            }

            var user = await _unitOfRepository.User.Where(u => u.UserName.ToLower() == payload.Username.ToLower())
                .AsNoTracking().FirstOrDefaultAsync(cancellationToken);
            if (user != null)
            {
                _logger.LogError($"{functionName} => UserName is already in used");
                response.StatusText = "UserName is already in used";
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
                _logger.LogError($"{functionName} => Failed to create user : Message = {createResult.Errors.ToString() + createUserRole.Errors.ToString()}");
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
                }, cancellationToken, ExchangeType.Topic);
                response.StatusText = "Created";
                response.StatusCode = (int)ResponseStatusCode.Created;
            }
            _logger.LogInformation($"{functionName} - End");
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"{functionName} => Error : Message = {ex.Message}");
            response.StatusCode = (int)ResponseStatusCode.InternalServerError;
            response.StatusText = "Internal server error";
            response.ErrorMessage = ex.Message;
            return response;
        }
    }
}
