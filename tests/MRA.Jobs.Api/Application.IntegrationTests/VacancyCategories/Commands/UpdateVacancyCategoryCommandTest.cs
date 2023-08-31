using System.Net.Http.Json;
using FluentAssertions;
using MRA.Jobs.Application.Contracts.VacancyCategories.Commands;
using MRA.Jobs.Application.Contracts.VacancyCategories.Responses;
using MRA.Jobs.Domain.Entities;
using Newtonsoft.Json;
using NUnit.Framework;

namespace MRA.Jobs.Application.IntegrationTests.VacancyCategories.Commands;
public class UpdateVacancyCategoryCommandTest : Testing
{
    [Test]
    public async Task UpdateVacancyCategoryCommand_ShouldCreateVacancyCategory_Success()
    {
        // new VacancyCategory
        var newVacancyCategory = new VacancyCategory
        {
            Name = "Design",
            Slug = "design"
        };
        await AddAsync(newVacancyCategory);

        // update VacancyCaterogy
        var updateCommand = new UpdateVacancyCategoryCommand { Name = "Programming 2", Slug = newVacancyCategory.Slug };
        var updateResponse = await _httpClient.PutAsJsonAsync($"/api/categories/{updateCommand.Slug}", updateCommand);
        var updatedCategorySlug = await updateResponse.Content.ReadAsStringAsync();
        Assert.AreEqual(updateCommand.Slug, updatedCategorySlug);

        // get updated VacancyCategory
        var updatedCategory = await FindAsync<VacancyCategory>(newVacancyCategory.Id);

        // Verify the job category properties have been updated
        Assert.AreNotEqual(newVacancyCategory.Name, updatedCategory.Name);
    }
}


