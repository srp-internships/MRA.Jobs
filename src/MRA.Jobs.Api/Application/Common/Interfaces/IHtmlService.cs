using MRA.Identity.Application.Contract.Profile.Responses;

namespace MRA.Jobs.Application.Common.Interfaces;

public interface IHtmlService
{
    string GenerateApplyVacancyContent(string userName); 
    string GenerateApplyVacancyContent_CreateApplication(string VacancyTitle, string CV, UserProfileResponse userInfo );
}