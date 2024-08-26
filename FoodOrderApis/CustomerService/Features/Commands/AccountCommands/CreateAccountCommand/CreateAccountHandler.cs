using CustomerService.Consumers;
using CustomerService.Data.Models;
using CustomerService.Features.Commands.AccountCommands.CreateAccountCommand;
using CustomerService.Repositories;
using MediatR;

namespace CustomerService.Features.Commands.AccountCommands.CreateAccountCommand;

public class CreateAccountHandler :IRequestHandler<CreateAccountRequest>
{
    private readonly IUnitOfRepository _repository;

    public CreateAccountHandler(IUnitOfRepository repository)
    {
        _repository = repository;
    }
    public async Task Handle(CreateAccountRequest request, CancellationToken cancellationToken)
    {
        try
        {
            await _repository.Account.Add(new Account
            {
                Id = request.Data.AccountId,
                Username = request.Data.Username
            });

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
