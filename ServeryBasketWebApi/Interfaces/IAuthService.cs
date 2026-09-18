using SurveyBasketWebApi.Contracts.Dtos.Authentication;

namespace SurveyBasketWebApi.Interfaces;

public interface IAuthService
{
    Task<AuthReponse?> GetTokenAsync(string email,
                                  string password,
                                  CancellationToken cancellationToken = default);
}