using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Commands.Create;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Commands.Delete;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Commands.Update;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Responses;
using MRA.Jobs.Client.Services.HttpClients;

namespace MRA.Jobs.Client.Services.InternshipsServices;
public class InternshipService(JobsApiHttpClientService httpClientService)
    : IInternshipService
{
    public CreateInternshipVacancyCommand createCommand { get; set; } = new()
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

    public UpdateInternshipVacancyCommand UpdateCommand { get; set; }
    public DeleteInternshipVacancyCommand DeleteCommand { get; set; }

    public async Task<ApiResponse> Create()
    {
        return await httpClientService.PostAsJsonAsync<string>("internships", createCommand);
    }

    public async Task<ApiResponse> Delete(string slug)
    {
        return await httpClientService.DeleteAsync($"internships/{slug}");
    }

    public async Task<ApiResponse<PagedList<InternshipVacancyListResponse>>> GetAll()
    {
        var response= await httpClientService.GetAsJsonAsync<PagedList<InternshipVacancyListResponse>>("internships");
        return response;
    }

    public async Task<InternshipVacancyResponse> GetBySlug(string slug)
    {
        var response = await httpClientService.GetAsJsonAsync<InternshipVacancyResponse>($"internships/{slug}");
        return response.Success ? response.Result : null;
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

        return await httpClientService.PutAsJsonAsync<string>($"internships/{slug}", updateCommand);
    }
}
