namespace SurveyBasketWebApi.Contracts.Dtos.Authentication;

public record loginRequest
(
    string Email,
    string Password
);