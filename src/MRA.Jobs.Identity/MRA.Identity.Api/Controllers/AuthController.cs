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
        var result = await _mediator.Send(request);
        if (result.IsSuccess)
        {
            return Ok(result.Response);
        }

        if (result.ErrorMessage!=null)
        {
            return BadRequest(result.ErrorMessage);
        }

        if (result.Exception!=null)
        {
            return BadRequest(result.Exception.ToString());
        }

        return BadRequest();
    }


    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand request)
    {
        var result = await _mediator.Send(request);
        if (result.IsSuccess)
        {
            return Ok(result.Response);
        }

        if (result.ErrorMessage!=null)
        {
            return BadRequest(result.ErrorMessage);
        }

        if (result.Exception!=null)
        {
            return BadRequest(result.Exception);
        }

        return BadRequest();
    }
}