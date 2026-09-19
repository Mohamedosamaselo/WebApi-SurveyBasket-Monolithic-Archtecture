namespace SurveyBasketWebApi.Contracts.Dtos.Authentication;

public record RefreshTokenRequest(
    string token, string refreshToken
    );