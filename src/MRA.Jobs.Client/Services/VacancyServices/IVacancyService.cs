using MRA.Jobs.Application.Contracts.InternshipVacancies.Queries.GetInternships;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Responses;
using MRA.Jobs.Application.Contracts.JobVacancies.Commands.CreateJobVacancy;
using MRA.Jobs.Application.Contracts.JobVacancies.Responses;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Queries;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;
using MRA.Jobs.Application.Contracts.VacancyCategories.Responses;

namespace MRA.Jobs.Client.Services.VacancyServices;

public interface IVacancyService
{
    event Action OnChange;
   
    List<JobVacancyListDto> Vacancies { get; set; }
 
    public int FilteredVacanciesCount { get; set; }
    CreateJobVacancyCommand creatingNewJob { get; set; } 
   
    /* Renamed version of the upper method name with a typo */
    // Task<List<JobVacancyListDto>> GetAllVacancies();
    // Task<List<JobVacancyListDto>> GetFilteredVacancies(string title = "", string categoryName = "All categories", int page = 1);
    Task<List<CategoryResponse>> GetAllCategory();

    Task<List<JobVacancyListDto>> GetVacancyByTitle(string title);
    Task<ApiResponse<string>> OnSaveCreateClick();
    Task<ApiResponse> OnDelete(string slug);
    Task<List<JobVacancyListDto>> GetJobs();
    Task<ApiResponse<string>> UpdateJobVacancy(string slug);
    Task<JobVacancyDetailsDto> GetBySlug(string slug);
    Task<List<InternshipVacancyListResponse>> GetInternship();
    Task<List<TrainingVacancyListDto>> GetTrainings();

}
