using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MRA.Identity.Application.Common.Interfaces.Services;
using MRA.Identity.Application.Contract.User.Commands;

using MRA.Identity.Application.Contract.User.Commands.ChangePassword;

using MRA.Identity.Application.Contract.User.Commands.GoogleAuth;

using MRA.Identity.Application.Contract.User.Commands.LoginUser;
using MRA.Identity.Application.Contract.User.Commands.RegisterUser;
using MRA.Identity.Application.Contract.User.Queries;

namespace MRA.Identity.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ISender _mediator;
    private readonly IEmailVerification _emailVerification;

    public AuthController(ISender mediator, IEmailVerification emailVerification)
    {
        _mediator = mediator;
        _emailVerification = emailVerification;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserCommand request)
    {
        var result = await _mediator.Send(request);
        return Ok(result);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand request)
    {
        var result = await _mediator.Send(request);
        return Ok(result);
    }

    [HttpGet("verify")]
    [Authorize]
    public async Task<IActionResult> Verify(string token)
    {
        await _emailVerification.VerifyEmailAsync(token);

        return Content("<h1>Thank you!</h1><p>Your email address has been successfully confirmed.</p>");
    }


    [HttpPost("VerifyEmail")]
    [Authorize]
    public async Task<IActionResult> ResendVerificationCode()
    {
        await _mediator.Send(new UserEmailCommand());
        return Ok();
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(GetAccessTokenUsingRefreshTokenQuery request)
    {
        var response = await _mediator.Send(request);
        return Ok(response);
    }


    [HttpPut("ChangePassword")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordUserCommand command)
    {
        var response = await _mediator.Send(command);
        return Ok(response);
}
    
    [HttpPost("googleCode")]
    public async Task<IActionResult> GoogleLogin([FromBody] GoogleAuthCommand googleAuth)
    {
        var result = await _mediator.Send(googleAuth);
        return Ok(result);
    }
}