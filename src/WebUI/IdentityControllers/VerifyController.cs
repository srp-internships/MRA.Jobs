using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MRA.Jobs.Infrastructure.Shared.Users.Commands;

namespace MRA.Jobs.API.ControllersAuth;

public class VerifyController : ControllerBase
{
    private readonly IMediator _mediator;

    public VerifyController(IMediator mediator)
    {
        _mediator = mediator;
    }

    //[HttpGet("email-exist")]
    //public async Task<ActionResult<bool>> GetById([FromQuery] string email)
    //{
    //    return Ok(await _mediator.Send(new EmailExistQuery() { Email = email }));
    //}

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand command, CancellationToken cancellationToken = default)
    {
        var user = await _mediator.Send(command, cancellationToken);
        return Ok();
    }

    //[HttpGet("me")]
    //public async Task<ActionResult<MyProfileResponce>> MyProfile()
    //{
    //    return Ok(await _mediator.Send(new GetMyProfileQuery()));
    //}

    //[HttpPut("me")]
    //public async Task<ActionResult<MyProfileResponce>> UpdateMyProfile(UpdateMyProfileCommand command)
    //{
    //    return Ok(await _mediator.Send(command));
    //}

    //[HttpGet("{id}/roles")]
    //public async Task<ActionResult<IEnumerable<string>>> UpdateRole([FromRoute] Guid id)
    //{
    //    return Ok(await _mediator.Send(new GetUserRolesQuery() { Id = id }));
    //}

    //[HttpPut("{id}/roles/reset")]
    //public async Task<ActionResult<IEnumerable<string>>> ResetRole([FromRoute] Guid id, [FromBody] ResetUserRolesCommand request)
    //{
    //    request.Id = id;
    //    return Ok(await _mediator.Send(request));
    //}

    //[HttpPut("{id}/roles/grant")]
    //public async Task<ActionResult<IEnumerable<string>>> GrantRole([FromRoute] Guid id, [FromBody] AddUserToRolesCommand request)
    //{
    //    request.Id = id;
    //    return Ok(await _mediator.Send(request));
    //}

    //[HttpPut("{id}/roles/revoke")]
    //public async Task<ActionResult<IEnumerable<string>>> RevokeRole([FromRoute] Guid id, [FromBody] RemoveUserFromRolesCommand request)
    //{
    //    request.Id = id;
    //    return Ok(await _mediator.Send(request));
    //}

}