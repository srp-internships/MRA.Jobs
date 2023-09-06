using MRA.Jobs.Domain.Entities;
using NUnit.Framework;

namespace MRA.Jobs.Application.IntegrationTests.Internships.Command;
public class DeleteInternshipVacancyCommandTest : Testing
{
    [Test]
    public async Task DeleteInternshipVacancyCommand_ShouldDeleteInternshipVacancyCommand_Success()
    {
        var internships = new InternshipsContext();

        var response = await _httpClient.DeleteAsync($"/api/internships/{(await internships.GetInternship("IniternshipVacancy")).Slug}");
            
        response.EnsureSuccessStatusCode();
    }
}
