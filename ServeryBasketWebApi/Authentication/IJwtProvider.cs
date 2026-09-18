using SurveyBasketWebApi.Entities;

namespace SurveyBasketWebApi.Authentication;

public interface IJwtProvider
{
    Task<(string Token, int ExpiresIn)> GenerateTokenAsync(ApplicationUser user);

    string? ValidateToken(string token);
}