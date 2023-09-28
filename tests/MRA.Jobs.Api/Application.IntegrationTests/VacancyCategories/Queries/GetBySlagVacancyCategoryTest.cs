using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MRA.Jobs.Application.Contracts.VacancyCategories.Queries.GetVacancyCategoryWithPagination;
using MRA.Jobs.Application.IntegrationTests.VacancyCategories.GetCreate;
using NUnit.Framework;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MRA.Jobs.Application.IntegrationTests.VacancyCategories.Queries;
public class GetBySlagVacancyCategoryTest : Testing
{
    private CategoryContext _categoryContext;

    [SetUp]
    public void SetUp()
    {
        _categoryContext = new CategoryContext();
    }

    [Test]
    public async Task GetVacancyCategoryBySlug_Returns_NotFound()
    {
        //Arrange 
        var query = new GetVacancyCategoryBySlugQuery { Slug = "ui" };

        //Act
        var response = await _httpClient.GetAsync($"/api/categories/{query.Slug}");

        //Assert 
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Test]
    public async Task GetVacancyCategoryBySlug_Returs_StatusOK()
    {
        //Arrange 
        var query = new GetVacancyCategoryBySlugQuery { Slug = (await _categoryContext.GetCategory("ux")).Slug };

        //Act
        var response = await _httpClient.GetAsync($"/api/categories/{query.Slug}");

        //Assert 
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }
}