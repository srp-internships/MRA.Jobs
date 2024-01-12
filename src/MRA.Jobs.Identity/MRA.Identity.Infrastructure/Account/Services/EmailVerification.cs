using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MRA.Identity.Application.Common.Exceptions;
using MRA.Identity.Application.Common.Interfaces.Services;
using MRA.Identity.Domain.Entities;
using Newtonsoft.Json.Linq;
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
        var emailBody =
            $@"    

<body style='background-color: #e9ecef;'>
  <table border='0' cellpadding='0' cellspacing='0' width='100%'>
    <tr>
      <td align='center' bgcolor='#e9ecef'>
        <table border='0' cellpadding='0' cellspacing='0' width='100%' style='max-width: 600px;'>
            <tr>
                <td align='center' valign='top' style='padding: 36px 24px;'>
                
                    <img   src='https://jobs.srp.tj/images/srp/srp_icon.png'
                    style='width: 30%' alt='Logo' border='0' style='display: block; width: 48px; max-width: 48px; min-width: 48px;'>
               
                </td>
              </tr>
          <tr>
            <td align='left' bgcolor='#ffffff' style='padding: 36px 24px 0; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; border-top: 3px solid #d4dadf;'>
              <h1 style='margin: 0; font-size: 32px; font-weight: 700; letter-spacing: -1px; line-height: 48px;'>Подтвердите свой адрес электронной почты</h1>
            </td>
          </tr>
        </table>
      </td>
    </tr>

    <tr>
      <td align='center' bgcolor='#e9ecef'>
        <table border='0' cellpadding='0' cellspacing='0' width='100%' style='max-width: 600px;'>
          <tr>
            <td align='left' bgcolor='#ffffff' style='padding: 24px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 16px; line-height: 24px;'>
              <p style='margin: 0;'>Нажмите кнопку ниже, чтобы подтвердить свой адрес электронной почты.</p>
            </td>
          </tr>
          <tr>
            <td align='left' bgcolor='#ffffff'>
              <table border='0' cellpadding='0' cellspacing='0' width='100%'>
                <tr>
                  <td bgcolor='#ffffff' style='padding: 12px;'>
                    <table border='0' cellpadding='0' cellspacing='0'>
                      <tr>
                        <td bgcolor='#1a82e2' style='border-radius: 6px;'>
                          <a href='{_configurations["MraIdentityClient-HostName"]}/verifyEmail?token={WebUtility.UrlEncode(token)}&userId={user.Id}' style='display: inline-block; padding: 16px 36px; font-size: 16px; color: #ffffff; text-decoration: none; border-radius: 6px;'>Подтвердить</a>
                        </td>
                      </tr>
                    </table>
                  </td>
                </tr>
              </table>
            </td>
          </tr>
       
          <tr>
            <td align='left' bgcolor='#ffffff' style='padding: 24px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 16px; line-height: 24px;'>
              <p style='margin: 0;'>Если вы не создавали учетную запись с помощью, вы можете безопасно удалить это письмо.</p>
            </td>
          </tr>
       
          <tr>
            <td align='left' bgcolor='#ffffff' style='padding: 24px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 16px; line-height: 24px; border-bottom: 3px solid #d4dadf'>
              <p style='margin: 0;'></p>
            </td>
          </tr>
        
        </table>
    
      </td>
    </tr> 
    </table>
</body>

";
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