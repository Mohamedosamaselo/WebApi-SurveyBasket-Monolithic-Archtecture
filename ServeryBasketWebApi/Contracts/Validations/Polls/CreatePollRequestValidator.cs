using FluentValidation;
using SurveyBasketWebApi.Contracts.Dtos.Request;

namespace SurveyBasketWebApi.Contracts.Validations.Polls;

public class CreatePollRequestValidator : AbstractValidator<CreatePollRequest>
{
    public CreatePollRequestValidator()
    {
        RuleFor(x => x.Title).NotEmpty()
                             .Length(3, 100);

        RuleFor(x => x.Summary).NotEmpty()
                               .Length(3, 1500);

        RuleFor(x => x.StartsAt)
            .NotEmpty()
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today));

        RuleFor(x => x.EndsAt)
            .NotEmpty()
            .GreaterThanOrEqualTo(x => x.StartsAt)
            .WithMessage("EndsAt must be greater than or equal to StartsAt.");
    }
}
