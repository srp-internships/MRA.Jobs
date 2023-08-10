using MRA.Jobs.Application.Contracts.JobVacancies.Commands;
using MRA.Jobs.Application.Contracts.JobVacancies.Responses;
using MRA.Jobs.Application.Contracts.VacancyCategories.Responses;

namespace MRA.Jobs.Client.Services.VacancyServices;

public interface IVacancyService
{
    string guidId { get; set; }
    List<CategoryResponse> Categories { get; set; }

    List<JobVacancyListDto> Vacanceies { get; set; }

    //  CreateVacancyCategoryCommand creatingEntity { get; set; }
    CreateJobVacancyCommand creatingNewJob { get; set; }
    event Action OnChange;
    Task<List<JobVacancyListDto>> GetAllVacancy();
    Task<List<CategoryResponse>> GetAllCategory();

    Task<List<JobVacancyListDto>> GetVacancyByTitle(string title);
    Task OnSaveCreateClick();
    Task OnDelete(Guid Id);
    Task<List<JobVacancyListDto>> GetJobs();
    Task UpdateJobVacancy(Guid id);
    Task<JobVacancyDetailsDto> GetById(Guid Id);
}