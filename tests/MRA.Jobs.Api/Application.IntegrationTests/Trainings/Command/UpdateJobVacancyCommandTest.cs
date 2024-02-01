

using System.Net.Http.Json;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Commands.Update;
using NUnit.Framework;

namespace MRA.Jobs.Application.IntegrationTests.Trainings.Command;
public class UpdateTrainingVacancyCommandTest : TrainingsContext
{

    [Test]
    public async Task UpdateTrainingVacancyCommand_UpdatingTrainingVacancyCommand_Success()
    {
        var training = await GetTraining("TrainingVacancy2", DateTime.Now.AddDays(1));

        var updateCommand = new UpdateTrainingVacancyCommand
        {
            Title = "TrainingVacancy Updated",
            ShortDescription = training.ShortDescription,
            Description = training.ShortDescription,
            Duration = training.Duration,
            Fees = training.Fees,
            EndDate = training.EndDate,
            PublishDate = training.PublishDate,
            CategoryId = training.CategoryId,
            Slug = training.Slug,
        };

        RunAsReviewerAsync();
        var response = await _httpClient.PutAsJsonAsync($"/api/Trainings/{training.Slug}", updateCommand);

        response.EnsureSuccessStatusCode();

    }
}
