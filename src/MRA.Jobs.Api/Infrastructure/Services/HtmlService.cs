using MRA.Configurations.Common.Interfaces.Services;
using MRA.Identity.Application.Contract.Profile.Responses;
using MRA.Jobs.Application.Contracts.Applications.Commands.CreateApplication;

namespace MRA.Jobs.Infrastructure.Services;

public class HtmlService(IEmailService emailService) : IHtmlService
{
    public string GenerateApplyVacancyContent(string userName)
    {
        var content = $@"
<div style=""display: flex;flex-direction: column;width: 100%"">
            <h2><strong>New apply</strong></h2>
            <h4>{userName}</h4>
            <button type=""button"" onclick=""location.href='#your_url'"">view</button>
            </div>
";
        return content;
    }

    public string GenerateApplyVacancyContent_CreateApplication(string hostName, string applicationSlug,
        string vacancyTitle, string cV, UserProfileResponse userInfo)
    {
        var content = $@"
<center>
        <table class='body-wrap' style='text-align:center;width:86%;font-family:arial,sans-serif;border-spacing:4px 20px;'>
            <tr>
                <img src='https://jobs.srp.tj/images/srp/srp_icon.png' style='width:25%;'>
                <td>
                    <center>
                        <table bgcolor='#FFFFFF' width='80%' border='0'>
                            <tbody>                               
                                <tr style='font-size:14px;'>
                                    <td>
                                        <center>
                                            <h2><strong>New Vacancy Received</strong></h2>                                        
                                        </center>
                                    </td>
                                </tr>
                                <tr style='font-size:14px;height:45px;'>
                                    <td>
                                        <span>{userInfo.UserName} applied to the {vacancyTitle}</span>
                                    </td>
                                </tr>
                                <tr style='font-size:14px;height:45px;'>
                                    <td>
                                        <span>Applicant Details:</span>
<ul>
            <li><strong>Name:</strong> {userInfo.FirstName} {userInfo.LastName}</li>
            <li><strong>Email:</strong> {userInfo.Email}</li>
            <li><strong>PhoneNumber:</strong> {userInfo.PhoneNumber}</li>
            <li><strong>Resume:</strong> <a href='{hostName}applications/downloadCv/{cV}'> Download CV </a> </li>
            </ul>
                                    </td>
                                </tr>
                                <tr style='text-align:center'>
                                    <td>
                                    <a href='https://www.jobs.srp.tj/ApplicationDetail/{applicationSlug}/{userInfo.UserName}' style='display:inline-block;background-color:#1c92c8;color:#fff;padding:10px 20px;text-decoration:none;border-radius:5px;'>Перейти к заявке</a>                                    
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
        return content;
    }

    public string GenerateApplyVacancyContent_NoVacancy(string hostName, string applicationSlug, string cV,
        CreateApplicationCommand userInfo)
    {
        var content = $"""
                       
                                   <div style="display: flex;flex-direction: column;width: 100%">
                                   <h2><strong>Hidden Vacancy apply</strong></h2>
                                   <ul>
                                   <li><strong>Name:</strong> {userInfo.VacancyResponses.FirstOrDefault(s => s.VacancyQuestion.Question == "Your name")?.Response}</li>
                                   <li><strong>PhoneNumber:</strong> {userInfo.VacancyResponses.FirstOrDefault(s => s.VacancyQuestion.Question == "Your phone number")?.Response}</li>
                                   <li><strong>Resume:</strong> <a href='{hostName}applications/downloadCv/{cV}'> Download CV </a> </li>
                                   </ul>
                                   <p>Link to application: <a href='{hostName}dashboard/applications/{applicationSlug}'>{hostName}dashboard/applications/{applicationSlug}</a></p>
                                   </div>

                       """;
        return content;
    }

    public async Task<bool> EmailApproved(UserProfileResponse applicant, string slug)
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
                                <tr style='font-size:14px;'>
                                    <td>
                                     <b>{applicant.FirstName} {applicant.LastName}</b><span> ,мы рады сообщить вам, что ваше резюме было одобрено, и мы хотели бы пригласить вас на собеседование для дальнейшего обсуждения вашего опыта и возможного вступления в нашу команду.</span>
                                    </td>
                                </tr>
                                <tr style='font-size:14px;height:45px;'>
                                    <td>
                                        <span>Мы будем связаться с вами в ближайшие дни для уточнения дополнительных деталей и подтверждения вашего участия.</span>
                                    </td>
                                </tr>
                                <tr style='font-size:14px;height:45px;'>
                                    <td>
                                        <span>С наилучшими пожеланиями, SRP Team!</span>
                                    </td>
                                </tr>
                                <tr style='text-align:center'>
                                    <td>
                                      <a href='https://www.jobs.srp.tj/ApplicationDetail/{slug}' style='display:inline-block;background-color:#1c92c8;color:#fff;padding:10px 20px;text-decoration:none;border-radius:5px;'>Перейти к заявке</a>
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
        return await emailService.SendEmailAsync(new[] { applicant.Email }, htmlContent, subject);
    }

    public async Task<bool> EmailRejected(UserProfileResponse applicant,string slug)
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
                                <tr style='font-size:14px;'>
                                    <td>
                                     <b>{applicant.FirstName} {applicant.LastName}</b><span>, благодарим вас за проявленный интерес к вакансии в нашей компании. К сожалению, после тщательного рассмотрения вашего резюме мы приняли решение не продолжать процесс с вами.</span>
                                    </td>
                                </tr>
                                <tr style='font-size:14px;height:45px;'>
                                    <td>
                                        <span>Мы ценим ваше время и усилия, вложенные в подачу заявки, и надеемся, что вы найдете подходящую возможность в другом месте.</span>
                                    </td>
                                </tr>
                                <tr style='font-size:14px;height:45px;'>
                                    <td>
                                        <span>С наилучшими пожеланиями, SRP Team!</span>
                                    </td>
                                </tr>
                                <tr style='text-align:center'>
                                    <td>
                                      <a href='https://www.jobs.srp.tj/ApplicationDetail/{slug}' style='display:inline-block;background-color:#1c92c8;color:#fff;padding:10px 20px;text-decoration:none;border-radius:5px;'>Перейти к заявке</a>
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
        return await emailService.SendEmailAsync(new[] { applicant.Email }, htmlContent, subject);
    }
}