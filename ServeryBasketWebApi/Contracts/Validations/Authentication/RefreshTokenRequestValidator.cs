using SurveyBasketWebApi.Contracts.Dtos.Authentication;
using SurveyBasketWebApi.Entities;

namespace SurveyBasketWebApi.Contracts.Validations.Authentication;

public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
{
    public RefreshTokenRequestValidator()
    {
        RuleFor(x => x.token).NotEmpty();
        RuleFor(x => x.refreshToken).NotEmpty();
    }
}