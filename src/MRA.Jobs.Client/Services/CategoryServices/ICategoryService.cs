using MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;
using MRA.Jobs.Application.Contracts.VacancyCategories.Commands;
using MRA.Jobs.Application.Contracts.VacancyCategories.Responses;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Responses;

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
    Task<List<TrainingCategoriesResponse>> GetTrainingCategories();
    Task<List<TrainingCategoriesResponse>> GetTrainingCategoriesSinceCheckDate();

    Task<List<InternshipCategoriesResponse>> GetInternshipCategories();
    Task<List<InternshipCategoriesResponse>> GetInternshipCategoriesSinceCheckDate();
}
