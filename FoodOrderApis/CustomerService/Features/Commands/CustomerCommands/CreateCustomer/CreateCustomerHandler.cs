using CustomerService.Data.Models;
using CustomerService.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CustomerService.Features.Commands.CustomerCommands.CreateCustomer;

public class CreateCustomerHandler : IRequestHandler<CreateCustomerRequest, ObjectResult>
{
    private readonly IUnitOfRepository _unitOfRepository;

    public CreateCustomerHandler(IUnitOfRepository unitOfRepository)
    {
        _unitOfRepository = unitOfRepository;
    }

    public async Task<ObjectResult> Handle(CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var createCustomerValidator = new CreateCustomerValidator();
            var validateResult = createCustomerValidator.Validate(request.Data);
            if (!validateResult.IsValid)
            {
                return new ObjectResult(
                    new
                    {
                        status = StatusCodes.Status400BadRequest,
                        statusText = "Invalid data"
                    }) { StatusCode = StatusCodes.Status400BadRequest };
            }
            else
            {
                var customerCreated = await _unitOfRepository.Customer.Add(new Customer
                {
                    FirstName = request.Data.FirstName,
                    LastName = request.Data.LastName,
                });
                return new ObjectResult(new
                {
                    status = StatusCodes.Status201Created,
                    statusText = "Customer created",
                    data = customerCreated
                })
                {
                    StatusCode = StatusCodes.Status201Created,
                };
            }
        }
        catch (Exception ex)
        {
            return new ObjectResult(
                new
                {
                    status = StatusCodes.Status500InternalServerError,
                    statusText = "Internal Server Error",
                }) { StatusCode = StatusCodes.Status500InternalServerError };
        }
    }
}