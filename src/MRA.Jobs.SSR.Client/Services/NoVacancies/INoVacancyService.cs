using Microsoft.AspNetCore.Components.Forms;
using MRA.Jobs.Application.Contracts.Applications.Commands.CreateApplication;
using MRA.Jobs.Application.Contracts.JobVacancies.Queries.GetJobVacancyBySlug;
using MRA.Jobs.Application.Contracts.JobVacancies.Responses;

namespace MRA.Jobs.SSR.Client.Services.NoVacancies;

public interface INoVacancyService
{
    Task<JobVacancyDetailsDto> GetNoVacancyAsync();

    Task CreateApplicationNoVacancyAsync(CreateApplicationCommand command, IBrowserFile file);
}