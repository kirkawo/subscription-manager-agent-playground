using FluentValidation;
using SubscriptionManager.Models.DTOs;

namespace SubscriptionManager.Validators;

public class CustomerDtoValidator : AbstractValidator<CustomerDto>
{
    public CustomerDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(100);

        RuleFor(x => x.Phone)
            .MaximumLength(20);

        RuleFor(x => x.Address)
            .MaximumLength(200);
    }
}
