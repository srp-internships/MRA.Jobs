using Microsoft.AspNetCore.Components.Authorization;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Commands.Create;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Commands.Delete;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Commands.Update;
using MRA.Jobs.Application.Contracts.TrainingVacancies.Responses;
using MRA.Jobs.Client.Services.HttpClients;

namespace MRA.Jobs.Client.Services.TrainingServices;

public class TrainingService : ITrainingService
{
    private readonly IHttpClientService _httpClientService;
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    private readonly IConfiguration _configuration;

    public TrainingService(IHttpClientService httpClientService, AuthenticationStateProvider authenticationStateProvider, IConfiguration configuration)
    {
        _httpClientService = httpClientService;
        _authenticationStateProvider = authenticationStateProvider;
        _configuration = configuration;
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
        return await _httpClientService.PostAsJsonAsync<ApiResponse>($"{_configuration["HttpClient:BaseAddress"]}trainings", createCommand);
    }

    public async Task<ApiResponse> Delete(string slug)
    {
        await _authenticationStateProvider.GetAuthenticationStateAsync();
        return await _httpClientService.DeleteAsync($"{_configuration["HttpClient:BaseAddress"]}trainings/{slug}");
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
        return await _httpClientService.PutAsJsonAsync<ApiResponse>($"{_configuration["HttpClient:BaseAddress"]}trainings/{slug}", UpdateCommand);
    }

    public async Task<ApiResponse<PagedList<TrainingVacancyListDto>>> GetAll()
    {
        await _authenticationStateProvider.GetAuthenticationStateAsync();
        var result = await _httpClientService.GetAsJsonAsync<PagedList<TrainingVacancyListDto>>($"{_configuration["HttpClient:BaseAddress"]}trainings");
        return result;
    }

    public async Task<TrainingVacancyDetailedResponse> GetBySlug(string slug)
    {
        await _authenticationStateProvider.GetAuthenticationStateAsync();
        var response = await _httpClientService.GetAsJsonAsync<TrainingVacancyDetailedResponse>($"{_configuration["HttpClient:BaseAddress"]}trainings/{slug}");
        return response.Success ? response.Result : null;
    }
}