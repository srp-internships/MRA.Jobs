using MediatR;
using Microsoft.AspNetCore.Mvc;
using MRA.Jobs.Application.Common.Security;
using MRA.Jobs.Application.Contracts.MyProfile.Queries;
using MRA.Jobs.Application.Contracts.Users.Commands;
using MRA.Jobs.Application.Contracts.Users.Responses;
using MRA.Jobs.Infrastructure.Shared.Users.Commands;
using MRA.Jobs.Infrastructure.Shared.Users.Commands.Roles;
using MRA.Jobs.Infrastructure.Shared.Users.Commands.Verifications;
using MRA.Jobs.Infrastructure.Shared.Users.Queries;
using MRA.Jobs.Web.Controllers;

namespace MRA.Jobs.API.ControllersAuth;
using Microsoft.AspNetCore.Authorization
    ;
[Authorize]
[Route("api/[controller]")]
public class AccountsController : ApiControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;

    public AccountsController(IMediator mediator, ICurrentUserService currentUserService)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand command, CancellationToken cancellationToken = default)
    {
        return Ok(await _mediator.Send(command, cancellationToken));
    }

    #region Roles
    [HttpGet("{id}/roles")]
    public async Task<ActionResult<IEnumerable<string>>> UpdateRole([FromRoute] Guid id)
    {
        return Ok(await _mediator.Send(new GetUserRolesQuery() { Id = id }));
    }

    [HttpPost("{id}/roles")]
    public async Task<ActionResult<IEnumerable<string>>> GrantRole([FromRoute] Guid id, [FromBody] AddUserToRolesCommand request)
    {
        request.Id = id;
        return Ok(await _mediator.Send(request));
    }

    [HttpDelete("{id}/roles")]
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

    [HttpPut("me/identity/change-password")]
    public async Task<IActionResult> ChangeMyPassword(ChangePasswordCommand command)
    {
        return Ok(await _mediator.Send(command));
    }

    [HttpGet("me/identity/verify/phone/send")]
    public async Task<IActionResult> ChangeMyEmail([FromQuery] string phone)
    {
        return Ok(await _mediator.Send(new ChangePhoneNumberCommand()
        {
            NewPhoneNumber = phone,
            UserId = _currentUserService.GetId() ?? Guid.Empty
        }));
    }

    [HttpGet("me/identity/verify/phone")]
    public async Task<IActionResult> ChangeMyEmail([FromQuery] string phone, [FromQuery] string code)
    {
        return Ok(await _mediator.Send(new ConfirmPhoneNumberChangeCommand()
        {
            NewPhoneNumber = phone,
            Code = code,
            UserId = _currentUserService.GetId() ?? Guid.Empty
        }));
    }

    #endregion

    //[HttpGet("identity/email")]
    //public async Task<IActionResult> VerifyMyEmailChange([FromQuery] string token, [FromQuery] string newEmail, [FromQuery] Guid userId)
    //{
    //    return Ok(await _mediator.Send(command));
    //}

    //[HttpPut("me/identity/change-phone")]
    //public async Task<IActionResult> ChangeMyPhone(UpdateMyProfileCommand command)
    //{
    //    return Ok(await _mediator.Send(command));
    //}

}
