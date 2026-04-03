using JobTrackPro.Application.Auth.Commands.Login;
using JobTrackPro.Application.Auth.Commands.Register;

using MediatR;

namespace JobTrackPro.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator) => _mediator = mediator;

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(new { error = result.Error });

        return Ok(new { token = result.Token });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return Unauthorized(new { error = result.Error });

        return Ok(new { token = result.Token });
    }
}