using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.VacancyCategories.Commands;
using MRA.Jobs.Application.Contracts.VacancyCategories.Responses;

namespace MRA.Jobs.Client.Services.CategoryServices;

public class CategoryService : ICategoryService
{
    private readonly HttpClient _http;

    public CategoryService(HttpClient http)
    {
        _http = http;
    }

    public List<CategoryResponse> Category { get; set; }
    public UpdateVacancyCategoryCommand updatingEntity { get; set; }
    public DeleteVacancyCategoryCommand deletingEntity { get; set; }
    public CreateVacancyCategoryCommand creatingEntity { get; set; }

    public async Task<List<CategoryResponse>> GetAllCategory()
    {
        PagedList<CategoryResponse> result = await _http.GetFromJsonAsync<PagedList<CategoryResponse>>("categories");
        Category = result.Items;
        creatingEntity = new CreateVacancyCategoryCommand { Name = "" };
        return result.Items;
    }

    public void OnUpdateClick(CategoryResponse updateEntity)
    {
        updatingEntity = new UpdateVacancyCategoryCommand { Id = updateEntity.Id, Name = updateEntity.Name };
    }

    public async Task OnSaveUpdateClick()
    {
        HttpResponseMessage result = await _http.PutAsJsonAsync($"categories/{updatingEntity.Id}", updatingEntity);
        result.EnsureSuccessStatusCode();
        updatingEntity = null;
        PagedList<CategoryResponse> result2 = await _http.GetFromJsonAsync<PagedList<CategoryResponse>>("categories");
        Category = result2.Items;
    }

    public async Task OnDeleteClick(Guid id)
    {
        await _http.DeleteAsync($"categories/{id}");
        PagedList<CategoryResponse> result = await _http.GetFromJsonAsync<PagedList<CategoryResponse>>("categories");
        Category = result.Items;
    }

    public async Task OnSaveCreateClick()
    {
        if (creatingEntity is not null)
        {
            await _http.PostAsJsonAsync("categories", creatingEntity);
        }

        creatingEntity.Name = string.Empty;
        PagedList<CategoryResponse> result = await _http.GetFromJsonAsync<PagedList<CategoryResponse>>("categories");
        Category = result.Items;
    }
}