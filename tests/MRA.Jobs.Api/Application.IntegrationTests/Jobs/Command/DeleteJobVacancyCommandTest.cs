using MRA.Jobs.Domain.Entities;
using NUnit.Framework;

namespace MRA.Jobs.Application.IntegrationTests.Jobs.Command;
public class DeleteJobVacancyCommandTest : JobsContext
{
    [Test]
    public async Task DeleteJobVacancyCommand_ShouldDeleteJobVacancyCommand_Success()
    {
        RunAsReviewerAsync();
        var response = await _httpClient.DeleteAsync($"/api/jobs/{(
            await GetJob("job1", DateTime.Now.AddDays(2))).Slug}");
            
        response.EnsureSuccessStatusCode();
    }
}
