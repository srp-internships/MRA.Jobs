using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MRA.Configurations.Common.Interfaces.Services;
using MRA.Identity.Application.Common.Exceptions;
using MRA.Identity.Application.Common.Interfaces.Services;
using MRA.Identity.Application.Contract.User.Responses;
using MRA.Identity.Domain.Entities;
using IEmailService = MRA.Configurations.Common.Interfaces.Services.IEmailService;

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
    public async Task VerifyEmailAsync(string token)
    {
        var userId=_user.GetUserId();
        var user = await _userManager.Users.FirstOrDefaultAsync(s => s.Id == userId);

        if (user == null)
        {
            throw new NotFoundException("user not found");
        }

        var result = await _userManager.ConfirmEmailAsync(user, token);

        if (!result.Succeeded)
        {
            throw new ValidationException("token is not valid");
        }
        

    }

}
