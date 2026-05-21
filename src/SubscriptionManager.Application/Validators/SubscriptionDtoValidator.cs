using FluentValidation;
using SubscriptionManager.Application.Models.DTOs;

namespace SubscriptionManager.Application.Validators;

public class SubscriptionDtoValidator : AbstractValidator<SubscriptionDto>
{
    public SubscriptionDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Description)
            .MaximumLength(500);

        RuleFor(x => x.StartDate)
            .NotEmpty();

        RuleFor(x => x.EndDate)
            .NotEmpty()
            .GreaterThan(x => x.StartDate)
            .WithMessage("End date must be after start date.");

        RuleFor(x => x.Price)
            .GreaterThan(0);

        RuleFor(x => x.Currency)
            .NotEmpty()
            .Length(3);

        RuleFor(x => x.Status)
            .NotEmpty()
            .MaximumLength(20);

        RuleFor(x => x.PaymentMethod)
            .MaximumLength(50);

        RuleFor(x => x.CustomerId)
            .GreaterThan(0);
    }
}
