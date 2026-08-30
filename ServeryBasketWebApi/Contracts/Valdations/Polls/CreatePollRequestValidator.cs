using FluentValidation;
using SurveyBasketWebApi.Contracts.Dtos.Request;

namespace SurveyBasketWebApi.Contracts.Valdations.Polls;

public class CreatePollRequestValidator : AbstractValidator<CreatepollRequest>
{
    public CreatePollRequestValidator()
    {
        RuleFor(x => x.Title).NotEmpty()
                             .Length(3, 100);

        RuleFor(x => x.Summary).NotEmpty()
                               .Length(3, 1500);

        RuleFor(x => x.StartsAt).NotEmpty()
                                .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today));

        RuleFor(x => x.EndsAt).NotEmpty();

        // here i valid on all the pollRequestModel
        RuleFor(x => x).Must(hasValidDate)
                       .WithName(nameof(CreatepollRequest.EndsAt))
                       .WithMessage("{PropertyName} should be greater than or Equal to start date ");
    }

    private bool hasValidDate(CreatepollRequest pollRequestModel)
    {
        return pollRequestModel.EndsAt >= pollRequestModel.StartsAt;
    }
}