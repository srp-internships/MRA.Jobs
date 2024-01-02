using MRA.Jobs.Application.Contracts.InternshipVacancies.Queries;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Responses;
using MRA.Jobs.Application.Contracts.JobVacancies.Queries.GetJobCategories;
using MRA.Jobs.Application.Contracts.JobVacancies.Responses;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Queries;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;
using MRA.Jobs.Application.Contracts.VacancyCategories.Commands.CreateVacancyCategory;
using MRA.Jobs.Application.Contracts.VacancyCategories.Commands.DeleteVacancyCategory;
using MRA.Jobs.Application.Contracts.VacancyCategories.Commands.UpdateVacancyCategory;
using MRA.Jobs.Application.Contracts.VacancyCategories.Queries.GetVacancyCategorySlugId;
using MRA.Jobs.Application.Contracts.VacancyCategories.Responses;

namespace MRA.Jobs.Client.Services.CategoryServices;

public interface ICategoryService
{
    List<CategoryResponse> Category { get; set; }
    UpdateVacancyCategoryCommand updatingEntity { get; set; }
    DeleteVacancyCategoryCommand deletingEntity { get; set; }
    CreateVacancyCategoryCommand creatingEntity { get; set; }
    Task<ApiResponse<List<CategoryResponse>>> GetAllCategory(GetVacancyCategoryByIdQuery query);
    Task OnSaveUpdateClick();
    Task OnDeleteClick(string slug);
    Task OnSaveCreateClick();
    void OnUpdateClick(CategoryResponse updateEntity);
    Task<ApiResponse<List<TrainingCategoriesResponce>>> GetTrainingCategories(GetTrainingCategoriesQuery getTrainingCategoriesQuery);
    Task<ApiResponse<List<TrainingCategoriesResponce>>> GetTrainingCategoriesSinceCheckDate(GetTrainingCategoriesQuery getTrainingCategoriesQuery);
    Task<ApiResponse<List<InternshipCategoriesResponce>>> GetInternshipCategories(GetInternshipCategoriesQuery getInternshipCategoriesQuery);
    Task<ApiResponse<List<InternshipCategoriesResponce>>> GetInternshipCategoriesSinceCheckDate(GetInternshipCategoriesQuery getInternshipCategoriesQuery);

    Task<ApiResponse<List<JobCategoriesResponse>>> GetJobCategories(GetJobCategoriesQuery getJobCategoriesQuery);
    Task<ApiResponse<List<JobCategoriesResponse>>> GetJobCategoriesSinceCheckDate(GetJobCategoriesQuery getJobCategoriesQuery);
}
