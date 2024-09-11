using CustomerService.Repositories;
using MediatR;

namespace CustomerService.Features.Commands.UserInfoCommands.DeleteUser;

public class DeleteUserHandler : IRequestHandler<DeleteUserCommand>
{
    private readonly IUnitOfRepository _unitOfRepository;
    private readonly ILogger<DeleteUserHandler> _logger;

    public DeleteUserHandler(IUnitOfRepository unitOfRepository, ILogger<DeleteUserHandler> logger)
    {
        _unitOfRepository = unitOfRepository;
        _logger = logger;
    }
    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var functionName = nameof(DeleteUserCommand);
        var userId = request.UserId;
        try
        {
            _logger.LogInformation($"{functionName} - Start");
            var user = await _unitOfRepository.User.GetById(userId);
            if (user == null)
            {
                _logger.LogError($"{functionName} => User not found");
                return;
            }
            user.IsActive = false;
            var deleteResult = _unitOfRepository.User.Update(user);
            if (deleteResult)
            {
                await _unitOfRepository.CompleteAsync();
            }

            _logger.LogInformation($"{functionName} - End");
        }
        catch (Exception ex)
        {
            _logger.LogError($"{functionName} => Has error: Message = {ex.Message}");
        }
    }
}
