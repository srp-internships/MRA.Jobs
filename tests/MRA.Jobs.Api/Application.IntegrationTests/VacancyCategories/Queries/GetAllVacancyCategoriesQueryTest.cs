using System.Net;
using System.Net.Http.Json;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.VacancyCategories.Responses;
using MRA.Jobs.Application.IntegrationTests.VacancyCategories.GetCreate;
using NUnit.Framework;

namespace MRA.Jobs.Application.IntegrationTests.VacancyCategories.Queries;

public class GetAllVacancyCategoriesQueryTest : Testing
{
    private CategoryContext _categoryContext;

    [SetUp]
    public void SetUp()
    {
        _categoryContext = new CategoryContext();
    }

    [Test]
    public async Task GetAllVacancyCategoriesQuery_ReturnsInternshipVacancies()
    {
        //Arrange
        await _categoryContext.GetCategory("category0001");
        await _categoryContext.GetCategory("category0002");

        //Act
        var response = await _httpClient.GetAsync("/api/categories");

        //Assert
        Assert.That(HttpStatusCode.OK == response.StatusCode);
        var vacancyCategories = await response.Content.ReadFromJsonAsync<PagedList<CategoryResponse>>();
        Assert.That(vacancyCategories, Is.Not.Null);
        Assert.That(vacancyCategories.Items, Is.Not.Empty);
    }
}