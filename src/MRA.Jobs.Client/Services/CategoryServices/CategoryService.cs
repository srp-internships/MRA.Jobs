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
        var result = await _http.GetFromJsonAsync<PaggedList<CategoryResponse>>("categories");
      
        Category = result.Items;
        creatingEntity = new() { Name = "" };
        return Category;
    }

    public void OnUpdateClick(CategoryResponse updateEntity)
    {
        updatingEntity = new()
        {
            Id = updateEntity.Id,
            Name = updateEntity.Name
        };
    }
    public async Task OnSaveUpdateClick()
    {
        var result = await _http.PutAsJsonAsync($"categories/{updatingEntity.Id}", updatingEntity);
        result.EnsureSuccessStatusCode();
        updatingEntity = null;
        var result2 = await _http.GetFromJsonAsync<PaggedList<CategoryResponse>>($"categories");
        Category = result2.Items;
    }
    public async Task OnDeleteClick(Guid id)
    {
        await _http.DeleteAsync($"category/{id}");
        var result = await _http.GetFromJsonAsync<PaggedList<CategoryResponse>>($"categories");
        Category = result.Items;
     

        //var result2 = await _http.GetFromJsonAsync<PaggedList<CategoryResponse>>($"category");
        //Category = result2.Items;
        //var result = await _http.GetFromJsonAsync<PaggedList<VacancyCategoryListDTO>>($"category");
        //Category = result.Items;
    }
    public async Task OnSaveCreateClick()
    {
        var ss = creatingEntity;
        if (creatingEntity is not null)
            await _http.PostAsJsonAsync("categories", creatingEntity);
        creatingEntity.Name = string.Empty;
        var result = await _http.GetFromJsonAsync<PaggedList<CategoryResponse>>($"categories");
        Category = result.Items;
    
    }


}
