using Microsoft.EntityFrameworkCore;
using SurveyBasketWebApi.Contracts.Dtos.Request;
using SurveyBasketWebApi.Contracts.Response;
using SurveyBasketWebApi.Interfaces;
using SurveyBasketWebApi.Mapping;
using SurveyBasketWebApi.Persistence;

namespace SurveyBasketWebApi.Services;

public class PollService(ApplicationDbContext context) : IPollService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<IEnumerable<Poll>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.Polls.AsNoTracking().ToListAsync();

    public async Task<Poll?> GetAsync(int id, CancellationToken cancellationToken = default)
        => await _context.Polls.FindAsync(id);

    public async Task<Poll> AddAsync(CreatepollRequest pollRequest, CancellationToken cancellationToken = default)
    {
        await _context.AddAsync(pollRequest);
        await _context.SaveChangesAsync();

        return pollRequest.MapToPoll();
    }

    public async Task<bool> UpdateAsync(int id, EditPollRequest poll, CancellationToken cancellationToken = default)
    {
        var currentPoll = await GetAsync(id, cancellationToken);

        if (currentPoll == null)
            return false;

        currentPoll.Title = poll.Title;
        currentPoll.Summary = poll.Summary;
        currentPoll.StartsAt = poll.StartsAt;
        currentPoll.EndsAt = poll.EndsAt;

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> TogglePublishStatusAsync(int id, CancellationToken cancellationToken = default)
    {
        var currentPoll = await GetAsync(id, cancellationToken);

        if (currentPoll == null)
            return false;

        currentPoll.IsPublished = !currentPoll.IsPublished;

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, Poll pollRequest, CancellationToken cancellationToken = default)
    {
        var currentPoll = await GetAsync(id, cancellationToken);

        if (currentPoll == null)
            return false;

        _context.Remove(currentPoll);

        await _context.SaveChangesAsync();

        return true;
    }
}