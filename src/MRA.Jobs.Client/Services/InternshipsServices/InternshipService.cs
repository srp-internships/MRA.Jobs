using System.Net.Http;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Commands.Create;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Commands.Delete;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Commands.Update;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Responses;

namespace MRA.Jobs.Client.Services.InternshipsServices;

public class InternshipService : IInternshipService
{
    private readonly HttpClient _http;

    public InternshipService(HttpClient http)
    {
        _http = http;
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

    public CreateInternshipVacancyCommand createCommand { get; set; }
    public UpdateInternshipVacancyCommand UpdateCommand { get; set; }
    public DeleteInternshipVacancyCommand DeleteCommand { get; set; }


    public async Task<HttpResponseMessage> Create()
    {
        return await _http.PostAsJsonAsync("internships", createCommand);
    }

    public async Task Delete(string slug)
    {
        await _http.DeleteAsync($"internships/{slug}");
    }

    public async Task<List<InternshipVacancyListResponse>> GetAll()
    {
        var result = await _http.GetFromJsonAsync<PagedList<InternshipVacancyListResponse>>("internships");
        return result.Items;
    }

    public async Task<InternshipVacancyResponse> GetBySlug(string slug)
    {
        return await _http.GetFromJsonAsync<InternshipVacancyResponse>($"internships/{slug}");
    }

    public async Task<HttpResponseMessage> Update(string slug)
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
        };

        return await _http.PutAsJsonAsync($"internships/{slug}", updateCommand);
    }

}
