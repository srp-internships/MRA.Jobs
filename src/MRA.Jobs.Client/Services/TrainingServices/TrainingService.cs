using Microsoft.AspNetCore.Components.Authorization;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Commands.Create;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Commands.Delete;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Commands.Update;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Queries;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;
using MRA.Jobs.Client.Services.HttpClients;

namespace MRA.Jobs.Client.Services.TrainingServices;

public class TrainingService : ITrainingService
{
    private readonly IHttpClientService _httpClientService;
    private readonly AuthenticationStateProvider _authenticationStateProvider;

    public TrainingService(IHttpClientService httpClientService, AuthenticationStateProvider authenticationStateProvider)
    {
        _httpClientService = httpClientService;
        _authenticationStateProvider = authenticationStateProvider;
        createCommand = new CreateTrainingVacancyCommand
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
    }

    public CreateTrainingVacancyCommand createCommand { get; set; }
    public UpdateTrainingVacancyCommand UpdateCommand { get; set; }
    public DeleteTrainingVacancyCommand DeleteCommand { get; set; }

    public async Task<ApiResponse> Create()
    {
        await _authenticationStateProvider.GetAuthenticationStateAsync();
        return await _httpClientService.PostAsJsonAsync<ApiResponse>("trainings", createCommand);
    }

    public async Task<ApiResponse> Delete(string slug)
    {
        await _authenticationStateProvider.GetAuthenticationStateAsync();
        return await _httpClientService.DeleteAsync($"trainings/{slug}");
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

        await _authenticationStateProvider.GetAuthenticationStateAsync();
        return await _httpClientService.PutAsJsonAsync<ApiResponse>($"trainings/{slug}", UpdateCommand);
    }

    public async Task<ApiResponse<PagedList<TrainingVacancyListDto>>> GetAll(GetTrainingVacancyBySlugQuery getTrainingVacancyBySlug)
    {
        await _authenticationStateProvider.GetAuthenticationStateAsync();
        var result = await _httpClientService.GetAsJsonAsync<PagedList<TrainingVacancyListDto>>("trainings", getTrainingVacancyBySlug);
        return result;
    }

    public async Task<TrainingVacancyDetailedResponse> GetBySlug(string slug, GetTrainingVacancyBySlugQuery getTrainingVacancyBySlug)
    {
        await _authenticationStateProvider.GetAuthenticationStateAsync();
        var response = await _httpClientService.GetAsJsonAsync<TrainingVacancyDetailedResponse>($"trainings/{slug}", getTrainingVacancyBySlug);
        if (response.Success)
        {
            return response.Result;
        }
        else
        {
            return null;
        }
    }
}