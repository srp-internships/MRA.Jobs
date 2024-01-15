using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MRA.Identity.Application.Common.Exceptions;
using MRA.Identity.Application.Common.Interfaces.Services;
using MRA.Identity.Domain.Entities;
using IEmailService = MRA.Configurations.Common.Interfaces.Services.IEmailService;

namespace MRA.Identity.Infrastructure.Account.Services;

public class EmailVerification : IEmailVerification
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEmailService _emailService;
    private readonly IConfiguration _configurations;


    public EmailVerification(UserManager<ApplicationUser> userManager, IEmailService emailService,
        IConfiguration configurations)
    {
        _userManager = userManager;
        _emailService = emailService;
        _configurations = configurations;
    }

    public async Task<bool> SendVerificationEmailAsync(ApplicationUser user)
    {
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var emailTemplate = await File.ReadAllTextAsync(Path.Combine("Resources", "verificationmessage.html"));
        var url = $"{_configurations["MraIdentityClient-HostName"]}/verifyEmail?token={WebUtility.UrlEncode(token)}&userId={user.Id}";
        var emailBody = emailTemplate.Replace("@FullName", $"{user.FirstName} {user.LastName}").Replace("@LinkToGo", url);
        var emailSubject = "Email Verification";
        return await _emailService.SendEmailAsync(new[] { user.Email }, emailBody, emailSubject);
    }

    public async Task VerifyEmailAsync(string token, Guid userId)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(s => s.Id == userId) ??
                   throw new NotFoundException("user not found");

        var result = await _userManager.ConfirmEmailAsync(user, token);

        if (!result.Succeeded)
        {
            throw new ValidationException("token is not valid");
        }
    }
}