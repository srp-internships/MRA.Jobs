using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.JobVacancies.Commands;
using MRA.Jobs.Application.Contracts.JobVacancies.Responses;
using MRA.Jobs.Application.Contracts.VacancyCategories.Commands;
using MRA.Jobs.Application.Contracts.VacancyCategories.Responses;

namespace MRA.Jobs.Client.Services.VacancyServices;

public interface IVacancyService
{
    event Action OnChange;
    string guidId { get; set; }
    List<CategoryResponse> Categories { get; set; }
    List<JobVacancyListDTO> Vacanceies { get; set; }
  //  CreateVacancyCategoryCommand creatingEntity { get; set; }
    CreateJobVacancyCommand creatingNewJob { get; set; }
    Task<List<JobVacancyListDTO>> GetAllVacancy();
    Task<List<CategoryResponse>> GetAllCategory();

    Task<List<JobVacancyListDTO>> GetVacancyByTitle(string title);
    Task OnSaveCreateClick();

    Task<List<JobVacancyListDTO>> GetJobs();
}
