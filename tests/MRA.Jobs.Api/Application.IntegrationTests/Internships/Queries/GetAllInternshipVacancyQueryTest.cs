using System.Net;
using System.Net.Http.Json;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Responses;
using NUnit.Framework;

namespace MRA.Jobs.Application.IntegrationTests.Internships.Queries;
public class GetAllInternshipVacancyQueryTest : Testing
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
    
    [Test]
    public async Task GetAllJobVacancyQuery_ReturnsJobVacanciesCount2_ForApplicant()
    {
        ResetState();
        await _context.GetInternship("Internship1", DateTime.Now.AddDays(2));
        await _context.GetInternship("Internship2", DateTime.Now.AddDays(3));
        await _context.GetInternship("Internship3", DateTime.Now.AddDays(-2));

        RunAsDefaultUserAsync("applicant");
        //Act
        var response = await _httpClient.GetAsync("/api/internships");

        //Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        var internshipVacancies = await response.Content.ReadFromJsonAsync<PagedList<InternshipVacancyListResponse>>();

        Assert.IsNotNull(internshipVacancies);
        Assert.AreEqual(internshipVacancies.Items.Count, 2);
    }
    
    [Test]
    public async Task GetAllJobVacancyQuery_ReturnsJobVacanciesCount3_ForReviewer()
    {
        ResetState();
        await _context.GetInternship("Internship1", DateTime.Now.AddDays(2));
        await _context.GetInternship("Internship2", DateTime.Now.AddDays(3));
        await _context.GetInternship("Internship3", DateTime.Now.AddDays(-1));

        RunAsReviewerAsync();
        //Act
        var response = await _httpClient.GetAsync("/api/internships");

        //Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        var inertnshipVacancies = await response.Content.ReadFromJsonAsync<PagedList<InternshipVacancyListResponse>>();

        Assert.IsNotNull(inertnshipVacancies);
        Assert.AreEqual(inertnshipVacancies.Items.Count, 3);
    }

}
