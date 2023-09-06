using System.Net;
using System.Net.Http.Json;
using MRA.Jobs.Application.Contracts.VacancyCategories.Commands;
using MRA.Jobs.Domain.Entities;
using NUnit.Framework;

namespace MRA.Jobs.Application.IntegrationTests.VacancyCategories.Commands;
public class UpdateVacancyCategoryCommandTest : Testing
{
    [Test]
    public async Task UpdateVacancyCategoryCommand_ShouldUpdateVacancyCategoryCommand_Success()
    {
        var category = new VacancyCategory
        {
            Name = "Test",
            Slug = "test"
        };
        await AddAsync(category);

        var updateCategory = new UpdateVacancyCategoryCommand
        {
            Name = "Test2",
            Slug = category.Slug
        };

        var response = await _httpClient.PutAsJsonAsync($"/api/categories/{category.Slug}", updateCategory);

        response.EnsureSuccessStatusCode();
    }

    [Test]
    public async Task UpdateVacancyCategoryCommand_ReturnsNotFound()
    {
        var updateCategory = new UpdateVacancyCategoryCommand { Name = "Test2", Slug="test2" };
        var response = await _httpClient.PutAsJsonAsync($"/api/categories/{updateCategory.Slug}", updateCategory);
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
    }
}
