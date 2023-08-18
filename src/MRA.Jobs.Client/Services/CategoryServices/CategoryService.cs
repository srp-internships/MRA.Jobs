﻿using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.VacancyTag.VacancyCategories.Commands;
using MRA.Jobs.Application.Contracts.VacancyTag.VacancyCategories.Responses;

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
        var result = await _http.GetFromJsonAsync<PagedList<CategoryResponse>>("categories");
        Category = result.Items;
        creatingEntity = new() { Name = "" };
        return result.Items;
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
        var result = await _http.PutAsJsonAsync($"categories/{updatingEntity.Slug}", updatingEntity);
        result.EnsureSuccessStatusCode();
        updatingEntity = null;
        var result2 = await _http.GetFromJsonAsync<PagedList<CategoryResponse>>($"categories");
        Category = result2.Items;
    }
    public async Task OnDeleteClick(string slug)
    {
        await _http.DeleteAsync($"categories/{slug}");
        var result = await _http.GetFromJsonAsync<PagedList<CategoryResponse>>($"categories");
        Category = result.Items;
    }
    public async Task OnSaveCreateClick()
    {
        if (creatingEntity is not null)
            await _http.PostAsJsonAsync("categories", creatingEntity);
        creatingEntity.Name = string.Empty;
        var result = await _http.GetFromJsonAsync<PagedList<CategoryResponse>>($"categories");
        Category = result.Items;
    }
}
