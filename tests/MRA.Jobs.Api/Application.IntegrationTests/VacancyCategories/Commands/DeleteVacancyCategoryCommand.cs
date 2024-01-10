using System.Net;
using MRA.Jobs.Domain.Entities;
using NUnit.Framework;

namespace MRA.Jobs.Application.IntegrationTests.VacancyCategories.Commands;

public class DeleteVacancyCategoryCommand : Testing
{
    [Test]
    public async Task DeleteVacancyCategoryCommand_ShouldDeleteVacancyCategory_Success()
    {
        var category = new VacancyCategory { Name = "Test4", Slug = "test4" };
        await AddAsync(category);
        RunAsReviewerAsync();
        var response = await _httpClient.DeleteAsync($"/api/categories/{category.Slug}");
        response.EnsureSuccessStatusCode();

        var getResponse = await FindAsync<VacancyCategory>(category.Id);
        Assert.That(getResponse, Is.Null);
    }

    [Test]
    public async Task DeleteVacancyCategoryCommand_ReturnsNotFound()
    {
        RunAsReviewerAsync();
        var response = await _httpClient.DeleteAsync("/api/categories/category125");
        Assert.That(HttpStatusCode.NotFound == response.StatusCode);
    }
}