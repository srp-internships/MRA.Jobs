using MRA.BlazorComponents.HttpClient.Responses;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Responses;
using MRA.Jobs.Application.Contracts.JobVacancies.Commands.CreateJobVacancy;
using MRA.Jobs.Application.Contracts.JobVacancies.Responses;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;
using MRA.Jobs.Application.Contracts.VacancyCategories.Responses;

namespace MRA.Jobs.Client.Services.VacanciesServices.Jobs;

public interface IJobsService
{
    event Action OnChange;
   
    List<JobVacancyListDto> Vacancies { get; set; }
    public int FilteredVacanciesCount { get; set; }
    CreateJobVacancyCommand creatingNewJob { get; set; } 
    Task<List<CategoryResponse>> GetAllCategory();
    Task<List<JobVacancyListDto>> GetVacancyByTitle(string title);
    Task<ApiResponse<string>> OnSaveCreateClick();
    Task<ApiResponse> OnDelete(string slug);
    Task<List<JobVacancyListDto>> GetJobs();
    Task<ApiResponse<string>> UpdateJobVacancy(string slug);
    Task<JobVacancyDetailsDto> GetBySlug(string slug);
    Task<List<InternshipVacancyListResponse>> GetInternship();
    Task<List<TrainingVacancyListDto>> GetTrainings();

    Task ChangeNoteAsync(JobVacancyListDto vacancy);
   
}
