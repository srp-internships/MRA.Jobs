using System.Net;
using System.Net.Http.Json;
using MRA.Jobs.Application.Contracts.VacancyCategories.Commands;
using NUnit.Framework;

namespace MRA.Jobs.Application.IntegrationTests.VacancyCategories.Commands;
public class DeleteVacancyCategoryCommandTest : Testing
{
    [Test]
    public async Task DeleteVacancyCategoryCommand_ShouldDeleteVacancyCategory_Success()
    {
        //Create New VacancyCategory
        var createCommand = new CreateVacancyCategoryCommand { Name = "Category 3"};
        var createResponse = await _httpClient.PostAsJsonAsync("/api/categories", createCommand);
        createResponse.EnsureSuccessStatusCode();
        var createdCategorySlug = await createResponse.Content.ReadAsStringAsync();

        // Delete VacancyCategory
        var deleteResponse = await _httpClient.DeleteAsync($"/api/categories/{createdCategorySlug}");
        deleteResponse.EnsureSuccessStatusCode();

        // Get deleted VacancyCategory
        var getResponse = await _httpClient.GetAsync($"/api/categories/{createdCategorySlug}");
        Assert.AreEqual(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

}
