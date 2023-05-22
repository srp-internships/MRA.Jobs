using MediatR;
using Microsoft.AspNetCore.Mvc;
using MRA.Jobs.Application.Contracts.Users.Commands;
using MRA.Jobs.Infrastructure.Shared.Users.Queries;
using MRA.Jobs.Web.Controllers;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace MRA.Jobs.API.ControllersAuth;

public class VerifyController : ApiControllerBase
{
    private readonly IMediator _mediator;

    public VerifyController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("email/exist")]
    public async Task<ActionResult<bool>> GetById([FromQuery] string email)
    {
        return Ok(await _mediator.Send(new EmailExistQuery() { Email = email }));
    }

    [HttpPut("email/send")]
    public async Task<IActionResult> ChangeMyEmail(UpdateMyProfileCommand command)
    {
        return Ok(await _mediator.Send(command));
    }

    //[HttpGet("email")]
    //public async Task<IActionResult> VerifyMyEmailChange([FromQuery] string token, [FromQuery] string newEmail, [FromQuery] Guid userId)
    //{
    //    return Ok(await _mediator.Send(command));
    //}

    //[HttpPut("phone")]
    //public async Task<IActionResult> VirifyPhoneNumber(UpdateMyProfileCommand command)
    //{
    //    return Ok(await _mediator.Send(command));
    //}

    //[HttpPut("phone/sent")]
    //public async Task<IActionResult> SentPhoneVerificationCode(UpdateMyProfileCommand command)
    //{
    //    return Ok(await _mediator.Send(command));
    //}

}