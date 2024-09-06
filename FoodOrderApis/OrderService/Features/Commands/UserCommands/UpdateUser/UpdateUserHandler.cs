﻿using MediatR;
using OrderService.Features.Commands.FoodCommands.UpdateFood;
using OrderService.Repositories;

namespace OrderService.Features.Commands.UserCommands.UpdateUser;

public class UpdateUserHandler : IRequestHandler<UpdateUserCommand>
{
    private readonly IUnitOfRepository _unitOfRepository;
    private readonly ILogger<UpdateUserHandler> _logger;

    public UpdateUserHandler(IUnitOfRepository unitOfRepository, ILogger<UpdateUserHandler> logger)
    {
        _unitOfRepository = unitOfRepository;
        _logger = logger;
    }
    public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var functionName = nameof(UpdateFoodHandler);
        try
        {
            _logger.LogInformation($"{functionName} - Start");
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
            _logger.LogInformation($"{functionName} - End");
        }
        catch (Exception ex)
        {
            _logger.LogError($"{functionName} => Has error : Message = {ex.Message}");
        }
    }
}
