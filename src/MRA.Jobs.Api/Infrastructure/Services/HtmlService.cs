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
              <h1 style='margin: 0; font-size: 32px; font-weight: 700; letter-spacing: -1px; line-height: 48px;'>Заявка одобрено!</h1>
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
              <p style='margin: 0;'><b>{applicant.FirstName} {applicant.LastName}</b><span> ,мы рады сообщить вам, что ваше резюме было одобрено, и мы хотели бы пригласить вас на собеседование для дальнейшего обсуждения вашего опыта и возможного вступления в нашу команду.</span></p>
            </td>
          </tr>
          <tr>
            <td align='left' bgcolor='#ffffff' style='padding: 24px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 16px; line-height: 24px;'>
              <p style='margin: 0;'> Мы будем связаться с вами в ближайшие дни для уточнения дополнительных деталей и подтверждения вашего участия.</p>
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
                          <a href='https://www.jobs.srp.tj/ApplicationDetail/{slug}' style='display: inline-block; padding: 16px 36px; font-size: 16px; color: #ffffff; text-decoration: none; border-radius: 6px;'>Перейти к заявке</a>
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
              <p style='margin: 0;'>С наилучшими пожеланиями, SRP Team!</p>
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
        var subject = "Заявка одобрено!";
        return await emailService.SendEmailAsync(new[] { applicant.Email }, htmlContent, subject);
    }

    public async Task<bool> EmailRejected(UserProfileResponse applicant,string slug)
    {
        string htmlContent = $@"

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
            <td align='left' bgcolor='#ffffff' style='padding: 36px 24px 0; border-top: 3px solid #d4dadf;'>
              <h1 style='margin: 0; font-size: 32px; font-weight: 700; letter-spacing: -1px; line-height: 48px;'>Уведомление об отказе</h1>
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
              <p style='margin: 0;'><b>{applicant.FirstName} {applicant.LastName}</b><span>, благодарим вас за проявленный интерес к вакансии в нашей компании. К сожалению, после тщательного рассмотрения вашего резюме мы приняли решение не продолжать процесс с вами.</p>
            </td>
          </tr>
          <tr>
            <td align='left' bgcolor='#ffffff' style='padding: 24px; font-family: 'Source Sans Pro', Helvetica, Arial, sans-serif; font-size: 16px; line-height: 24px;'>
              <p style='margin: 0;'>Мы ценим ваше время и усилия, вложенные в подачу заявки, и надеемся, что вы найдете подходящую возможность в другом месте.</p>
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
                          <a href='https://www.jobs.srp.tj/ApplicationDetail/{slug}' style='display: inline-block; padding: 16px 36px; font-size: 16px; color: #ffffff; text-decoration: none; border-radius: 6px;'>Перейти к заявке</a>
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
              <p style='margin: 0;'>С наилучшими пожеланиями, SRP Team!</p>
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

        var subject = "Уведомление об отказе";
        return await emailService.SendEmailAsync(new[] { applicant.Email }, htmlContent, subject);
    }
}