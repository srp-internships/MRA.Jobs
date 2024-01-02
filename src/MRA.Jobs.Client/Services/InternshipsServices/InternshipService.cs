using MRA.Jobs.Application.Contracts.InternshipVacancies.Commands.Create;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Commands.Delete;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Commands.Update;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Queries.GetInternshipBySlug;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Responses;
using MRA.Jobs.Client.Services.HttpClients;

namespace MRA.Jobs.Client.Services.InternshipsServices;
public class InternshipService : IInternshipService
{
    private readonly IHttpClientService _httpClientService;
    public CreateInternshipVacancyCommand createCommand { get; set; }
    public UpdateInternshipVacancyCommand UpdateCommand { get; set; }
    public DeleteInternshipVacancyCommand DeleteCommand { get; set; }

    public InternshipService(IHttpClientService httpClientService)
    {
        _httpClientService = httpClientService;
        createCommand = new CreateInternshipVacancyCommand
        {
            Title = "",
            ShortDescription = "",
            Description = "",
            CategoryId = Guid.NewGuid(),
            PublishDate = DateTime.Now,
            EndDate = DateTime.Now,
            ApplicationDeadline = DateTime.Now,
            Duration = 0,
            Stipend = 0
        };
    }

    public async Task<ApiResponse> Create()
    {
        return await _httpClientService.PostAsJsonAsync<ApiResponse>("internships", createCommand);
    }

    public async Task<ApiResponse> Delete(string slug)
    {
        return await _httpClientService.DeleteAsync($"internships/{slug}");
    }

    public async Task<ApiResponse<List<InternshipVacancyListResponse>>> GetAll(GetInternshipVacancyBySlugQuery getInternshipVacancyBySlugQuery)
    {
        return await _httpClientService.GetAsJsonAsync<List<InternshipVacancyListResponse>>("internships", getInternshipVacancyBySlugQuery);
    }

    public async Task<InternshipVacancyResponse> GetBySlug(string slug, GetInternshipVacancyBySlugQuery getInternshipVacancyBySlugQuery)
    {
        var response = await _httpClientService.GetAsJsonAsync<InternshipVacancyResponse>($"internships/{slug}", getInternshipVacancyBySlugQuery);
        if (response.Success)
        {
            return response.Result;
        }
        else
        {
            return null;
        }
    }

    public async Task<ApiResponse> Update(string slug)
    {
        var updateCommand = new UpdateInternshipVacancyCommand
        {
            Slug = slug,
            Title = createCommand.Title,
            ShortDescription = createCommand.ShortDescription,
            Description = createCommand.Description,
            CategoryId = createCommand.CategoryId,
            PublishDate = createCommand.PublishDate,
            EndDate = createCommand.EndDate,
            ApplicationDeadline = createCommand.ApplicationDeadline,
            Duration = createCommand.Duration,
            Stipend = createCommand.Stipend,
            VacancyQuestions = createCommand.VacancyQuestions,
            VacancyTasks = createCommand.VacancyTasks
        };

        return await _httpClientService.PutAsJsonAsync<ApiResponse>($"internships/{slug}", updateCommand);
    }
}
