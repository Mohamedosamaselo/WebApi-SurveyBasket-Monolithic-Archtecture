namespace SurveyBasketWebApi.Contracts.Dtos.Request;

public record CreatepollRequest
 (
 string Title,
 string Summary,
 bool IsPublished,
 DateOnly StartsAt,
 DateOnly EndsAt);