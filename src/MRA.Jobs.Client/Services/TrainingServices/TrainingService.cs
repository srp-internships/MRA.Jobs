using Microsoft.AspNetCore.Components.Authorization;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Commands.Create;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Commands.Delete;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Commands.Update;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;

namespace MRA.Jobs.Client.Services.TrainingServices;

public class TrainingService : ITrainingService
{
    private readonly HttpClient _httpClient;
    private readonly AuthenticationStateProvider _authenticationStateProvider;

    public TrainingService(HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider)
    {
        _httpClient = httpClient;
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

    public async Task<HttpResponseMessage> Create()
    {
        await _authenticationStateProvider.GetAuthenticationStateAsync();
        return await _httpClient.PostAsJsonAsync("trainings", createCommand);
    }

    public async Task Delete(string slug)
    {
        await _authenticationStateProvider.GetAuthenticationStateAsync();
        await _httpClient.DeleteAsync($"trainings/{slug}");
    }

    public async Task<HttpResponseMessage> Update(string slug)
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
            Fees = createCommand.Fees
        };
        
        await _authenticationStateProvider.GetAuthenticationStateAsync();
        return await _httpClient.PutAsJsonAsync($"trainings/{slug}", UpdateCommand);
    }

    public async Task<PagedList<TrainingVacancyListDto>> GetAll()
    {
        await _authenticationStateProvider.GetAuthenticationStateAsync();
        var result = await _httpClient.GetFromJsonAsync<PagedList<TrainingVacancyListDto>>("trainings");
        return result;
    }

    public async Task<TrainingVacancyDetailedResponse> GetBySlug(string slug)
    {
        await _authenticationStateProvider.GetAuthenticationStateAsync();
        return await _httpClient.GetFromJsonAsync<TrainingVacancyDetailedResponse>($"trainings/{slug}");
    }
}