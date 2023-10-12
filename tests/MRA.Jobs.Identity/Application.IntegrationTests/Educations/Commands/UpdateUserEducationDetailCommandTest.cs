using System.Net.Http.Json;
using MRA.Identity.Application.Contract.Educations.Command.Update;
using MRA.Identity.Domain.Entities;

namespace MRA.Jobs.Application.IntegrationTests.Educations.Commands;
public class UpdateUserEducationDetailCommandTest : BaseTest
{
    [Test]
    public async Task UpdateUserEductionDetailCommand_ShouldUpdateUserEductionDetailCommand_Success()
    {
        await AddApplicantAuthorizationAsync();

        var education = new EducationDetail()
        {
            Speciality = "test",
            University = "test",
            StartDate = DateTime.Now.AddYears(-5),
            EndDate = DateTime.Now.AddYears(-1),
            UntilNow = false,
            UserId = Applicant.Id,
        };

        await AddEntity(education);

        var command = new UpdateEducationDetailCommand()
        {
            Id = education.Id,
            Speciality = "test3",
            University = "test3",
            StartDate = education.StartDate,
            EndDate = education.EndDate,
            UntilNow = education.UntilNow,
        };

        var response2 = await _client.PutAsJsonAsync("/api/Profile/UpdateEducationDetail", command);

        response2.EnsureSuccessStatusCode();
    }
}
