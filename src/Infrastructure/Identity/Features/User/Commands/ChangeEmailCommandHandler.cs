using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using MRA.Jobs.Infrastructure.Shared.Users.Commands;

namespace MRA.Jobs.Infrastructure.Identity.Features.User.Commands;

public class ChangeEmailCommandHandler : IRequestHandler<ChangeEmailCommand, Unit>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ChangeEmailCommandHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Unit> Handle(ChangeEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null)
            throw new NotFoundException(nameof(ApplicationUser), request.UserId);

        if (user.Email.Equals(request.NewEmail))
            return await Task.FromResult(Unit.Value);

        if (_userManager.Users.Any(u => u.NormalizedEmail == request.NewEmail && u.Id != u.Id))
            throw new ValidationException(new[] { new ValidationFailure() { PropertyName = nameof(request.NewEmail), ErrorMessage = $"Account with {request.NewEmail} email already exist!" } });

        var token = await _userManager.GenerateChangeEmailTokenAsync(user, request.NewEmail);

        //TODO: Send email to confirm email
        //var confirmationLink = Path.Combine(.Action(nameof(ConfirmEmail), "Account", new { userId = user.Id, token, newEmail }, Request.Scheme);
        //await _emailSender.SendEmailAsync(newEmail, "Confirm your email", $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(confirmationLink)}'>clicking here</a>.");

        return Unit.Value;
    }
}

public class AccountController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IEmailSender _emailSender; // Implementation of this service depends on your chosen method for sending emails

    public AccountController(UserManager<IdentityUser> userManager, IEmailSender emailSender)
    {
        _userManager = userManager;
        _emailSender = emailSender;
    }

    // We assume that you are using an external service for sending sms. Replace ISmsSender with the appropriate service
    [HttpPost]
    public async Task<IActionResult> ChangePhoneNumber(string newPhoneNumber, [FromServices] ISmsSender smsSender)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        var token = await _userManager.GenerateChangePhoneNumberTokenAsync(user, newPhoneNumber);

        await smsSender.SendSmsAsync(newPhoneNumber, $"Your verification code is: {token}");

        return Ok("Verification code sent. Please check your phone.");
    }

    [HttpPost]
    public async Task<IActionResult> ConfirmPhoneNumber(string verificationCode, string newPhoneNumber)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        var result = await _userManager.ChangePhoneNumberAsync(user, newPhoneNumber, verificationCode);
        if (!result.Succeeded) return BadRequest("Phone number can't be confirmed");

        return Ok("Phone number confirmed successfully");
    }
}
