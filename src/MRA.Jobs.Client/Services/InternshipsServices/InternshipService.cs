using System.Net.Http.Json;
using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Commands;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Responses;
using MRA.Jobs.Application.Contracts.JobVacancies.Responses;
using MRA.Jobs.Client.Components.Client;

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

    public  async Task Delete(Guid id)
    {
        DeleteCommand = new DeleteInternshipVacancyCommand
        {
            Id = id
        };
        await  _http.DeleteFromJsonAsync<DeleteInternshipVacancyCommand>($"internships/{id}");
    }

    public async Task<List<InternshipVacancyListResponce>> GetAll()
    {
        var result = await _http.GetFromJsonAsync<PaggedList<InternshipVacancyListResponce>>("internships");
        return result.Items;
    }

    public async Task<InternshipVacancyResponce> GetById(Guid id)
    {
        return await _http.GetFromJsonAsync<InternshipVacancyResponce>($"internships/{id}");
    }

    public async Task<HttpResponseMessage> Update(Guid id)
    {
        var updateCommand = new UpdateInternshipVacancyCommand
        {
            Id = id,
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

        return await _http.PutAsJsonAsync($"internships/{id}", updateCommand);
    }

}
