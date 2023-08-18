using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.JobVacancies.Commands;
using MRA.Jobs.Application.Contracts.JobVacancies.Responses;
using MRA.Jobs.Application.Contracts.VacancyCategories.Commands;
using MRA.Jobs.Application.Contracts.VacancyTag.VacancyCategories.Responses;

namespace MRA.Jobs.Client.Services.VacancyServices;

public interface IVacancyService
{
    event Action OnChange;
    string guidId { get; set; }
    List<CategoryResponse> Categories { get; set; }
    List<JobVacancyListDto> Vacanceies { get; set; }
    //  CreateVacancyCategoryCommand creatingEntity { get; set; }
    CreateJobVacancyCommand creatingNewJob { get; set; }
    Task<List<JobVacancyListDto>> GetAllVacancy();
    Task<List<CategoryResponse>> GetAllCategory();

    Task<List<JobVacancyListDto>> GetVacancyByTitle(string title);
    Task OnSaveCreateClick();
    Task OnDelete(string slug);
    Task<List<JobVacancyListDto>> GetJobs();
    Task UpdateJobVacancy(string slug);
    Task<JobVacancyDetailsDto> GetBySlug(string slug);
}
