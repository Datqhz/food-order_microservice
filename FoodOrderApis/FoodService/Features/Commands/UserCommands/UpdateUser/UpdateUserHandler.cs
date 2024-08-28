using FoodService.Repositories;
using MediatR;

namespace FoodService.Features.Commands.UserCommands.UpdateUser;

public class UpdateUserHandler : IRequestHandler<UpdateUserCommand>
{
    private readonly IUnitOfRepository _unitOfRepository;

    public UpdateUserHandler(IUnitOfRepository unitOfRepository)
    {
        _unitOfRepository = unitOfRepository;
    }
    public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var payload = request.Payload;
            var user = await _unitOfRepository.User.GetById(payload.UserId);
            if (user == null)
            {
                return;
            }

            user.DisplayName = payload.DisplayName;
            user.PhoneNumber = payload.PhoneNumber;
            _unitOfRepository.User.Update(user);
            await _unitOfRepository.CompleteAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
