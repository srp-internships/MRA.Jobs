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
    public string GenerateApplyVacancyContent_CreateApplication(string userName)
    {
        var content = $@"
            <div style=""display: flex;flex-direction: column;width: 100%"">
            <h2><strong>New Job Application Received</strong></h2>
            <h2>{userName} applied to the</h2>
            <h2>Applicant Details:</h2>
            <ul>
            <li><strong>Name:</strong> [Candidate's Full Name]</li>
            <li><strong>Email:</strong> [Candidate's Email Address]</li>
            <li><strong>PhoneNumber:</strong> [Phone Number]</li>
            <li><strong>Resume:</strong> [Attach the resume or provide a link to download]</li>
            </ul>
            <p>Link to application: <a href=""http://192.168.1.17:5006/dashboard/applications/komdil-senior.net-1-1"">http://192.168.1.17:5006/dashboard/applications/komdil-senior.net-1-1</a></p>
            <button type=""button"" onclick=""location.href='#your_url'"">view</button>
            </div>
";
        return content;
    }
}