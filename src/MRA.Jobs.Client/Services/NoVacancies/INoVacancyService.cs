using Microsoft.AspNetCore.Components.Forms;
using MRA.Jobs.Application.Contracts.Applications.Commands.CreateApplication;
using MRA.Jobs.Application.Contracts.NoVacancies.Responses;

namespace MRA.Jobs.Client.Services.NoVacancies;

public interface INoVacancyService
{
    Task<NoVacancyResponse> GetNoVacancy();

    Task CreateApplicationNoVacancy(CreateApplicationNoVacancyCommand command, IBrowserFile file);
}