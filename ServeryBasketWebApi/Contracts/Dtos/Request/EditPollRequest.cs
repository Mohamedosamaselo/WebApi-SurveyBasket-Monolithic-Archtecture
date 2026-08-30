namespace SurveyBasketWebApi.Contracts.Dtos.Request;

public record EditPollRequest(
    int Id,
    string Title,
    string Summary,
    bool IsPublished,
    DateOnly StartsAt,
    DateOnly EndsAt);