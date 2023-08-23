using System.Net.Http.Json;
using FluentAssertions;
using MRA.Jobs.Application.Contracts.VacancyCategories.Commands;
using NUnit.Framework;
namespace MRA.Jobs.Application.IntegrationTests.VacancyCategories.Commands;
public class CreateVacancyCategoryCommandTest:Testing
{
    [Test]
    public async Task CreateVacancyCategoryCommand_ShouldCreateVacancyCategory_Success()
    {
        var category = new CreateVacancyCategoryCommand { Name = "internship" };

        var response = await _httpClient.PostAsJsonAsync("/api/categories", category);

        response.EnsureSuccessStatusCode();

        var responseGuid = await response.Content.ReadAsStringAsync();

        responseGuid.Should().NotBeEmpty();
    }
}
