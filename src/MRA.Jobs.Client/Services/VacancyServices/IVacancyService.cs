using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.JobVacancies.Commands;
using MRA.Jobs.Application.Contracts.JobVacancies.Responses;
using MRA.Jobs.Application.Contracts.VacancyCategories.Commands;
using MRA.Jobs.Application.Contracts.VacancyCategories.Responses;
using MRA.Jobs.Client.Pages.Admin;

namespace MRA.Jobs.Client.Services.VacancyServices;

public interface IVacancyService
{
    public int PagesCount { get; set; }
    event Action OnChange;
    string guidId { get; set; }
    List<CategoryResponse> Categories { get; set; }
    List<JobVacancyListDTO> VacanciesOnPage { get; set; }
    public int FilteredVacanciesCount { get; set; }
    public List<JobVacancyListDTO> AllVacancies { get; set; }
    //  CreateVacancyCategoryCommand creatingEntity { get; set; }
    CreateJobVacancyCommand creatingNewJob { get; set; }
    public int VacanciesCountPerCategory(string categoryName);
    public Task InitAllVacancies();
    Task<List<JobVacancyListDTO>> GetFilteredVacancies(string title = "", string categoryName = "All categories", int page = 1);
    Task OnSaveCreateClick();
    Task OnDelete(Guid Id);
    Task UpdateJobVacancy(Guid id);
    Task<JobVacancyDetailsDTO> GetById(Guid Id);
}