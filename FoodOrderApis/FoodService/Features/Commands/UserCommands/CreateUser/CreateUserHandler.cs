using FoodService.Consumers;
using FoodService.Data.Models;
using FoodService.Repositories;
using MediatR;

namespace FoodService.Features.Commands.UserCommands.CreateUser;

public class CreateUserHandler : IRequestHandler<CreateUserCommand>
{
    private readonly IUnitOfRepository _unitOfRepository;
    private readonly ILogger<CreateUserHandler> _logger;

    public CreateUserHandler(IUnitOfRepository unitOfRepository, ILogger<CreateUserHandler> logger)
    {
        _unitOfRepository = unitOfRepository;
        _logger = logger;
    }
    public async Task Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var functionName = nameof(CreateUserHandler);
        try
        {
            _logger.LogInformation($"{functionName} - Start");
            var payload = request.Payload;
            var user = new User
            {
                UserId = payload.UserId,
                DisplayName = payload.DisplayName,
                PhoneNumber = payload.PhoneNumber
            };
            await _unitOfRepository.User.Add(user);
            await _unitOfRepository.CompleteAsync();
            _logger.LogInformation($"{functionName} - End");
        }
        catch (Exception ex)
        {
            _logger.LogError($"{functionName} => Has error : Message = {ex.Message}");
        }
    }
}
