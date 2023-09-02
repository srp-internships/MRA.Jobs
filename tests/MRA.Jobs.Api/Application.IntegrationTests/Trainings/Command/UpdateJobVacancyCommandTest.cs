
using System.Net.Http.Json;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Commands;
using MRA.Jobs.Application.IntegrationTests;
using NUnit.Framework;

namespace MRA.Jobs.Application.IntegrationTests.Trainings.Command;
public class UpdateTrainingVacancyCommandTest : Testing
{

    [Test]
    public async Task UpdateTrainingVacancyCommand_UpdatingTrainingVacancyCommand_Success()
    {
        TrainingsContext Trainings = new TrainingsContext();
        var Training = await Trainings.GetTraining("TrainingVacncy");

        var updateCommand = new UpdateTrainingVacancyCommand
        {
            Title = "TrainingVacncy Updated",
            ShortDescription = Training.ShortDescription,
            Description = Training.ShortDescription,
            Duration = Training.Duration,
            Fees = Training.Fees,
            EndDate = Training.EndDate,
            PublishDate = Training.PublishDate,
            CategoryId = Training.CategoryId,
            Slug = Training.Slug,
        };

        var response = await _httpClient.PutAsJsonAsync($"/api/Trainings/{Training.Slug}", updateCommand);

        response.EnsureSuccessStatusCode();

    }
}
