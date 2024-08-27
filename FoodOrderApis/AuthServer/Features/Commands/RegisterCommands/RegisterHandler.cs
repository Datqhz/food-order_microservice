using AuthServer.Data.Models;
using AuthServer.Repositories;
using FoodOrderApis.Common.MassTransit.Consumers;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthServer.Features.Commands.RegisterCommands;

public class RegisterHandler : IRequestHandler<RegisterCommand, ObjectResult>
{
    private readonly IUnitOfRepository _unitOfRepository;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly UserManager<User> _userManager;

    public RegisterHandler(IUnitOfRepository unitOfRepository, IPublishEndpoint publishEndpoint, UserManager<User> userManager)
    {
        _unitOfRepository = unitOfRepository;
        _publishEndpoint = publishEndpoint;
        _userManager = userManager;
    }
    public async Task<ObjectResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var payload = request.Payload;
        try
        {
            RegisterValidator validator = new RegisterValidator();
            var validateResult = validator.Validate(request);
            if (!validateResult.IsValid)
            {
                return new ObjectResult
                (new
                    {
                        status = StatusCodes.Status400BadRequest,
                        statusText = "Bad request",
                        message = "Invalid information"
                    }
                ) { StatusCode = StatusCodes.Status400BadRequest };
            }

            var clientId = await _unitOfRepository.Client
                .Where(c => c.ClientId == payload.ClientId)
                .AsNoTracking()
                .Select(_ => _.Id).FirstOrDefaultAsync(cancellationToken);
            if (clientId == 0)
            {
                return new ObjectResult
                (new
                    {
                        status = StatusCodes.Status400BadRequest,
                        statusText = "Bad request",
                        message = "Invalid information"
                    }
                ) { StatusCode = StatusCodes.Status400BadRequest };
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
                return new ObjectResult
                (new
                    {
                        status = StatusCodes.Status500InternalServerError,
                        statusText = "Internal Server Error",
                        message = createResult.ToString()
                    }
                ){StatusCode = StatusCodes.Status500InternalServerError};
            }
            
            /*var hasher = new PasswordHasher<User>();
            string hashedPassword = hasher.HashPassword(newUser, payload.Password);
            newUser.PasswordHash = hashedPassword;
            var createResult = await _unitOfRepository.User.Add(newUser);
            await _unitOfRepository.CompleteAsync();*/
            _publishEndpoint.Publish(new CreateAccount
            {
                AccountId = userId,
                Username = payload.Username,
                CreatedDate = DateTime.Now,
                PhoneNumber = payload.PhoneNumber,
            });
            return new ObjectResult
            (new
                {
                    status = StatusCodes.Status201Created,
                    statusText = "Created",
                    message = "Create account successful"
                }
            ) { StatusCode = StatusCodes.Status201Created };
        }
        catch (Exception ex)
        {
            return new ObjectResult
            (new
                {
                    status = StatusCodes.Status500InternalServerError,
                    statusText = "Internal Server Error",
                    message = ex.ToString()
                }
            ){StatusCode = StatusCodes.Status500InternalServerError};
        }
    }
}
