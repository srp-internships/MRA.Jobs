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


    public EmailVerification(UserManager<ApplicationUser> userManager, IEmailService emailService, IConfiguration configurations)
    {
        _userManager = userManager;
        _emailService = emailService;
        _configurations = configurations;
    }

    public async Task<bool> EmailApproved(ApplicationUser user)
    {
        string htmlContent = $@"
    <center>
        <table class='body-wrap' style='text-align:center;width:86%;font-family:arial,sans-serif;border-spacing:4px 20px;'>
            <tr>
                <img src='https://jobs.srp.tj/images/srp/srp_icon.png' style='width:25%;'>
                <td>
                    <center>
                        <table bgcolor='#FFFFFF' width='80%' border='0'>
                            <tbody>
                                <tr style='text-align:center;color:#575252;font-size:14px;'>
                                    <td>
                                        <span><h3>Уважаемый/ая {user.FirstName},<h3></span>
                                    </td>
                                </tr>
                                <tr style='color:#a2a2a2;font-size:14px;'>
                                    <td>
                                        <span>Мы рады сообщить вам, что ваше резюме было одобрено, и мы хотели бы пригласить вас на собеседование для дальнейшего обсуждения вашего опыта и возможного вступления в нашу команду.</span>
                                    </td>
                                </tr>
                                <tr style='color:#a2a2a2;font-size:14px;height:45px;'>
                                    <td>
                                        <span>Мы будем связаться с вами в ближайшие дни для уточнения дополнительных деталей и подтверждения вашего участия.</span>
                                    </td>
                                </tr>
                                <tr style='color:#a2a2a2;font-size:14px;height:45px;'>
                                    <td>
                                        <span>С наилучшими пожеланиями, SRP Team!</span>
                                    </td>
                                </tr>
                                <tr style='text-align:center'>
                                    <td>
                                      <a href='https://www.jobs.srp.tj/applications' style='display:inline-block;background-color:#1c92c8;color:#fff;padding:10px 20px;text-decoration:none;border-radius:5px;'>Перейти к заявке</a>
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
        var subject = "Заявка одобрено!";
        return await _emailService.SendEmailAsync(new[] { user.Email }, htmlContent, subject);
    }

    public async Task<bool> EmailRejected(ApplicationUser user)
    {
        string htmlContent = $@"
    <center>
        <table class='body-wrap' style='text-align:center;width:86%;font-family:arial,sans-serif;border-spacing:4px 20px;'>
            <tr>
                <img src='https://jobs.srp.tj/images/srp/srp_icon.png' style='width:25%;'>
                <td>
                    <center>
                        <table bgcolor='#FFFFFF' width='80%' border='0'>
                            <tbody>
                                <tr style='text-align:center;color:#575252;font-size:14px;'>
                                    <td>
                                        <span><h3>Уважаемый/ая {user.FirstName},<h3></span>
                                    </td>
                                </tr>
                                <tr style='color:#a2a2a2;font-size:14px;'>
                                    <td>
                                        <span>Благодарим вас за проявленный интерес к вакансии в нашей компании. К сожалению, после тщательного рассмотрения вашего резюме мы приняли решение не продолжать процесс с вами.</span>
                                    </td>
                                </tr>
                                <tr style='color:#a2a2a2;font-size:14px;height:45px;'>
                                    <td>
                                        <span>Мы ценим ваше время и усилия, вложенные в подачу заявки, и надеемся, что вы найдете подходящую возможность в другом месте.</span>
                                    </td>
                                </tr>
                                <tr style='color:#a2a2a2;font-size:14px;height:45px;'>
                                    <td>
                                        <span>С наилучшими пожеланиями, SRP Team!</span>
                                    </td>
                                </tr>
                                <tr style='text-align:center'>
                                    <td>
                                      <a href='https://www.jobs.srp.tj/applications' style='display:inline-block;background-color:#1c92c8;color:#fff;padding:10px 20px;text-decoration:none;border-radius:5px;'>Перейти к заявке</a>
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

        var subject = "Уведомление об отказе";
        return await _emailService.SendEmailAsync(new[] { user.Email }, htmlContent, subject);
    }

    public async Task<bool> SendVerificationEmailAsync(ApplicationUser user)
    {
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var emailBody =
            $"<p>Пожалуйста, перейдите по следующей ссылке для дальнейших действий: <a href='{_configurations["MraIdentityClient-HostName"]}/verifyEmail?token={WebUtility.UrlEncode(token)}&userId={user.Id}'>Ссылка</a></p>";
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