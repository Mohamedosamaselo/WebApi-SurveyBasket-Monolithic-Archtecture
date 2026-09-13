namespace SurveyBasketWebApi.Contracts.Dtos.Authentication;

public record AuthReponse(
    string Id,
    string FirstName,
    string LastName,
    string? Email,
    string Token,
    int ExpiresIn
);