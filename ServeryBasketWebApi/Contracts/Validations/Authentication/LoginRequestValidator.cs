using SurveyBasketWebApi.Contracts.Dtos.Authentication;

namespace SurveyBasketWebApi.Contracts.Validations.Authentication;

public class LoginRequestValidator : AbstractValidator<loginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email).NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password).NotEmpty()
            .MinimumLength(6);
    }
}