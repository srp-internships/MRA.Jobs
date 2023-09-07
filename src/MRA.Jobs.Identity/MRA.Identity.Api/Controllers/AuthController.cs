using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MRA.Identity.Application.Contract.User.Commands;
using MRA.Identity.Application.Contract.User.Queries;

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

        if (result.ErrorMessage != null)
        {
            return Unauthorized(result.ErrorMessage);
        }

        if (result.Exception != null)
        {
            return Unauthorized(result.Exception.ToString());
        }

        return Unauthorized();
    }


    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand request)
    {
        var result = await _mediator.Send(request);
        if (result.IsSuccess)
        {
            return Ok(result.Response);
        }

        if (result.ErrorMessage != null)
        {
            return Unauthorized(result.ErrorMessage);
        }

        if (result.Exception != null)
        {
            return Unauthorized(result.Exception);
        }

        return Unauthorized();
    }


    [HttpPost("refresh")]   
    public async Task<IActionResult> Refresh(GetAccessTokenUsingRefreshTokenQuery request)
    {
        var response = await _mediator.Send(request);
        if (response.IsSuccess == false)
        {
            return BadRequest(response.ErrorMessage);
        }

        return Ok(response);
    }
}