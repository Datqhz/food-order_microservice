using CustomerService.Data.Models;
using CustomerService.Features.Commands;
using CustomerService.Helpers.Validators;
using CustomerService.Repositories.Implements;
using CustomerService.Repositories.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CustomerService.Handler;

public class CreateCustomerHandler : IRequestHandler<CreateCustomerRequest, ObjectResult>
{
    private readonly ICustomerRepository _customerRepository;

    public CreateCustomerHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
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
                var customerCreated = await _customerRepository.Add(new Customer
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