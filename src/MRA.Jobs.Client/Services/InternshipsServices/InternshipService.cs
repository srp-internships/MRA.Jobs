using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Commands;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Responses;
using MRA.Jobs.Application.Contracts.VacancyCategories.Responses;

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
    public List<CategoryResponse> Categories { get; set; }

    public async Task<HttpResponseMessage> Create(CreateInternshipVacancyCommand createCommand)
    {
        return await _http.PostAsJsonAsync("internships", createCommand);
    }

    public Task Delete(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<List<InternshipVacancyListResponce>> GetAll()
    {
        var result = await _http.GetFromJsonAsync<PaggedList<InternshipVacancyListResponce>>("internships");
        return result.Items;
    }

    public Task<InternshipVacancyResponce> GetById(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task Update(UpdateInternshipVacancyCommand updateCommand)
    {
        throw new NotImplementedException();
    }

    public async Task<List<CategoryResponse>> GetAllCategory()
    {
        var result = await _http.GetFromJsonAsync<PaggedList<CategoryResponse>>("categories");
        Categories = result.Items;
        return Categories;
    }
}
