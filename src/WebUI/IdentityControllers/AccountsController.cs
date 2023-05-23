using MediatR;
using Microsoft.AspNetCore.Mvc;
using MRA.Jobs.Application.Common.Security;
using MRA.Jobs.Application.Contracts.MyProfile.Queries;
using MRA.Jobs.Infrastructure.Shared.Users.Commands;
using MRA.Jobs.Infrastructure.Shared.Users.Commands.Roles;
using MRA.Jobs.Infrastructure.Shared.Users.Commands.Verifications;
using MRA.Jobs.Infrastructure.Shared.Users.Queries;
using MRA.Jobs.Web.Controllers;

namespace MRA.Jobs.Web.IdentityControllers;

using Microsoft.AspNetCore.Authorization
    ;
using MRA.Jobs.Application.Contracts.MyProfile.Commands;
using MRA.Jobs.Application.Contracts.MyProfile.Responses;

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
    public async Task<IActionResult> SendVerificationCodeForMyPhone([FromQuery] string phone)
    {
        return Ok(await _mediator.Send(new ChangePhoneNumberCommand()
        {
            NewPhoneNumber = phone,
            UserId = _currentUserService.GetId() ?? Guid.Empty
        }));
    }

    [HttpGet("me/identity/verify/phone")]
    public async Task<IActionResult> VerifyMyPhone([FromQuery] string phone, [FromQuery] string code)
    {
        return Ok(await _mediator.Send(new ConfirmPhoneNumberChangeCommand()
        {
            NewPhoneNumber = phone,
            Code = code,
            UserId = _currentUserService.GetId() ?? Guid.Empty
        }));
    }

    [HttpGet("me/identity/verify/email/sent")]
    public async Task<IActionResult> SendVerificationTokenForMyEmail([FromQuery] string newEmail)
    {
        return Ok(await _mediator.Send(new ChangeEmailCommand()
        {
            UserId = _currentUserService.GetId() ?? Guid.Empty,
            NewEmail = newEmail,
        }));
    }

    [HttpGet("me/identity/verify/email")]
    public async Task<IActionResult> VerifyMyEmail([FromQuery] string token, [FromQuery] string newEmail, [FromQuery] Guid userId)
    {
        return Ok(await _mediator.Send(new ConfirmEmailChangeCommand()
        {
            UserId = _currentUserService.GetId() ?? Guid.Empty,
            NewEmail = newEmail,
            Token = token
        }));
    }

    #endregion

    [HttpGet("identity/verify/phone/send")]
    public async Task<IActionResult> SendPhoneVerificationCode([FromQuery] string phone)
    {

        return Ok(await _mediator.Send(new ChangePhoneNumberCommand()
        {
            NewPhoneNumber = phone,
            UserId = _currentUserService.GetId() ?? Guid.Empty
        }));
    }

    [HttpGet("identity/verify/phone")]
    public async Task<IActionResult> VerifyPhone([FromQuery] string phone, [FromQuery] string code)
    {
        return Ok(await _mediator.Send(new ConfirmPhoneNumberChangeCommand()
        {
            NewPhoneNumber = phone,
            Code = code,
            UserId = _currentUserService.GetId() ?? Guid.Empty
        }));
    }

    [HttpGet("identity/email/exist")]
    public async Task<ActionResult<bool>> GetById([FromQuery] string email)
    {
        return Ok(await _mediator.Send(new EmailExistQuery() { Email = email }));
    }

    [HttpGet("identity/verify/email/sent")]
    public async Task<IActionResult> SendEmailVerificationToken([FromQuery] string newEmail)
    {
        return Ok(await _mediator.Send(new ChangeEmailCommand()
        {
            UserId = _currentUserService.GetId() ?? Guid.Empty,
            NewEmail = newEmail,
        }));
    }

    [HttpPut("identity/verify/email")]
    public async Task<IActionResult> VerifyEmail(UpdateMyProfileCommand command)
    {
        return Ok(await _mediator.Send(command));
    }
}
