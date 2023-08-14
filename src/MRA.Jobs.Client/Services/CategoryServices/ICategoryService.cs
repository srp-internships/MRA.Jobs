using MRA.Jobs.Application.Contracts.VacancyCategories.Commands;
using MRA.Jobs.Application.Contracts.VacancyCategories.Responses;

namespace MRA.Jobs.Client.Services.CategoryServices;

public interface ICategoryService
{
    List<CategoryResponse> Category { get; set; }
    UpdateVacancyCategoryCommand updatingEntity { get; set; }
    DeleteVacancyCategoryCommand deletingEntity { get; set; }
    CreateVacancyCategoryCommand creatingEntity { get; set; }
    Task<List<CategoryResponse>> GetAllCategory();
    Task OnSaveUpdateClick();
    Task OnDeleteClick(string slug);
    Task OnSaveCreateClick();
    void OnUpdateClick(CategoryResponse updateEntity);
}
