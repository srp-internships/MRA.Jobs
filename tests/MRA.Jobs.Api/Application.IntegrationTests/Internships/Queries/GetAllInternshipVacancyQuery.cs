using System.Net;
using System.Net.Http.Json;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Responses;
using NUnit.Framework;

namespace MRA.Jobs.Application.IntegrationTests.Internships.Queries;
public class GetAllInternshipVacancyQuery : Testing
{

    private InternshipsContext _context;

    [SetUp]
    public void SetUp()
    {
        _context = new InternshipsContext();       
    }


    [Test]
    public async Task GetAllInternshipVacancyQuery_ReturnsInternshipVacancies()    {
     
        await _context.GetInternship("Internship1");
        await _context.GetInternship("Internship2");

        //Act
        var response = await _httpClient.GetAsync("/api/internships");

        //Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        var internshipVacancies = await response.Content.ReadFromJsonAsync<PagedList<InternshipVacancyListResponse>>();

        Assert.IsNotNull(internshipVacancies);
        Assert.IsNotEmpty(internshipVacancies.Items);
    }

}
