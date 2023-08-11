using MRA.Jobs.Application.Contracts.InternshipVacancies.Commands;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Responses;

namespace MRA.Jobs.Client.Services.InternshipsServices;

public interface IInternshipService
{
    CreateInternshipVacancyCommand createCommand { get; set; }
    UpdateInternshipVacancyCommand UpdateCommand { get; set; }
    DeleteInternshipVacancyCommand DeleteCommand { get; set; }
    Task<List<InternshipVacancyListResponse>> GetAll();
    Task<InternshipVacancyResponse> GetById(Guid id);

    Task<HttpResponseMessage> Create();
    Task<HttpResponseMessage> Update(Guid id);
    Task Delete(Guid id);
}