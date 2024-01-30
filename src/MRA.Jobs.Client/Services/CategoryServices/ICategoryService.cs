using MRA.BlazorComponents.HttpClient.Responses;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Responses;
using MRA.Jobs.Application.Contracts.JobVacancies.Responses;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;
using MRA.Jobs.Application.Contracts.VacancyCategories.Commands.CreateVacancyCategory;
using MRA.Jobs.Application.Contracts.VacancyCategories.Commands.DeleteVacancyCategory;
using MRA.Jobs.Application.Contracts.VacancyCategories.Commands.UpdateVacancyCategory;
using MRA.Jobs.Application.Contracts.VacancyCategories.Responses;

namespace MRA.Jobs.Client.Services.CategoryServices;

public interface ICategoryService
{
    List<CategoryResponse> Category { get; set; }
    UpdateVacancyCategoryCommand updatingEntity { get; set; }
    DeleteVacancyCategoryCommand deletingEntity { get; set; }
    CreateVacancyCategoryCommand creatingEntity { get; set; }
    Task<ApiResponse<PagedList<CategoryResponse>>> GetAllCategory();
    Task OnSaveUpdateClick();
    Task OnDeleteClick(string slug);
    Task OnSaveCreateClick();
    void OnUpdateClick(CategoryResponse updateEntity);
    Task<ApiResponse<List<TrainingCategoriesResponce>>> GetTrainingCategories();
    Task<List<TrainingCategoriesResponce>> GetTrainingCategoriesSinceCheckDate();
    Task<ApiResponse<List<InternshipCategoriesResponce>>> GetInternshipCategories();
    Task<List<InternshipCategoriesResponce>> GetInternshipCategoriesSinceCheckDate();

    Task<ApiResponse<List<JobCategoriesResponse>>> GetJobCategories();
    Task<List<JobCategoriesResponse>> GetJobCategoriesSinceCheckDate();
}
