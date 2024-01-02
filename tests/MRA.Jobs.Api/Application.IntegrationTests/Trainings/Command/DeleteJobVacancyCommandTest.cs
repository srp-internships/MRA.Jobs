using MRA.Jobs.Application.Contracts.TrainingVacancies.Commands.Delete;
using NUnit.Framework;

namespace MRA.Jobs.Application.IntegrationTests.Trainings.Command;
public class DeleteTrainingVacancyCommandTest : Testing
{
    [Test]
    public async Task DeleteTrainingVacancyCommand_ShouldDeleteTrainingVacancyCommand_Success()
    {
        var trainings = new TrainingsContext();
        var training = await trainings.GetTraining("TrainingVacancy", DateTime.Now.AddDays(1));
        var deleteTraining = new DeleteTrainingVacancyCommand
        {
            Slug = training.Slug,
        };
        
        RunAsReviewerAsync();
        var response = await _httpClient.DeleteAsync($"/api/trainings/{training.Slug}");
            
        response.EnsureSuccessStatusCode();
    }
}
