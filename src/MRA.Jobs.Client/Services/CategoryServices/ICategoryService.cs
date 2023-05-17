using MRA.Jobs.Application.Contracts.VacancyCategories.Queries;

namespace MRA.Jobs.Client.Services.CategoryServices;

public interface ICategoryService
{
    List<GetVacancyCategoriesQuery> Category { get; set; }
    Task<List<GetVacancyCategoriesQuery>> GetAllCategory();
}
