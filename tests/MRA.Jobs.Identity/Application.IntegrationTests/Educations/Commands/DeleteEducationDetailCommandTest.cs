using MRA.Identity.Domain.Entities;

namespace MRA.Jobs.Application.IntegrationTests.Educations.Commands;
public class DeleteEducationDetailCommandTest : BaseTest
{
    [Test]
    public async Task DeleteEducationDetailCommand_ShouldDeleteEducationDetiel_Success()
    {
        await AddApplicantAuthorizationAsync();

        var education = new EducationDetail()
        {
            Speciality = "test4",
            University = "test4",
            EndDate = DateTime.Now.AddYears(-1),
            StartDate = DateTime.Now.AddYears(-4),
            UntilNow = false,
            UserId = Applicant.Id,
        };
        await AddEntity(education);

        var response = await _client.DeleteAsync($"/api/Profile/DeleteEducationDetail/{education.Id}");

        response.EnsureSuccessStatusCode();
    }
}
