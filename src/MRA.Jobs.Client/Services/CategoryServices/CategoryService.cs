using MRA.BlazorComponents.Configuration;
using MRA.BlazorComponents.HttpClient.Responses;
using MRA.BlazorComponents.HttpClient.Services;
using MRA.BlazorComponents.Snackbar.Extensions;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Responses;
using MRA.Jobs.Application.Contracts.JobVacancies.Responses;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;
using MRA.Jobs.Application.Contracts.VacancyCategories.Commands.CreateVacancyCategory;
using MRA.Jobs.Application.Contracts.VacancyCategories.Commands.DeleteVacancyCategory;
using MRA.Jobs.Application.Contracts.VacancyCategories.Commands.UpdateVacancyCategory;
using MRA.Jobs.Application.Contracts.VacancyCategories.Responses;
using MRA.Jobs.Client.Services.ContentService;
using MudBlazor;

namespace MRA.Jobs.Client.Services.CategoryServices;

public class CategoryService(
    IHttpClientService httpClient,
    IConfiguration configuration,
    ISnackbar snackbar,
    IContentService contentService)
    : ICategoryService
{
    public List<CategoryResponse> Category { get; set; }
    public UpdateVacancyCategoryCommand updatingEntity { get; set; }
    public DeleteVacancyCategoryCommand deletingEntity { get; set; }
    public CreateVacancyCategoryCommand creatingEntity { get; set; }


    public async Task<ApiResponse<PagedList<CategoryResponse>>> GetAllCategory()
    {
        var result =
            await httpClient.GetFromJsonAsync<PagedList<CategoryResponse>>(configuration.GetJobsUrl("categories"));
        Category = result.Result.Items;
        creatingEntity = new() { Name = "" };
        return result;
    }

    public void OnUpdateClick(CategoryResponse updateEntity)
    {
        updatingEntity = new() { Slug = updateEntity.Slug, Name = updateEntity.Name };
    }

    public async Task OnSaveUpdateClick()
    {
        var result =
            await httpClient.PutAsJsonAsync<string>(configuration.GetJobsUrl($"categories/{updatingEntity.Slug}"),
                updatingEntity);
        if (result.Success)
        {
            updatingEntity = null;
        }

        var result2 =
            await httpClient.GetFromJsonAsync<PagedList<CategoryResponse>>(configuration.GetJobsUrl("categories"));
        Category = result2.Result.Items;
    }

    public async Task OnDeleteClick(string slug)
    {
        await httpClient.DeleteAsync(configuration.GetJobsUrl($"categories/{slug}"));
        var result =
            await httpClient.GetFromJsonAsync<PagedList<CategoryResponse>>(configuration.GetJobsUrl("categories"));
        Category = result.Result.Items;
    }

    public async Task OnSaveCreateClick()
    {
        if (creatingEntity is not null)
            await httpClient.PostAsJsonAsync<string>(configuration.GetJobsUrl("categories"), creatingEntity);
        creatingEntity.Name = string.Empty;
        var result =
            await httpClient.GetFromJsonAsync<PagedList<CategoryResponse>>(configuration.GetJobsUrl("categories"));
        Category = result.Result.Items;
    }

    public async Task<List<TrainingCategoriesResponce>> GetTrainingCategories()
    {
        var response =await httpClient.GetFromJsonAsync<List<TrainingCategoriesResponce>>(
            configuration.GetJobsUrl("categories/training"));
        snackbar.ShowIfError(response, contentService["ServerIsNotResponding"]);
        return response.Success ? response.Result : null;
    }

    public async Task<List<TrainingCategoriesResponce>> GetTrainingCategoriesSinceCheckDate()
    {
        var response =
            await httpClient.GetFromJsonAsync<List<TrainingCategoriesResponce>>(
                configuration.GetJobsUrl("categories/training"));
        return response.Success ? response.Result : null;
    }

    public async Task<List<InternshipCategoriesResponse>> GetInternshipCategories()
    {
        var response =
            await httpClient.GetFromJsonAsync<List<InternshipCategoriesResponse>>(
                configuration.GetJobsUrl("categories/internships"));
        snackbar.ShowIfError(response, contentService["ServerIsNotResponding"]);
        return response.Success ? response.Result : null;
    }
    

    public async Task<List<JobCategoriesResponse>> GetJobCategories()
    {
        var response =await httpClient.GetFromJsonAsync<List<JobCategoriesResponse>>(
            configuration.GetJobsUrl("categories/job"));
        snackbar.ShowIfError(response, contentService["ServerIsNotResponding"]);
        return response.Success ? response.Result : null;
    }
}