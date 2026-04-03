
using JobTrackPro.Application.Common;
using JobTrackPro.Application.Jobs.Commands.CreateJob;
using JobTrackPro.Application.Jobs.Commands.DeleteJob;
using JobTrackPro.Application.Jobs.Commands.UpdateJob;

using JobTrackPro.Application.Jobs.Queries.GetById;
using JobTrackPro.Application.Jobs.Queries.GetJobs;

using JobTrackPro.Application.Jobs.Queries.GetJobStats;
using JobTrackPro.Domain.Enums;

using MediatR;

using Microsoft.AspNetCore.Authorization;

namespace JobTrackPro.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class JobsController : ControllerBase
{
    private readonly IMediator _mediator;

    public JobsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? search,
        [FromQuery] ApplicationStatus? status,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await _mediator.Send(new GetJobsQuery(search, status, page, pageSize));
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetJobByIdQuery(id));
        return result.IsSuccess
            ? Ok(result.Value)
            : NotFound(new { error = result.Error });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateJobCommand command)
    {
        var id = await _mediator.Send(command);
        
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateJobCommand command)
    {
        if (id != command.Id)
            return BadRequest(new { error = "Route id and body id must match." });

        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteJobCommand(id));
        return NoContent();
    }

    [HttpGet("stats")]
    public async Task<IActionResult> GetStats()
    {
        var result = await _mediator.Send(new GetJobStatsQuery());
        return Ok(result);
    }
}