using MediatR;
using Microsoft.AspNetCore.Mvc;
using MRA.Jobs.Infrastructure.Shared.Auth.Commands;
using MRA.Jobs.Infrastructure.Shared.Auth.Responses;

namespace MRA.Jobs.API.ControllersAuth;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("login")]
    public async Task<ActionResult<JwtTokenResponse>> Login([FromBody] BasicAuthenticationCommand command, CancellationToken cancellationToken = default)
    {
        return Ok(await _mediator.Send(command, cancellationToken));
    }

    [HttpPut("refresh-token")]
    public async Task<ActionResult<JwtTokenResponse>> RefreshToken([FromBody] RefreshTokenCommand command, CancellationToken cancellationToken = default)
    {
        return Ok(await _mediator.Send(command, cancellationToken));
    }

    [HttpDelete("revoke-refresh-token")]
    public async Task<ActionResult<JwtTokenResponse>> RevokeRefreshToken([FromBody] RevokeRefreshTokenCommand command, CancellationToken cancellationToken = default)
    {
        return Ok(await _mediator.Send(command, cancellationToken));
    }
}
