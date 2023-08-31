using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using MRA.Jobs.Application.Contracts.Applications.Commands;
using MRA.Jobs.Domain.Entities;
using NUnit.Framework;

namespace MRA.Jobs.Application.IntegrationTests.Application;
public class CreateApplicationCommandTest:Testing
{
    private static Random random = new Random();

    [Test]
    public async Task CreateApplicationCommand_CreatingApplication_Success()
    {
        RunAsDefaultUserAsync();
        var vacancyId = await AddJobVacancy();
        var testSubmit = new CreateApplicationCommand
        {
            VacancyId = vacancyId,
            CoverLetter = RandomString(150)
        };
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.PostAsJsonAsync("/api/applications", testSubmit);

        response.EnsureSuccessStatusCode();

        var responseGuid = await response.Content.ReadAsStringAsync();

        responseGuid.Should().NotBeEmpty();
    }
    

    async Task<Guid> AddVacancyCategory(string name)
    {
        var vacancyCategory = new VacancyCategory
        {
            Name = name,
            Id = Guid.NewGuid(),
        };
        await AddAsync(vacancyCategory);
        return vacancyCategory.Id;
    }

    async Task<Guid> AddJobVacancy()
    {
        var internshipVacancy = new InternshipVacancy
        {
            Id = Guid.NewGuid(),
            Title = "foobar",
            Description = RandomString(50),
            ShortDescription = RandomString(10),
            Stipend = 100,
            Duration = 3,
            PublishDate = DateTime.Now,
            ApplicationDeadline = DateTime.Now.AddDays(2),
            CategoryId = await AddVacancyCategory("internship")
        };
        await AddAsync(internshipVacancy);
        return internshipVacancy.Id;
    }

    public static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
