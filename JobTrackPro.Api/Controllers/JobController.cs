using MediatR;


using JobTrackPro.Application.Jobs.Commands.CreateJob;
using JobTrackPro.Application.Jobs.Commands.UpdateJob;


using JobTrackPro.Application.Jobs.Commands.DeleteJob;

using JobTrackPro.Application.Jobs.Queries.GetJobs;
using JobTrackPro.Application.Jobs.Queries.GetById;



namespace JobTrackPro.Api.Controllers;



[ApiController]
[Route("api/[controller]")]
public class JobsController : ControllerBase
{
    private readonly IMediator _mediator;


    public JobsController(IMediator mediator) => _mediator = mediator;

    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var jobs = await _mediator.Send(new GetJobsQuery());

        return Ok(jobs);

    }

    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var job = await _mediator.Send(new GetJobByIdQuery(id));
        return Ok(job);
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

    
    [HttpDelete("{id:guid}")]

    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteJobCommand(id));
        return NoContent();
    }
}
