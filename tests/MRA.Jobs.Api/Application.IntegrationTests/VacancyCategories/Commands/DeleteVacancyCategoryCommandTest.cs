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
        var getResponse = await FindAsync<VacancyCategory>(newVacancyCategory.Id);
        Assert.IsNull(getResponse);
    }

}
