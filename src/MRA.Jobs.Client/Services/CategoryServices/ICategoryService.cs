using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.VacancyCategories.Queries;
using MRA.Jobs.Application.Contracts.VacancyCategories.Responces;

namespace MRA.Jobs.Client.Services.CategoryServices;

public interface ICategoryService
{
    List<VacancyCategoryListDTO> Category { get; set; }
    Task<PaggedList<VacancyCategoryListDTO>> GetAllCategory();

    event Action ProductChanged;
}
