using NUnit.Framework;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;
using FluentAssertions;
using System.Net.Http.Json;

namespace MRA.Jobs.Application.IntegrationTests.TrainingIntegrationTests;
public class TrainingsGetAllTest : Testing
{
    [Test]
    public async Task NoFilter_ShouldReturnAllTrainings()
    {
        var responce = await _httpClient.GetFromJsonAsync<PagedList<TrainingVacancyListDto>>("/api/trainings");
        responce.Items.Should().NotBeEmpty();
    }
    [Test]
    public async Task GetAllSinceCheckDate_ShouldReturnAllTrainingsSinceCheckDate()
    {
        var result = await _httpClient.GetFromJsonAsync<PagedList<TrainingVacancyListDto>>("/api/trainings?CheckDate=true");
        result.Items.Should().NotBeEmpty();
    }


    [Test]
    public async Task FilterByCategoryName_ShouldReturnCategriesTrainings()
    {
        string slug = "programmer";
        var training = await _httpClient.GetFromJsonAsync<PagedList<TrainingVacancyListDto>>($"/api/trainings?CategorySlug={slug}");

        training.Should().NotBeNull();
    }
    [Test]
    public async Task FilterByCategorySinceCheckDate_ShouldReturnAllTrainingsSinceCheckDate()
    {
        string slug = "programmer";
        var training = await _httpClient.GetFromJsonAsync<PagedList<TrainingVacancyListDto>>($"/api/trainings?CategorySlug={slug}&CheckDate=true");
        training.Items.Should().NotBeEmpty();
    }
    [Test]
    public async Task InvalidCategoryName_StatusCodeShouldNotIndicateSucces()
    {

        string slug = "lkjalksd";
        var responce = await _httpClient.GetFromJsonAsync<PagedList<TrainingVacancyListDto>>($"/api/trainings?CategorySlug={slug}");

        responce.Items.Should().BeEmpty();
    }


    [Test]
    public async Task SearchInput_ShouldReturnSearchedTrainigns()
    {
        string text = "internshipwegewrpgj";
        var trainings = await _httpClient.GetFromJsonAsync<PagedList<TrainingVacancyListDto>>($"api/trainings?SearchText={text}");
        trainings.Should().NotBeNull();
    }
    [Test]
    public async Task InvalidSearchInput_StatusCodeShouldNotIndicateSucces()
    {
        string text = "internship";
        var responce = await _httpClient.GetAsync($"trainings?SearchText={text}");

        responce.IsSuccessStatusCode.Should().BeFalse();
    }

}
