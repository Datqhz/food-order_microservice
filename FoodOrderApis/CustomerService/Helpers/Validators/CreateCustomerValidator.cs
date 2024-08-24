using CustomerService.Data.Models.Dtos.Inputs;
using FluentValidation;

namespace CustomerService.Helpers.Validators;

public class CreateCustomerValidator : AbstractValidator<CreateCustomerInput>
{
    public CreateCustomerValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name cannot be empty")
            .MaximumLength(50).WithMessage("First name cannot be more than 50 characters");
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name cannot be empty")
            .MaximumLength(50).WithMessage("Last name cannot be more than 50 characters");
    }
}