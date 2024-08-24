using CustomerService.Data.Models.Dtos.Inputs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CustomerService.Features.Commands;

public class CreateCustomerRequest : IRequest<ObjectResult>
{
    public CreateCustomerInput Data { get; set; }
}