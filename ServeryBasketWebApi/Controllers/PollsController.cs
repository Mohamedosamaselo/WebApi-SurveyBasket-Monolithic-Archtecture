using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SurveyBasketWebApi.Contracts.Dtos.Polls;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace SurveyBasketWebApi.Controllers;

[Route("[controller]")]
[ApiController]
public class PollsController(IPollService pollService) : ControllerBase
{
    private readonly IPollService _pollService = pollService;

    // GetAll Endpoint
    [HttpGet("GetAll")]
    [Authorize]
    public async Task<IActionResult> GetAllAsync()
    {
        var polls = await _pollService.GetAllAsync();

        return polls is not null ? Ok(polls) : NotFound();
    }

    // GetById Endpoint

    [HttpGet("Get/{id:int}")]
    public async Task<IActionResult> GetAsync([FromRoute] int id)
    {
        var poll = await _pollService.GetAsync(id);

        return poll is not null ? Ok(poll.MapToPollResponse()) : NotFound();
    }

    // Add Endpoint
    [HttpPost("Add")]
    public async Task<IActionResult> AddAsync([FromBody] CreatePollRequest createPollRequest, CancellationToken cancellationToken = default)
    {
        var newPoll = await _pollService.AddAsync(createPollRequest, cancellationToken);
        return CreatedAtAction(nameof(GetAsync), new { id = newPoll.Id }, newPoll);
    }

    [HttpPut("Update/{id:int}")]
    public async Task<IActionResult> EditAsync([FromRoute] int id, [FromBody] EditPollRequest pollRequest, CancellationToken cancellationToken = default)
    {
        var UpdatedPoll = await _pollService.UpdateAsync(id, pollRequest, cancellationToken);
        if (!UpdatedPoll)
            return NotFound();

        return NoContent();
    }

    [HttpPut("ToggleIsPublishedStatus/{id:int}")]
    public async Task<IActionResult> ToggleIsPublishedStatusAsync([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        var isPublishedToggled = await _pollService.TogglePublishStatusAsync(id, cancellationToken);

        if (!isPublishedToggled)
            return NotFound();

        return NoContent();
    }

    //  [HttpDelete("Delete/{id:int}")]
    //public async Task<IActionResult> Delete([FromRoute] int id, [FromBody] Poll pollRequest, CancellationToken cancellationToken = default)
    //{
    //    var deletededPoll = await _pollService.DeleteAsync(id, pollRequest, cancellationToken);

    //    if (!deletededPoll)

    //        return NotFound();

    //    return NoContent();
    //}
};