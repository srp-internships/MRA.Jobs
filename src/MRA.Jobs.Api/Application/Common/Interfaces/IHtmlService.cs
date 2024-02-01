using MRA.Identity.Application.Contract.Profile.Responses;
using MRA.Jobs.Application.Contracts.Applications.Commands.CreateApplication;

namespace MRA.Jobs.Application.Common.Interfaces;

public interface IHtmlService
{
    string GenerateApplyVacancyContent(string userName);

    string GenerateApplyVacancyContent_CreateApplication(string hostName, string applicationSlug, string vacancyTitle,
        string cV, UserProfileResponse userInfo);

    string GenerateApplyVacancyContent_NoVacancy(string hostName, string applicationSlug,
        string cV, CreateApplicationCommand createApplicationCommand);

    Task<bool> EmailApproved(UserProfileResponse applicant, string slug);
    Task<bool> EmailRejected(UserProfileResponse applicant, string slug, string vacancyTitle);
    Task<bool> EmailCustom(string emailSubject, string emailText, string emailAddress);
}