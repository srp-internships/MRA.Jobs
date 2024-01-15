using Microsoft.AspNetCore.Components.Authorization;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Commands.Create;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Commands.Delete;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Commands.Update;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;
using MRA.Jobs.SSR.Client.Services.HttpClients;

namespace MRA.Jobs.SSR.Client.Services.TrainingServices;

public class TrainingService(
    JobsApiHttpClientService httpClientService,
    AuthenticationStateProvider authenticationStateProvider)
    : ITrainingService
{
    public CreateTrainingVacancyCommand createCommand { get; set; } = new()
    {
        Title = "",
        CategoryId = Guid.NewGuid(),
        Description = "",
        ShortDescription = "",
        Duration = 0,
        EndDate = DateTime.Now,
        PublishDate = DateTime.Now,
        Fees = 0
    };

    public UpdateTrainingVacancyCommand UpdateCommand { get; set; }
    public DeleteTrainingVacancyCommand DeleteCommand { get; set; }

    public async Task<ApiResponse<string>> Create()
    {
        await authenticationStateProvider.GetAuthenticationStateAsync();
        return await httpClientService.PostAsJsonAsync<string>("trainings", createCommand);
    }

    public async Task<ApiResponse> Delete(string slug)
    {
        await authenticationStateProvider.GetAuthenticationStateAsync();
        return await httpClientService.DeleteAsync($"trainings/{slug}");
    }

    public async Task<ApiResponse> Update(string slug)
    {
        UpdateCommand = new UpdateTrainingVacancyCommand
        {
            Slug = slug,
            Title = createCommand.Title,
            Description = createCommand.Description,
            Duration = createCommand.Duration,
            EndDate = createCommand.EndDate,
            PublishDate = createCommand.PublishDate,
            ShortDescription = createCommand.ShortDescription,
            CategoryId = createCommand.CategoryId,
            Fees = createCommand.Fees,
            VacancyQuestions = createCommand.VacancyQuestions,
            VacancyTasks = createCommand.VacancyTasks
        };
        return await httpClientService.PutAsJsonAsync<string>($"trainings/{slug}", UpdateCommand);
    }

    public async Task<ApiResponse<PagedList<TrainingVacancyListDto>>> GetAll()
    {
        await authenticationStateProvider.GetAuthenticationStateAsync();
        var result = await httpClientService.GetAsJsonAsync<PagedList<TrainingVacancyListDto>>("trainings");
        return result;
    }

    public async Task<TrainingVacancyDetailedResponse> GetBySlug(string slug)
    {
        await authenticationStateProvider.GetAuthenticationStateAsync();
        var response = await httpClientService.GetAsJsonAsync<TrainingVacancyDetailedResponse>($"trainings/{slug}");
        return response.Success ? response.Result : null;
    }
}