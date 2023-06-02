using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.JobVacancies.Commands;
using MRA.Jobs.Application.Contracts.JobVacancies.Responses;
using MRA.Jobs.Application.Contracts.VacancyCategories.Commands;
using MRA.Jobs.Application.Contracts.VacancyCategories.Responses;

namespace MRA.Jobs.Client.Services.VacancyServices;

public interface IVacancyService
{
    string guidId { get; set; }
    List<CategoryResponse> Categories { get; set; }
    List<JobVacancyListDTO> Vacanceies { get; set; }
    Task<List<JobVacancyListDTO>> GetAllVacancy();
    CreateJobVacancyCommand creatingNewJob { get; set; }
    CreateVacancyCategoryCommand creatingEntity { get; set; }
    Task<List<CategoryResponse>> GetAllCategory();
    Task OnSaveCreateClick();
}
