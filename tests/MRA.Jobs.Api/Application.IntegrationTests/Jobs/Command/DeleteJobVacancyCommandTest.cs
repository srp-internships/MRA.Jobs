using MRA.Jobs.Domain.Entities;
using NUnit.Framework;

namespace MRA.Jobs.Application.IntegrationTests.Jobs.Command;
public class DeleteJobVacancyCommandTest : Testing
{
    [Test]
    public async Task DeleteJobVacancyCommand_ShouldDeleteJobVacancyCommand_Success()
    {
        var Jobs = new JobsContext();
        RunAsReviewerAsync();
        var response = await _httpClient.DeleteAsync($"/api/jobs/{(
            await Jobs.GetJob("job1", DateTime.Now.AddDays(2))).Slug}");
            
        response.EnsureSuccessStatusCode();
    }
}
