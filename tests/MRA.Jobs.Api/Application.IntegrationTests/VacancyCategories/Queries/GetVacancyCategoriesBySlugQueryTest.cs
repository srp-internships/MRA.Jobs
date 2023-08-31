using MRA.Jobs.Domain.Entities;
using NUnit.Framework;

namespace MRA.Jobs.Application.IntegrationTests.VacancyCategories.Queries;
public class GetVacancyCategoriesBySlugQueryTest : Testing
{
    [Test]
    public async Task GivenValidQuery_ShouldReturnVacancyCategoryDetailsDTO()
    {
        // Arrange
        var newCategory = new VacancyCategory
        {
            Name = "Test Get",
            Slug = "test-get",
        };
        await AddAsync(newCategory);

        // Act
        var resonse = await _httpClient.GetAsync($"/api/categories/{newCategory.Slug}");
        var getVacancyCategory = await resonse.Content.ReadAsAsync<VacancyCategory>();
        // Assert
      
        Assert.AreEqual(newCategory.Id, getVacancyCategory.Id);
    }
 
}
