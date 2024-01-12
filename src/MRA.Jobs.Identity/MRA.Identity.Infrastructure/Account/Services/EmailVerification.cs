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
<center>
 <table class='body-wrap' style='text-align:center;width:86%;font-family:arial,sans-serif;border-spacing:4px 20px;'>
            <tr>
                <img src='https://jobs.srp.tj/images/srp/srp_icon.png' style='width:25%;'>
                <td>
                    <center>
                        <table bgcolor='#FFFFFF' width='80%' border='0'>
                            <tbody>                             
                                <tr style='font-size:14px;'>
                                    <td><center>
                                    <span>Пожалуйста, перейдите по следующей ссылке для  действий: <a href='{_configurations["MraIdentityClient-HostName"]}/verifyEmail?token={WebUtility.UrlEncode(token)}&userId={user.Id}'>Ссылка</a></span>
                                   </center> </td>
                                </tr>
                            </tbody>
                        </table>
                    </center>
                </td>
            </tr>
        </table>
    </center>
<center>
  <table
    class=""body-wrap""
    style=""
      text-align: center;
      width: 86%;
      font-family: arial, sans-serif;
      border-spacing: 4px 20px;
    ""
  >
    <tr>
      <img
        src=""https://jobs.srp.tj/images/srp/srp_icon.png""
        style=""width: 25%""
      />
      <td>
        <center>
          <table bgcolor=""#FFFFFF"" width=""80%"" border=""0"">
            <tbody>
              <tr style=""font-size: 14px"">
                <td>
                    Здравствуйте!
                </td>
                <td>
                    Благодарим вас за регистрацию на нашем сайте. Мы рады, что вы присоединились к нашему сообществу.
                </td>
                <td>
                    Для завершения регистрации и активации вашей учетной записи вам необходимо подтвердить ваш адрес электронной почты. Для этого просто перейдите по ссылке ниже:
                </td>
                <td>
<a href='{_configurations["MraIdentityClient-HostName"]}/verifyEmail?token={WebUtility.UrlEncode(token)}&userId={user.Id}'>Подтвердить электронную почту</a>                   
                </td>
                <td>
                    Если вы не регистрировались на нашем сайте, пожалуйста, проигнорируйте это письмо.
                </td>
              </tr>
            </tbody>
          </table>
        </center>
      </td>
    </tr>
  </table>
</center>
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