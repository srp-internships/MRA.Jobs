using FluentAssertions;
using NUnit.Framework;

namespace MRA.Jobs.Application.IntegrationTests.TrainingIntegrationTests;

public class TrainingsGetBySlugTest : Testing
{
    [Test]
    public async Task ValidSlug_ShouldReturnTrainings()
    {
        string slug = "c-internship-2023-8";
        var responce = await _httpClient.GetAsync($"/api/trainings/{slug}");

        responce.EnsureSuccessStatusCode();
        responce.Should().NotBeNull();
    }

    [Test]
    public async Task InValidSlug_ShouldReturnNull()
    {
        string slug = "asf";
        var responce = await _httpClient.GetAsync($"/api/trainings/{slug}");

        responce.IsSuccessStatusCode.Should().BeFalse();
    }
}
