using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Responses;
using MRA.Jobs.Application.Contracts.JobVacancies.Commands.CreateJobVacancy;
using MRA.Jobs.Application.Contracts.JobVacancies.Responses;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;
using MRA.Jobs.Application.Contracts.VacancyCategories.Commands;
using MRA.Jobs.Application.Contracts.VacancyCategories.Responses;

namespace MRA.Jobs.Client.Services.VacancyServices;

public interface IVacancyService
{
    event Action OnChange;
    string guidId { get; set; }
    public int PagesCount { get; set; }
    List<CategoryResponse> Categories { get; set; }
    List<JobVacancyListDto> Vacanceies { get; set; }
    //  CreateVacancyCategoryCommand creatingEntity { get; set; }
    public int FilteredVacanciesCount { get; set; }
    CreateJobVacancyCommand creatingNewJob { get; set; } 
    Task<List<JobVacancyListDto>> GetAllVacancy();
    /* Renamed version of the upper method name with a typo */
    Task<List<JobVacancyListDto>> GetAllVacancies();
    Task<List<JobVacancyListDto>> GetFilteredVacancies(string title = "", string categoryName = "All categories", int page = 1);
    Task<List<CategoryResponse>> GetAllCategory();

    Task<List<JobVacancyListDto>> GetVacancyByTitle(string title);
    Task<HttpResponseMessage> OnSaveCreateClick();
    Task OnDelete(string slug);
    Task<List<JobVacancyListDto>> GetJobs();
    Task<List<InternshipVacancyListResponse>> GetInternship();
    Task<List<TrainingVacancyListDto>> GetTrainings();
    Task UpdateJobVacancy(string slug);
    Task<JobVacancyDetailsDto> GetBySlug(string slug);
}
