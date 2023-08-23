using System.Net.Http.Json;
using FluentAssertions;
using MRA.Jobs.Application.Contracts.VacancyCategories.Responses;
using NUnit.Framework;

namespace MRA.Jobs.Application.IntegrationTests.VacancyCategories.Queries;
public class GetCategoriesBySlugTest : Testing
{
    [Test]
    public async Task ValidSlug_ShouldReturnMatchingCategory()
    {
        string slug = "programmer";
        var responce = await _httpClient.GetFromJsonAsync<CategoryResponse>($"api/categories/{slug}");

        responce.Slug.Should().Be(slug);
    }
    [Test]
    public async Task InvalidSlug_ShouldReturnEmptyCategory()
    {
        string slug = "fjhs";
        var responce = await _httpClient.GetAsync($"api/categories/{slug}");

        responce.IsSuccessStatusCode.Should().BeFalse();
    }
    [Test]
    public async Task ShouldGetAllCategories()
    {
        var responce = await _httpClient.GetAsync("/api/categories");

        responce.EnsureSuccessStatusCode();
    }
}
