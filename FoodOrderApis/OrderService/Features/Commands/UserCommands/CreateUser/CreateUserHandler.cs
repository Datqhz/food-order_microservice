using MediatR;
using OrderService.Data.Models;
using OrderService.Features.Commands.UserCommands.CreateUser;
using OrderService.Repositories;

namespace OrderService.Features.Commands.UserCommands.CreateUser;

public class CreateUserHandler : IRequestHandler<CreateUserCommand>
{
    private readonly IUnitOfRepository _unitOfRepository;

    public CreateUserHandler(IUnitOfRepository unitOfRepository)
    {
        _unitOfRepository = unitOfRepository;
    }
    public async Task Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var payload = request.Payload;
            var user = new User
            {
                UserId = payload.UserId,
                DisplayName = payload.DisplayName,
                PhoneNumber = payload.PhoneNumber
            };
            await _unitOfRepository.User.Add(user);
            await _unitOfRepository.CompleteAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
