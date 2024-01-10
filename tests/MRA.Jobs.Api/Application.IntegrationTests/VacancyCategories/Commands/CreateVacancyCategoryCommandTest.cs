using System.Net;
using System.Net.Http.Json;
using MRA.Jobs.Application.Contracts.VacancyCategories.Commands.CreateVacancyCategory;
using NUnit.Framework;

namespace MRA.Jobs.Application.IntegrationTests.VacancyCategories.Commands;

public class CreateVacancyCategoryCommandTest : Testing
{
    [Test]
    public async Task CreateVacancyCategoryCommand_ShouldCreateVacancyCategory_Success()
    {
        var category = new CreateVacancyCategoryCommand { Name = "internship" };

        RunAsAdministratorAsync();
        var response = await _httpClient.PostAsJsonAsync("/api/categories", category);

        Assert.That(HttpStatusCode.Created == response.StatusCode);
    }
}