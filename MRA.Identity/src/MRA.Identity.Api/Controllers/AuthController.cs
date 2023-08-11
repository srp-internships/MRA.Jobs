using MediatR;
using Microsoft.AspNetCore.Mvc;
using MRA.Identity.Application.Contract.User.Commands;
using MRA.Identity.Application.Contract.User.Responses;

namespace MRA.Identity.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ISender _mediator;

    public AuthController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserCommand request)
    {
        JwtTokenResponse? result = await _mediator.Send(request);
        return result == null ? Unauthorized() : Ok(result);
    }


    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand request)
    {
        Guid? result = await _mediator.Send(request);
        return result == null ? BadRequest() : Ok();
    }
}