using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MRA.Identity.Application.Common.Interfaces.Services;
using MRA.Identity.Application.Contract.User.Commands;
using MRA.Identity.Application.Contract.User.Responses;
using MRA.Identity.Application.Features.Users.Command.RegisterUser;
using MRA.Identity.Domain.Entities;
using MRA.Identity.Infrastructure.Account.Services;

namespace MRA.Identity.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ISender _mediator;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEmailVerification _emailVerification;

    public AuthController(ISender mediator, UserManager<ApplicationUser> userManager, IEmailVerification emailVerification)
    {

        _mediator = mediator;
        _userManager = userManager;
        _emailVerification = emailVerification;
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
            return Unauthorized(result.Exception);
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

    [HttpGet("verify")]
    //[Authorize]
    public async Task<IActionResult> Verify([FromQuery] string token)
    {
        var result = await _emailVerification.VerifyEmailAsync(token);
        if (result.Success)
        {
            return Content("<h1>Thank you!</h1><p>Your email address has been successfully confirmed.</p>");
        }
        else
        {
            return BadRequest(result.ErrorMessage);
        }
    }


    [HttpPost("VirifyEmail")]
    //[Authorize]
    public async Task<IActionResult> ResendVirificationCode()
    {
        var result = await _mediator.Send(new UserEmallCommand());
        return Ok();
    }
}