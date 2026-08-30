using SurveyBasketWebApi.Contracts.Dtos.Request;
using SurveyBasketWebApi.Contracts.Response;

namespace SurveyBasketWebApi.Mapping;

public static class ContractMapping
{
    //from  poll to pollResponse
    public static PollResponse MapToPollResponse(this Poll poll)
    {
        return new PollResponse()
        {
            Id = poll.Id,
            Title = poll.Title,
            Summary = poll.Summary,
            IsPublished = poll.IsPublished,
            StartsAt = poll.StartsAt,
            EndsAt = poll.EndsAt,
        };
    }

    // from CreatePollResponse to poll
    public static Poll MapToPoll(this CreatepollRequest poll)
    {
        return new Poll()
        {
            Title = poll.Title,
            Summary = poll.Summary,
            IsPublished = poll.IsPublished,
            StartsAt = poll.StartsAt,
            EndsAt = poll.EndsAt,
        };
    }

    // from UpatePollResponse to poll
    public static Poll MapToPoll(this EditPollRequest updateRequest)
    {
        return new Poll()
        {
            Id = updateRequest.Id,
            Title = updateRequest.Title,
            Summary = updateRequest.Summary,
            IsPublished = updateRequest.IsPublished,
            StartsAt = updateRequest.StartsAt,
            EndsAt = updateRequest.EndsAt,
        };
    }
}