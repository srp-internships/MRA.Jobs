using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MRA.Jobs.Application.Contracts.MyProfile.Queries;
using MRA.Jobs.Application.Contracts.Users.Commands;
using MRA.Jobs.Application.Contracts.Users.Responses;
using MRA.Jobs.Infrastructure.Shared.Users.Commands;
using MRA.Jobs.Infrastructure.Shared.Users.Commands.Roles;
using MRA.Jobs.Infrastructure.Shared.Users.Queries;

namespace MRA.Jobs.API.ControllersAuth;
[ApiController]
[Authorize]
[Route("api")]
public class AccountController : ControllerBase
{
    private readonly IMediator _mediator;

    public AccountController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [AllowAnonymous]
    [HttpPost("identity/register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand command, CancellationToken cancellationToken = default)
    {
        return Ok(await _mediator.Send(command, cancellationToken));
    }

    #region Roles
    [HttpGet("identity/{id}/roles")]
    public async Task<ActionResult<IEnumerable<string>>> UpdateRole([FromRoute] Guid id)
    {
        return Ok(await _mediator.Send(new GetUserRolesQuery() { Id = id }));
    }

    [HttpPost("identity/{id}/roles")]
    public async Task<ActionResult<IEnumerable<string>>> GrantRole([FromRoute] Guid id, [FromBody] AddUserToRolesCommand request)
    {
        request.Id = id;
        return Ok(await _mediator.Send(request));
    }

    [HttpDelete("identity/{id}/roles")]
    public async Task<ActionResult<IEnumerable<string>>> RevokeRole([FromRoute] Guid id, [FromBody] RemoveUserFromRolesCommand request)
    {
        request.Id = id;
        return Ok(await _mediator.Send(request));
    }
    #endregion

    #region me
    [HttpGet("me")]
    public async Task<ActionResult<MyProfileResponse>> MyProfile()
    {
        return Ok(await _mediator.Send(new GetMyProfileQuery()));
    }


    [HttpPut("me")]
    public async Task<ActionResult<MyProfileResponse>> UpdateMyProfile(UpdateMyProfileCommand command)
    {
        return Ok(await _mediator.Send(command));
    }

    #endregion
}
