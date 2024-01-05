using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Responses;
using MRA.Jobs.Application.Contracts.JobVacancies.Responses;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;
using MRA.Jobs.Application.Contracts.VacancyCategories.Commands.CreateVacancyCategory;
using MRA.Jobs.Application.Contracts.VacancyCategories.Commands.DeleteVacancyCategory;
using MRA.Jobs.Application.Contracts.VacancyCategories.Commands.UpdateVacancyCategory;
using MRA.Jobs.Application.Contracts.VacancyCategories.Responses;
using MRA.Jobs.Client.Services.HttpClients;

namespace MRA.Jobs.Client.Services.CategoryServices;

public class CategoryService(JobsApiHttpClientService httpClient)
    : ICategoryService
{
    public List<CategoryResponse> Category { get; set; }
    public UpdateVacancyCategoryCommand updatingEntity { get; set; }
    public DeleteVacancyCategoryCommand deletingEntity { get; set; }
    public CreateVacancyCategoryCommand creatingEntity { get; set; }


    public async Task<ApiResponse<PagedList<CategoryResponse>>> GetAllCategory()
    {
        return await httpClient.GetAsJsonAsync<PagedList<CategoryResponse>>("categories");
    }

    public void OnUpdateClick(CategoryResponse updateEntity)
    {
        updatingEntity = new()
        {
            Slug = updateEntity.Slug,
            Name = updateEntity.Name
        };
    }
    public async Task OnSaveUpdateClick()
    {
        var result = await httpClient.PutAsJsonAsync<string>($"categories/{updatingEntity.Slug}", updatingEntity);
        if (result.Success)
        {
            updatingEntity = null;
        }
        var result2 = await httpClient.GetAsJsonAsync<List<CategoryResponse>>("categories");
        Category = result2.Result;
    }
    public async Task OnDeleteClick(string slug)
    {
        await httpClient.DeleteAsync($"categories/{slug}");
        var result = await httpClient.GetAsJsonAsync<List<CategoryResponse>>("categories");
        Category = result.Result;
    }
    public async Task OnSaveCreateClick()
    {
        if (creatingEntity is not null)
            await httpClient.PostAsJsonAsync<string>("categories", creatingEntity);
        creatingEntity.Name = string.Empty;
        var result = await httpClient.GetAsJsonAsync<List<CategoryResponse>>("categories");
        Category = result.Result;
    }

    public async Task<ApiResponse<List<TrainingCategoriesResponce>>> GetTrainingCategories()
    {
        return await httpClient.GetAsJsonAsync<List<TrainingCategoriesResponce>>("categories/training");

    }

    public async Task<List<TrainingCategoriesResponce>> GetTrainingCategoriesSinceCheckDate()
    {
        var respons = await httpClient.GetAsJsonAsync<List<TrainingCategoriesResponce>>("categories/training");
        return respons.Success ? respons.Result : null;

    }

    public async Task<ApiResponse<List<InternshipCategoriesResponce>>> GetInternshipCategories()
    {
        return await httpClient.GetAsJsonAsync<List<InternshipCategoriesResponce>>("categories/internships");

    }

    public async Task<List<InternshipCategoriesResponce>> GetInternshipCategoriesSinceCheckDate()
    {
        var respons = await httpClient.GetAsJsonAsync<List<InternshipCategoriesResponce>>("categories/internships");
        return respons.Success ? respons.Result : null;
    }

    public async Task<ApiResponse<List<JobCategoriesResponse>>> GetJobCategories()
    {
        return await httpClient.GetAsJsonAsync<List<JobCategoriesResponse>>("categories/job");

    }

    public async Task<List<JobCategoriesResponse>> GetJobCategoriesSinceCheckDate()
    {
        var respons= await httpClient.GetAsJsonAsync<List<JobCategoriesResponse>>("categories/job");
        return respons.Success ? respons.Result : null;
    }
}
