
using JobTrackPro.Application.Jobs.Commands.CreateJob;

using JobTrackPro.Application.Jobs.Commands.UpdateJob;
using JobTrackPro.Application.Jobs.Queries.GetById;
using JobTrackPro.Application.Jobs.Queries.GetJobs;
using JobTrackPro.Application.Jobs.Queries.GetJobStats;
using JobTrackPro.Domain.Enums;
using JobTrackPro.Application.Jobs.Commands.DeleteJob;
using JobTrackPro.Application.Jobs.DTOs;

using JobTrackPro.Application.Jobs.Queries.GetById;
using JobTrackPro.Application.Common;
using MediatR;


namespace JobTrackPro.Api.Controllers;



[ApiController]
[Route("api/[controller]")]
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
        var jobs = await _mediator.Send(new GetJobsQuery(search, status, page, pageSize));
        return Ok(jobs);
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

        return Ok(id);
    }

    
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateJobCommand command)
    {
        if (id != command.Id) return BadRequest("Id mismatch");


        await _mediator.Send(command);
        return NoContent();
    }

    
    

    [HttpGet("stats")]
    public async Task<IActionResult> GetStats()
    {
        var result = await _mediator.Send(new GetJobStatsQuery());
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteJobCommand(id));
        return NoContent(); 
    }


    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var result = await _mediator.Send(new GetJobByIdQuery(id));

        return result.IsSuccess
            ? Ok(result.Value)
            : NotFound(new { error = result.Error });
    }
}
