namespace SurveyBasketWebApi.Contracts.Dtos.Polls;

public record EditPollRequest(
    int Id,
    string Title,
    string Summary,
    bool IsPublished,
    DateOnly StartsAt,
    DateOnly EndsAt);