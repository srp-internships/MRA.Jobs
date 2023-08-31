using System.Net;
using System.Net.Http.Json;
using MRA.Jobs.Application.Contracts.VacancyCategories.Commands;
using MRA.Jobs.Domain.Entities;
using NUnit.Framework;

namespace MRA.Jobs.Application.IntegrationTests.VacancyCategories.Commands;
public class DeleteVacancyCategoryCommandTest : Testing
{
    [Test]
    public async Task DeleteVacancyCategoryCommand_ShouldDeleteVacancyCategory_Success()
    {
        //Create New VacancyCategory
        var newVacancyCategory = new VacancyCategory
        {
            Name = "Test",
            Slug = "test",
        };
        await AddAsync(newVacancyCategory);
       

        // Delete VacancyCategory
        var deleteResponse = await _httpClient.DeleteAsync($"/api/categories/{newVacancyCategory.Slug}");
        deleteResponse.EnsureSuccessStatusCode();

        // Get deleted VacancyCategory
        var getResponse = await _httpClient.GetAsync($"/api/categories/{newVacancyCategory.Slug}");
        Assert.AreEqual(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

}
