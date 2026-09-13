namespace SurveyBasketWebApi.Contracts.Dtos.Polls;

public record CreatePollRequest(
    string Title,
    string Summary,
    bool IsPublished,
    DateOnly StartsAt,
    DateOnly EndsAt);