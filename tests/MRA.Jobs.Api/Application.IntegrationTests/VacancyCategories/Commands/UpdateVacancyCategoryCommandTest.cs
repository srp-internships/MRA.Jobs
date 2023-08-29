using System.Net.Http.Json;
using FluentAssertions;
using MRA.Jobs.Application.Contracts.VacancyCategories.Commands;
using MRA.Jobs.Application.Contracts.VacancyCategories.Responses;
using Newtonsoft.Json;
using NUnit.Framework;

namespace MRA.Jobs.Application.IntegrationTests.VacancyCategories.Commands;
public class UpdateVacancyCategoryCommandTest : Testing
{
    [Test]
    public async Task UpdateVacancyCategoryCommand_ShouldCreateVacancyCategory_Success()
    {
        // Создайте новую категорию вакансий
        var createCommand = new CreateVacancyCategoryCommand { Name = "Design" };
        var slug = "design";
        var createResponse = await _httpClient.PostAsJsonAsync("/api/categories", createCommand);
        createResponse.EnsureSuccessStatusCode();
        var createdCategorySlug = await createResponse.Content.ReadAsStringAsync();
        Assert.AreEqual(slug, createdCategorySlug);

        // Обновите категорию вакансий
        var updateCommand = new UpdateVacancyCategoryCommand { Name = "Programming 2", Slug = createdCategorySlug };
        var updateResponse = await _httpClient.PutAsJsonAsync($"/api/categories/{updateCommand.Slug}", updateCommand);
        var updatedCategorySlug = await updateResponse.Content.ReadAsStringAsync();
        Assert.AreEqual(updateCommand.Slug, updatedCategorySlug);

        // Получите обновленную категорию вакансий
        var updatedResponse = await _httpClient.GetAsync($"/api/categories/{updateCommand.Slug}");
        updatedResponse.EnsureSuccessStatusCode();
        var updatedCategory = JsonConvert.DeserializeObject<CategoryResponse>(await updatedResponse.Content.ReadAsStringAsync());

        // Проверьте, что свойства категории вакансий были обновлены
        Assert.AreEqual(updateCommand.Name, updatedCategory.Name);
    }

}
