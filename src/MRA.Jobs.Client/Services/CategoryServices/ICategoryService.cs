using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.VacancyCategories.Commands;
using MRA.Jobs.Application.Contracts.VacancyCategories.Queries;
using MRA.Jobs.Application.Contracts.VacancyCategories.Responces;

namespace MRA.Jobs.Client.Services.CategoryServices;

public interface ICategoryService
{
    List<VacancyCategoryListDTO> Category { get; set; }
    UpdateVacancyCategoryCommand updatingEntity { get; set; }
    DeleteVacancyCategoryCommand deletingEntity { get; set; }
    CreateVacancyCategoryCommand creatingEntity { get; set; }
    Task<List<VacancyCategoryListDTO>> GetAllCategory();
    Task OnSaveUpdateClick();
    Task OnDeleteClick(Guid id);
    Task OnSaveCreateClick();
    void OnUpdateClick(VacancyCategoryListDTO updateEntity);
}
