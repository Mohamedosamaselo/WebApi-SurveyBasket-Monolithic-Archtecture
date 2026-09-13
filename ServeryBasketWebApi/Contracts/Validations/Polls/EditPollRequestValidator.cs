using FluentValidation;
using SurveyBasketWebApi.Contracts.Dtos.Polls;

namespace SurveyBasketWebApi.Contracts.Validations.Polls;

public class EditPollRequestValidator : AbstractValidator<EditPollRequest>
{
    public EditPollRequestValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id must be greater than 0.");

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