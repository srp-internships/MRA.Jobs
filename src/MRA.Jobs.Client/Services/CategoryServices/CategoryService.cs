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
using MRA.Jobs.Client.Services.HttpClients;

namespace MRA.Jobs.Client.Services.CategoryServices;

public class CategoryService : ICategoryService
{

    private readonly IHttpClientService _httpClient;
    private readonly IConfiguration _configuration;

    public CategoryService(IHttpClientService httpClient, IConfiguration configuration)
    {

        _httpClient = httpClient;
        _configuration = configuration;
    }
    public List<CategoryResponse> Category { get; set; }
    public UpdateVacancyCategoryCommand updatingEntity { get; set; }
    public DeleteVacancyCategoryCommand deletingEntity { get; set; }
    public CreateVacancyCategoryCommand creatingEntity { get; set; }
    public GetVacancyCategoryByIdQuery query { get; set; }


    public async Task<ApiResponse<List<CategoryResponse>>> GetAllCategory(GetVacancyCategoryByIdQuery query)
    {
        return await _httpClient.GetAsJsonAsync<List<CategoryResponse>>($"{_configuration["HttpClient:BaseAddress"]}categories", query);
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
        var result = await _httpClient.PutAsJsonAsync<CategoryResponse>($"{_configuration["HttpClient:BaseAddress"]}categories/{updatingEntity.Slug}", updatingEntity);
        if (result.Success)
        {
            updatingEntity = null;
        }
        var result2 = await _httpClient.GetAsJsonAsync<List<CategoryResponse>>($"{_configuration["HttpClient:BaseAddress"]}categories", query);
        Category = result2.Result;
    }
    public async Task OnDeleteClick(string slug)
    {
        await _httpClient.DeleteAsync($"{_configuration["HttpClient:BaseAddress"]}categories/{slug}");
        var result = await _httpClient.GetAsJsonAsync<List<CategoryResponse>>($"{_configuration["HttpClient:BaseAddress"]}categories", query);
        Category = result.Result;
    }
    public async Task OnSaveCreateClick()
    {
        if (creatingEntity is not null)
            await _httpClient.PostAsJsonAsync<CategoryResponse>($"{_configuration["HttpClient:BaseAddress"]}categories", creatingEntity);
        creatingEntity.Name = string.Empty;
        var result = await _httpClient.GetAsJsonAsync<List<CategoryResponse>>($"{_configuration["HttpClient:BaseAddress"]}categories", query);
        Category = result.Result;
    }

    public async Task<ApiResponse<List<TrainingCategoriesResponce>>> GetTrainingCategories(GetTrainingCategoriesQuery getTrainingCategoriesQuery)
    {
        return await _httpClient.GetAsJsonAsync<List<TrainingCategoriesResponce>>($"{_configuration["HttpClient:BaseAddress"]}categories/training", getTrainingCategoriesQuery);

    }

    public async Task<List<TrainingCategoriesResponce>> GetTrainingCategoriesSinceCheckDate(GetTrainingCategoriesQuery getTrainingCategoriesQuery)
    {
        var respons = await _httpClient.GetAsJsonAsync<List<TrainingCategoriesResponce>>($"{_configuration["HttpClient:BaseAddress"]}categories/training?CheckDate=true", getTrainingCategoriesQuery);
        return respons.Success ? respons.Result : null;

    }

    public async Task<ApiResponse<List<InternshipCategoriesResponce>>> GetInternshipCategories(GetInternshipCategoriesQuery getInternshipCategoriesQuery)
    {
        return await _httpClient.GetAsJsonAsync<List<InternshipCategoriesResponce>>($"{_configuration["HttpClient:BaseAddress"]}categories/internships", getInternshipCategoriesQuery);

    }

    public async Task<List<InternshipCategoriesResponce>> GetInternshipCategoriesSinceCheckDate(GetInternshipCategoriesQuery getInternshipCategoriesQuery)
    {
        var respons = await _httpClient.GetAsJsonAsync<List<InternshipCategoriesResponce>>($"{_configuration["HttpClient:BaseAddress"]}categories/internships?CheckDate=true", getInternshipCategoriesQuery);
        return respons.Success ? respons.Result : null;
    }

    public async Task<ApiResponse<List<JobCategoriesResponse>>> GetJobCategories(GetJobCategoriesQuery getJobCategoriesQuery)
    {
        return await _httpClient.GetAsJsonAsync<List<JobCategoriesResponse>>($"{_configuration["HttpClient:BaseAddress"]}categories/job", getJobCategoriesQuery);

    }

    public async Task<List<JobCategoriesResponse>> GetJobCategoriesSinceCheckDate(GetJobCategoriesQuery getJobCategoriesQuery)
    {
        var respons= await _httpClient.GetAsJsonAsync<List<JobCategoriesResponse>>($"{_configuration["HttpClient:BaseAddress"]}categories/job?CheckDate=true", getJobCategoriesQuery);
        return respons.Success ? respons.Result : null;
    }
}
