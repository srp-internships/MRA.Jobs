using MRA.Identity.Application.Contract.Profile.Responses;

namespace MRA.Jobs.Infrastructure.Services;

public class HtmlService : IHtmlService
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
    public string GenerateApplyVacancyContent_CreateApplication(string VacancyTitle, string CV, UserProfileResponse userInfo)
    {
        var content = $@"
            <div style=""display: flex;flex-direction: column;width: 100%"">
            <h2><strong>New Job Application Received</strong></h2>
            <h2>{userInfo.UserName} applied to the {VacancyTitle}</h2>
            <h2>Applicant Details:</h2>
            <ul>
            <li><strong>Name:</strong> {userInfo.FirstName} {userInfo.LastName}</li>
            <li><strong>Email:</strong>{userInfo.Email}</li>
            <li><strong>PhoneNumber:</strong>{userInfo.PhoneNumber}</li>
            <li><strong>Resume:</strong> {CV}</li>
            </ul>
            <p>Link to application: <a href=""http://192.168.1.17:5006/dashboard/applications/{userInfo.UserName}""></a></p>
            <button type=""button"" onclick=""location.href='#your_url'"">Link to application</button>
            </div>
";
        return content;
    }
}