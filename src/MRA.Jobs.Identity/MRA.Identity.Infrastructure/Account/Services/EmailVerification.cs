using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Mra.Shared.Common.Interfaces.Services;
using MRA.Identity.Application.Common.Interfaces.Services;
using MRA.Identity.Application.Contract.User.Responses;
using MRA.Identity.Domain.Entities;
using IEmailService = Mra.Shared.Common.Interfaces.Services.IEmailService;

namespace MRA.Identity.Infrastructure.Account.Services;
public class EmailVerification : IEmailVerification
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEmailService _emailService;
    private readonly IUserHttpContextAccessor _user;

    public EmailVerification(UserManager<ApplicationUser> userManager, IEmailService emailService, IUserHttpContextAccessor user)
    {
        _userManager = userManager;
        _emailService = emailService;
        _user = user;
    }
    public async Task<bool> SendVerificationEmailAsync(ApplicationUser user)
    {
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
    var emailBody = $"<p>Пожалуйста, перейдите по следующей ссылке для дальнейших действий: <a href='http://MRAJOBS.tj/Auth/verify?token={token}'>Ссылка</a></p>";
    var emailSubject = "Email Verification";
    return await _emailService.SendEmailAsync(new[] { user.Email }, emailBody, emailSubject);
    }
    public async Task<VerifyUserEmailResponse> VerifyEmailAsync(string token)
    {
        var userId=_user.GetUserId();
        var user = await _userManager.Users.FirstOrDefaultAsync(s => s.Id == userId);

        if (user == null)
        {
            return new VerifyUserEmailResponse() { ErrorMessage = "user can not be null", Success = false };
        }

        var result = await _userManager.ConfirmEmailAsync(user, token);

        if (result.Succeeded)
        {
            return new VerifyUserEmailResponse() { Success = true };
        }
        else
        {
            return new VerifyUserEmailResponse() { ErrorMessage =$"{result.Errors}", Success = false };
        }

    }

}
