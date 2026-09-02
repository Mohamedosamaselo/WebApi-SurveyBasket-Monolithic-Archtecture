using SurveyBasketWebApi.Contracts.Dtos.Request;
using SurveyBasketWebApi.Contracts.Response;

namespace SurveyBasketWebApi.Interfaces;

public interface IPollService
{
    Task<IEnumerable<Poll>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Poll?> GetAsync(int id, CancellationToken cancellationToken = default);

    Task<Poll> AddAsync(CreatePollRequest createPollRequest, CancellationToken cancellationToken = default);

    Task<bool> UpdateAsync(int id, EditPollRequest pollRequest, CancellationToken cancellationToken = default);

    Task<bool> TogglePublishStatusAsync(int id, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(int id, Poll pollRequest, CancellationToken cancellationToken = default);
}