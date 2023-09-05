using MRA.Jobs.Domain.Entities;
using NUnit.Framework;

namespace MRA.Jobs.Application.IntegrationTests.Jobs.Command;
public class DeleteJobVacancyCommandTest : Testing
{
    [Test]
    public async Task DeleteJobVacancyCommand_ShouldDeleteJobVacancyCommand_Success()
    {
        var Jobs = new JobsContext();

        var response = await _httpClient.DeleteAsync($"/api/jobs/{(await Jobs.GetJob("JobVacancy")).Slug}");
            
        response.EnsureSuccessStatusCode();
    }
}
