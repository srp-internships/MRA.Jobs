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
    
        var slug = "design";
        await AddAsync(new VacancyCategory {
            Name = "Design",
            Slug = slug });

        // Обновите категорию вакансий
        var updateCommand = new UpdateVacancyCategoryCommand { Name = "Programming 2", Slug = slug };
        var updateResponse = await _httpClient.PutAsJsonAsync($"/api/categories/{updateCommand.Slug}", updateCommand);
        var updatedCategorySlug = await updateResponse.Content.ReadAsStringAsync();
        Assert.AreEqual(updateCommand.Slug, updatedCategorySlug);

        // Получите обновленную категорию вакансий
        var updatedCategory = await FindBySlugAsync<VacancyCategory>(updateCommand.Slug);

        // Проверьте, что свойства категории вакансий были обновлены
        Assert.AreEqual(updateCommand.Name, updatedCategory.Name);
    }
}


