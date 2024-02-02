using MRA.Jobs.Domain.Entities;
using NUnit.Framework;

namespace MRA.Jobs.Application.IntegrationTests.Internships.Command;
public class DeleteInternshipVacancyCommandTest : InternshipsContext
{
    [Test]
    public async Task DeleteInternshipVacancyCommand_ShouldDeleteInternshipVacancyCommand_Success()
    {
        RunAsReviewerAsync();
        var response = await _httpClient.DeleteAsync($"/api/internships/{(await GetInternship("IniternshipVacancy")).Slug}");
            
        response.EnsureSuccessStatusCode();
    }
}
