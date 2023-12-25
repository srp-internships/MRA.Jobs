﻿using MRA.Identity.Application.Contract.Profile.Responses;
using MRA.Jobs.Application.Contracts.Applications.Commands.CreateApplication;

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

    public string GenerateApplyVacancyContent_CreateApplication(string hostName, string applicationSlug,
        string vacancyTitle, string cV, UserProfileResponse userInfo)
    {
        var content = $@"
            <div style=""display: flex;flex-direction: column;width: 100%"">
            <h2><strong>New Job Application Received</strong></h2>
            <h2>{userInfo.UserName} applied to the {vacancyTitle}</h2>
            <h2>Applicant Details:</h2>
            <ul>
            <li><strong>Name:</strong> {userInfo.FirstName} {userInfo.LastName}</li>
            <li><strong>Email:</strong> {userInfo.Email}</li>
            <li><strong>PhoneNumber:</strong> {userInfo.PhoneNumber}</li>
            <li><strong>Resume:</strong> <a href='{hostName}applications/downloadCv/{cV}'> Download CV </a> </li>
            </ul>
            <p>Link to application: <a href='{hostName}dashboard/applications/{applicationSlug}'>{hostName}dashboard/applications/{applicationSlug}</a></p>
            </div>
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
}