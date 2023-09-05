using MRA.Jobs.Application.Contracts.Common;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Commands;
using MRA.Jobs.Application.Contracts.InternshipVacancies.Responses;

namespace MRA.Jobs.Client.Services.InternshipsServices;

public interface IInternshipService
{
    Task<List<InternshipVacancyListResponse>> GetAll();
    Task<InternshipVacancyResponse> GetBySlug(string slug);

    Task<HttpResponseMessage> Create();
    Task<HttpResponseMessage> Update(string slug);
    Task Delete(string slug);

    Task<PagedList<InternshipVacancyListResponse>> GetAllSinceCheckDate();
    Task<PagedList<InternshipVacancyListResponse>> GetByCategorySinceCheckDate(string slug);
    Task<PagedList<InternshipVacancyListResponse>> SearchInternshipsSinceSearchDate(string searchInput);

    CreateInternshipVacancyCommand createCommand { get; set; }
    UpdateInternshipVacancyCommand UpdateCommand { get; set; }
    DeleteInternshipVacancyCommand DeleteCommand { get; set; }
}
