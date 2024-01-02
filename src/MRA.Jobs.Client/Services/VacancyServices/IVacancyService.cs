using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Queries.GetInternships;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Responses;
using MRA.Jobs.Application.Contracts.JobVacancies.Commands.CreateJobVacancy;
using MRA.Jobs.Application.Contracts.JobVacancies.Queries.GetJobs;
using MRA.Jobs.Application.Contracts.JobVacancies.Queries.GetJobVacancyBySlug;
using MRA.Jobs.Application.Contracts.JobVacancies.Responses;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Queries;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;
using MRA.Jobs.Application.Contracts.VacancyCategories.Commands;
using MRA.Jobs.Application.Contracts.VacancyCategories.Queries.GetVacancyCategorySlugId;
using MRA.Jobs.Application.Contracts.VacancyCategories.Responses;

namespace MRA.Jobs.Client.Services.VacancyServices;

public interface IVacancyService
{
    event Action OnChange;
    // public int PagesCount { get; set; }
    // List<CategoryResponse> Categories { get; set; }
    List<JobVacancyListDto> Vacanceies { get; set; }
    //  CreateVacancyCategoryCommand creatingEntity { get; set; }
    public int FilteredVacanciesCount { get; set; }
    CreateJobVacancyCommand creatingNewJob { get; set; } 
    Task<List<JobVacancyListDto>> GetAllVacancy(GetJobsQueryOptions getJobsQuery);
    /* Renamed version of the upper method name with a typo */
    // Task<List<JobVacancyListDto>> GetAllVacancies();
    // Task<List<JobVacancyListDto>> GetFilteredVacancies(string title = "", string categoryName = "All categories", int page = 1);
    Task<List<CategoryResponse>> GetAllCategory(GetVacancyCategoryByIdQuery getVacancyCategoryByIdQuery );

    Task<List<JobVacancyListDto>> GetVacancyByTitle(string title ,GetJobsQueryOptions getJobsQuery);
    Task<ApiResponse> OnSaveCreateClick();
    Task<ApiResponse> OnDelete(string slug);
    Task<List<JobVacancyListDto>> GetJobs(GetJobsQueryOptions getJobsQuery);
    Task<ApiResponse> UpdateJobVacancy(string slug);
    Task<JobVacancyDetailsDto> GetBySlug(string slug,GetJobVacancyBySlugQuery getJobVacancyBySlug);
    Task<List<InternshipVacancyListResponse>> GetInternship(GetInternshipsQueryOptions getInternshipsQuery);
    Task<List<TrainingVacancyListDto>> GetTrainings(GetTrainingsQueryOptions getTrainingsQuery);

}
